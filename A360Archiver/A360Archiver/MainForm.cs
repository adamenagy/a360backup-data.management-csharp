using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Forge;
using Autodesk.Forge.Model;
using System.Net.Http;
using System.Web;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using RestSharp;

namespace A360Archiver
{
    public partial class MainForm : Form
    {
        public Dictionary<DownloadState, Color> downloadStateToColor = new Dictionary<DownloadState, Color>()
        {
            { DownloadState.Default, Color.Empty },
            { DownloadState.Downloaded, Color.Green },
            { DownloadState.Downloading, Color.Orange },
            { DownloadState.Failed, Color.Red },
            { DownloadState.Waiting, Color.Yellow }
        };

        public enum DownloadState
        {
            Default,
            Downloading,
            Downloaded,
            Waiting,
            Failed
        };

        // Also used for setting the icon for the node
        // that's why we associate value with them
        public enum NodeType
        {
            Hub = 0,
            Project = 1,
            Folder = 2,
            Item = 3,
            Version = 4
        };

        public class MyTreeNode : TreeNode
        {
            public MyTreeNode(string id, string text, string a360Type, string fileType, NodeType nodeType)
            {
                this.id = id;
                this.Text = text;
                this.a360Type = a360Type;
                this.fileType = fileType;
                this.nodeType = nodeType;
                this.ImageIndex = this.SelectedImageIndex = (int)nodeType;
                this.nodeState = DownloadState.Default;
            }

            public bool isFusionFile()
            {
                return (a360Type.StartsWith("versions:autodesk.fusion360:"));
            }

            public string id;
            public string a360Type;
            public string fileType;
            public NodeType nodeType;
            public DownloadState nodeState;
        }

        public class MyListItem : ListViewItem
        {
            public MyListItem(MyTreeNode node, string localPath)
            {
                this.node = node;
                this.localPath = localPath;
                this.Text = localPath;
                this.fileState = DownloadState.Default;
                this.UseItemStyleForSubItems = false;
            }

            public override string ToString()
            {
                return (this.fileState == DownloadState.Default) ? localPath : localPath + " [" + this.fileState.ToString() + "]";
            }

            public MyTreeNode node;
            public string localPath;
            public DownloadState fileState;
        }

        private LogInInfo logInInfo = new LogInInfo();
        private MyTreeNode nodeToDownload;
        private List<MyTreeNode> nodesLoading = new List<MyTreeNode>();
        const char kUpdateChar = '\u21bb';
        const int kTokenRefreshMultiplier = 900; // should be 900
        const int kTimeOutDelay = 5000;
        Timer timer = new Timer();

        private DialogResult showLogIn()
        {
            LogInForm logIn = new LogInForm(ref logInInfo);
            return logIn.ShowDialog();
        }

        public async Task<bool> refreshToken()
        {
            Debug.Print("refreshToken : Refreshing token...");

            try
            {
                var authApi = new ThreeLeggedApi();
                var response = await authApi.RefreshtokenAsync(
                logInInfo.clientId, logInInfo.clientSecret, "refresh_token", logInInfo.refreshToken);

                logInInfo.accessToken = response["access_token"];
                logInInfo.refreshToken = response["refresh_token"];
                logInInfo.expiresIn = response["expires_in"];
            }
            catch (Exception ex)
            {
                Debug.Print("refreshToken >> catch : " + ex.Message);
                return false;
            }

            return true;
        }

        public async void refreshTokenTick(object sender, EventArgs e)
        {
            timer.Stop();

            while (!await refreshToken());

            timer.Interval = (int)logInInfo.expiresIn * kTokenRefreshMultiplier; // do it 10% before it actually expires    
            timer.Start();

            Debug.Print("refreshToken : Refreshed token...");
        }

        // This way the file list will not flicker when things change in it
        public void useDoubleBuffered(Control control, bool enable)
        {
            var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            doubleBufferPropertyInfo.SetValue(control, enable, null);
        }

        public MainForm()
        {
            InitializeComponent();

            useDoubleBuffered(this.ltvFiles, true);
            useDoubleBuffered(this.treeView, true);

            if (showLogIn() == DialogResult.OK)
            {
                // Register for refreshing token
                timer.Tick += refreshTokenTick;
                timer.Interval = (int)logInInfo.expiresIn * kTokenRefreshMultiplier; // do it 10% before it actually expires             
                timer.Start();

                // 
                listHubs();
            }
            else
            {
                this.Load += (s, e) => Close();
            }
        }

        public delegate void DelegateSetNodeState(MyTreeNode node, bool isUpdating);
        private void setNodeState(MyTreeNode node, bool isUpdating)
        {
            if (treeView.InvokeRequired)
                treeView.Invoke(new DelegateSetNodeState(this.setNodeState), new Object[] { node, isUpdating });
            else
            {
                if (isUpdating)
                {
                    node.nodeState = DownloadState.Downloading;
                    nodesLoading.Add(node);

                    // If the update character is not there, then add it
                    if (node.Text[node.Text.Length - 1] != kUpdateChar)
                    {
                        node.Text += kUpdateChar;
                        node.BackColor = Color.Yellow;
                    }
                }
                else
                {
                    node.nodeState = DownloadState.Downloaded;
                    nodesLoading.Remove(node);

                    // If the update character is there, then remove it
                    if (node.Text[node.Text.Length - 1] == kUpdateChar)
                    {
                        node.Text = node.Text.Remove(node.Text.Length - 1);
                        node.BackColor = Color.Empty;
                    }

                    if (nodesLoading.Count == 0)
                    {
                        btnBackup.Enabled = true;
                    }
                }
            }
        }

        public delegate void DelegateAddToTreeView(TreeNode parentNode, MyTreeNode childNode);
        private void addToTreeView(TreeNode parentNode, MyTreeNode childNode)
        {
            if (treeView.InvokeRequired)
                treeView.Invoke(new DelegateAddToTreeView(this.addToTreeView), new Object[] { parentNode, childNode });
            else
            {
                TreeNodeCollection nodes;
                if (parentNode == null)
                    nodes = treeView.Nodes;
                else
                    nodes = parentNode.Nodes;

                nodes.Add(childNode);
            }
        }

        private async void listHubs()
        {
            var hubsApi = new HubsApi();
            hubsApi.Configuration.AccessToken = logInInfo.accessToken;

            var hubs = await hubsApi.GetHubsAsync();
            foreach (KeyValuePair<string, dynamic> hubInfo in new DynamicDictionaryItems(hubs.data))
            {
                MyTreeNode hubNode = new MyTreeNode(
                    hubInfo.Value.links.self.href,
                    hubInfo.Value.attributes.name,
                    hubInfo.Value.attributes.extension.type,
                    "",
                    NodeType.Hub
                );
                addToTreeView(null, hubNode);
                listProjects(hubNode);
            }
        }

        private async void listProjects(MyTreeNode hubNode)
        {
            var projectsApi = new ProjectsApi();
            projectsApi.Configuration.AccessToken = logInInfo.accessToken;

            string[] idParams = hubNode.id.Split('/');
            string hubId = idParams[idParams.Length - 1];

            setNodeState(hubNode, true);
            var projects = await projectsApi.GetHubProjectsAsync(hubId);
            foreach (KeyValuePair<string, dynamic> projectInfo in new DynamicDictionaryItems(projects.data))
            {
                MyTreeNode projectNode = new MyTreeNode(
                    projectInfo.Value.links.self.href,
                    projectInfo.Value.attributes.name,
                    projectInfo.Value.attributes.extension.type,
                    "",
                    NodeType.Project
                );
                addToTreeView(hubNode, projectNode);
            }
            setNodeState(hubNode, false);
        }

        private async void listTopFolders(MyTreeNode projectNode)
        {
            var projectsApi = new ProjectsApi();
            projectsApi.Configuration.AccessToken = logInInfo.accessToken;

            string[] idParams = projectNode.id.Split('/');
            string hubId = idParams[idParams.Length - 3];
            string projectId = idParams[idParams.Length - 1];

            setNodeState(projectNode, true);
            var folders = await projectsApi.GetProjectTopFoldersAsync(hubId, projectId);
            foreach (KeyValuePair<string, dynamic> folderInfo in new DynamicDictionaryItems(folders.data))
            {
                MyTreeNode folderNode = new MyTreeNode(
                    folderInfo.Value.links.self.href,
                    folderInfo.Value.attributes.displayName,
                    folderInfo.Value.attributes.extension.type,
                    "",
                    NodeType.Folder
                );
                addToTreeView(projectNode, folderNode);
            }
            setNodeState(projectNode, false);
        }

        private async void listFolderContents(MyTreeNode folderNode, bool isForDownload, bool isRecursive)
        {
            if (folderNode.nodeState == DownloadState.Downloaded ||
                folderNode.nodeState == DownloadState.Downloading)
            {
                if (isRecursive)
                {
                    foreach (MyTreeNode node in folderNode.Nodes)
                    {
                        if (node.nodeType == NodeType.Folder)
                            listFolderContents(node, isForDownload, isRecursive);
                        else if (node.nodeType == NodeType.Item)
                            listItemVersions(node, isForDownload);
                    }
                }
            }
            else
            {
                var foldersApi = new FoldersApi();
                foldersApi.Configuration.AccessToken = logInInfo.accessToken;

                string[] idParams = folderNode.id.Split('/');
                string folderId = idParams[idParams.Length - 1];
                string projectId = idParams[idParams.Length - 3];

                setNodeState(folderNode, true);
               
                dynamic contents = null;
                while (contents == null)
                {
                    try
                    {
                        contents = await foldersApi.GetFolderContentsAsync(projectId, folderId);
                    }
                    catch (Exception ex)
                    {
                        Debug.Print("startDownload >> catch1 : " + ex.Message);
                        await Task.Delay(kTimeOutDelay);
                    }
                }

                foreach (KeyValuePair<string, dynamic> contentInfo in new DynamicDictionaryItems(contents.data))
                {
                    NodeType nodeType = contentInfo.Value.attributes.extension.type.EndsWith(":Folder") ? NodeType.Folder : NodeType.Item;
                    MyTreeNode contentNode = new MyTreeNode(
                        contentInfo.Value.links.self.href,
                        contentInfo.Value.attributes.displayName,
                        contentInfo.Value.attributes.extension.type,
                        "",
                        nodeType
                    );
                    addToTreeView(folderNode, contentNode);
                    if (isRecursive)
                    {
                        if (contentNode.nodeType == NodeType.Folder)
                            listFolderContents(contentNode, isForDownload, isRecursive);
                        else if (contentNode.nodeType == NodeType.Item)
                            listItemVersions(contentNode, isForDownload);
                    }

                }
                setNodeState(folderNode, false);
            }
        }

        private async void listItemVersions(MyTreeNode itemNode, bool isForDownload)
        {
            if (itemNode.nodeState == DownloadState.Downloaded ||
                itemNode.nodeState == DownloadState.Downloading)
            {
                if (isForDownload)
                    startDownload((MyTreeNode)itemNode.FirstNode, null);

                return;
            }

            try
            {
                var itemsApi = new ItemsApi();
                itemsApi.Configuration.AccessToken = logInInfo.accessToken;

                string[] idParams = itemNode.id.Split('/');
                string itemId = idParams[idParams.Length - 1];
                string projectId = idParams[idParams.Length - 3];

                setNodeState(itemNode, true);

                dynamic versions = null;
                while (versions == null)
                {
                    try
                    {
                        versions = await itemsApi.GetItemVersionsAsync(projectId, itemId);
                    }
                    catch (Exception ex)
                    {
                        Debug.Print("startDownload >> catch1 : " + ex.Message);
                        await Task.Delay(kTimeOutDelay);
                    }
                }

                foreach (KeyValuePair<string, dynamic> versionInfo in new DynamicDictionaryItems(versions.data))
                {
                    string nodeType = versionInfo.Value.attributes.extension.type.EndsWith(":Folder") ? "folder" : "item";
                    string displayName = versionInfo.Value.attributes.displayName;
                    var fileType = "";
                    try
                    {
                        fileType = versionInfo.Value.attributes.fileType;
                        string str = "";
                    }
                    catch { }
                    Debug.Print(displayName + " fileType = " + fileType);
                    MyTreeNode versionNode = new MyTreeNode(
                        versionInfo.Value.links.self.href,
                        displayName + " (v" + versionInfo.Value.attributes.versionNumber + ")",
                        versionInfo.Value.attributes.extension.type,
                        fileType,
                        NodeType.Version
                    );
                    versionNode.nodeState = DownloadState.Downloaded;
                    addToTreeView(itemNode, versionNode);

                    // If it's for download then start downloading the latest version
                    // Note: latest version is first in the list
                    if (isForDownload)
                    {
                        startDownload(versionNode, null);
                        isForDownload = false;
                    }
                }
                setNodeState(itemNode, false);
            }
            catch (Exception ex)
            {
                itemNode.nodeState = DownloadState.Failed;
                // maybe we should do this: setNodeState
            }
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            // Check if the children has their children retieved already
            // (we always need to go one level lower then shown in the UI)
            foreach (MyTreeNode node in e.Node.Nodes)
            {
                if (node.nodeState == DownloadState.Default)
                {
                    if (node.nodeType == NodeType.Project)
                        listTopFolders(node);
                    else if (node.nodeType == NodeType.Folder)
                        listFolderContents(node, false, false);
                    else if (node.nodeType == NodeType.Item)
                        listItemVersions(node, false);
                }
            }
        }

        public delegate void DelegateSetItemState(MyListItem item, DownloadState state);
        private void setItemState(MyListItem item, DownloadState state)
        {
            if (treeView.InvokeRequired)
                treeView.Invoke(new DelegateSetItemState(this.setItemState), new Object[] { item, state });
            else
            {
                item.fileState = state;
                ListViewItem.ListViewSubItem subItem = item.SubItems[1];
                subItem.Text = state.ToString();
                subItem.BackColor = downloadStateToColor[state];
            }
        }

        private async Task downloadFile(string localPath, string href)
        {
            Debug.Print("downloadFile >> localPath : " + localPath);
            Debug.Print("downloadFile >> href : " + href);

            while (true)
            {
                try
                {
                    // Some parts will need direct HTTP messaging
                    var httpClient = new HttpClient(
                          // this should avoid HttpClient seaching for proxy settings
                          new HttpClientHandler()
                          {
                              UseProxy = false,
                              Proxy = null
                          }, true);

                    // Make sure local folder exists
                    string folderPath = Path.GetDirectoryName(localPath);
                    if (!Directory.Exists(folderPath))
                        Directory.CreateDirectory(folderPath);

                    // Now download the file
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, href);
                    request.Headers.Add("Authorization", "Bearer " + logInInfo.accessToken);
                    using (var response = await httpClient.SendAsync(request,
                         // this ResponseHeadersRead force the SendAsync to return
                         // as soon as the header is ready, faster
                         HttpCompletionOption.ResponseHeadersRead))
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var fs = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await stream.CopyToAsync(fs);
                        Debug.Print("downloadFile : After downloadFile finished");
                    }

                    return;
                }
                catch (Exception ex)
                {
                    Debug.Print("downloadFile >> catch : " + ex.Message);
                    Debug.Print("downloadFile : retry");
                    await Task.Delay(kTimeOutDelay);
                }
            }
        }

        public async Task<IRestResponse> getUrl(string href)
        {
            Uri hrefUri = new Uri(href);

            RestClient client = new RestClient(hrefUri.Scheme + "://" + hrefUri.Host);
            RestRequest request = new RestRequest(hrefUri.PathAndQuery, RestSharp.Method.GET);
            var accessToken = logInInfo.accessToken;
            Debug.Print("getUrl >> accessToken : " + accessToken);
            request.AddHeader("Authorization", "Bearer " + accessToken);

            IRestResponse response = await client.ExecuteTaskAsync(request);

            Debug.Print("getUrl >> request.Resource : " + request.Resource);

            return response;
        }

        public async Task<string> getF3zUrl(string href)
        {
            Debug.Print("getF3zUrl >> href1 : " + href);
            
            // Keep asking for an update until it's available
            while (true)
            {
                try
                {
                    Debug.Print("getF3zUrl >> href2 : " + href);
                    var response = await getUrl(href);
                    Debug.Print("getF3zUrl >> href3 : " + href);

                    Debug.Print("getF3zUrl >> response.StatusCode : " + response.StatusCode.ToString());// + " / " + response.StatusDescription);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Debug.Print("getF3zUrl >> response.Content : " + response.Content);
                        dynamic json = SimpleJson.DeserializeObject(response.Content);
                        
                        if (json.data[0].type == "jobs")
                        {
                            // It's still a job so we have to wait
                            if (json.data[0].attributes.status == "failed")
                                throw new Exception("f3z generation failed");

                            await Task.Delay(kTimeOutDelay);
                        }
                        else if (json.data[0].type == "downloads")
                        {
                            // The download is available
                            return json.data[0].relationships.storage.meta.link.href;
                        }
                        else
                        {
                            // Problem
                            throw new Exception("Download failed");
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        // Seems we got redirected - see https://stackoverflow.com/questions/28564961/authorization-header-is-lost-on-redirect
                        href = response.ResponseUri.AbsoluteUri;
                        Debug.Print("getF3zUrl : got redirected");
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.Print("getF3zUrl >> catch : " + ex.Message);
                    return null;
                }
            }
        }

        private async Task<string> getF3z(string projectId, string versionId)
        {
            Debug.Print("getF3z >> projectId : " + projectId);
            Debug.Print("getF3z >> versionId : " + versionId);

            var path = "/data/v1/projects/" + projectId + "/downloads";
            var body = "{" +
                "\"jsonapi\": {" +
                    "\"version\": \"1.0\"" +
                "}," +
                "\"data\": {" +
                    "\"type\": \"downloads\"," +
                    "\"attributes\": {" +
                        "\"format\": {" +
                            "\"fileType\": \"f3z\"" +
                        "}" +
                    "}," +
                    "\"relationships\": {" +
                        "\"source\": {" +
                            "\"data\": {" +
                                "\"type\": \"versions\"," +
                                "\"id\": \"" + versionId + "\"" +
                            "}" +
                        "}" +
                    "}" +
                "}" +
            "}";

            RestClient client = new RestClient("https://developer.api.autodesk.com");
            RestRequest request = new RestRequest(path, RestSharp.Method.POST);

            // Now download the file
            var accessToken = logInInfo.accessToken;
            Debug.Print("getF3z >> accessToken : " + accessToken);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            request.AddParameter("application/vnd.api+json", body, ParameterType.RequestBody);

            try
            {
                IRestResponse response = await client.ExecuteTaskAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Accepted)
                {
                    Debug.Print("getF3z >> response.Content : " + response.Content);
                    dynamic json = SimpleJson.DeserializeObject(response.Content);

                    var url = await getF3zUrl(json.data[0].links.self.href);

                    return url;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("getF3z >> catch : " + ex.Message); 
            }
           
            return null;
        }

        private async void startDownload(MyTreeNode node, MyListItem item)
        {
            if (item == null)
            {
                item = addFileToList(node);
            }

            try
            {
                setItemState(item, DownloadState.Downloading);

                var versionsApi = new VersionsApi();
                versionsApi.Configuration.AccessToken = logInInfo.accessToken;

                string[] idParams = node.id.Split('/');
                string versionId = HttpUtility.UrlDecode(idParams[idParams.Length - 1]);
                string projectId = idParams[idParams.Length - 3];

                dynamic version = null;
                while (version == null)
                {
                    try
                    {
                        version = await versionsApi.GetVersionAsync(projectId, versionId);
                    }
                    catch (Exception ex)
                    {
                        Debug.Print("startDownload >> catch1 : " + ex.Message);
                        await Task.Delay(kTimeOutDelay);
                    }
                }
                
                string href = null;

                // Is it a Fusion Design or Drawing?
                if (node.isFusionFile())
                {
                    // Request f3z
                    setItemState(item, DownloadState.Waiting);
                    href = await getF3z(projectId, versionId);
                    setItemState(item, DownloadState.Downloading);
                }
                else
                {
                    try
                    {
                        href = version.data.relationships.storage.meta.link.href;
                    }
                    catch (Exception ex)
                    {
                        Debug.Print("startDownload >> catch2 : " + ex.Message);
                    }
                }

                if (href == null)
                    throw new Exception("Download failed");

                Debug.Print("startDownload : Before calling downloadFile"); 
                await downloadFile(item.localPath, href);
                Debug.Print("startDownload : After calling downloadFile");

                setItemState(item, DownloadState.Downloaded);
            }
            catch (Exception ex)
            {
                Debug.Print("startDownload >> catch : " + ex.Message);
                setItemState(item, DownloadState.Failed);
            }
        }

        static string invalidChars = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        public string removeIllegalFilenameCharacters(string fileName)
        {
            foreach (char c in invalidChars)
            {
                // Let's use '_' just to show that a character got replaced
                fileName = fileName.Replace(c.ToString(), "_");
            }

            return fileName;
        }

        public MyListItem addFileToList(MyTreeNode versionNode)
        {
            // In case of Fusion files we'll be downloading f3z
            var postFix = versionNode.isFusionFile() ? ".f3z" : "." + versionNode.fileType;

            // Get relative folder path that we need to recreate on the 
            // local storage side
            var relPath = "";
            MyTreeNode node = versionNode;
            do
            {
                node = (MyTreeNode)node.Parent;
                relPath = "\\" + removeIllegalFilenameCharacters(node.Text) + relPath;
            } while (nodeToDownload != node);
            relPath = relPath.Replace(new string(kUpdateChar, 1), "");

            // Add file extrension if needed
            if (postFix != "." && !relPath.EndsWith(postFix) && !relPath.EndsWith(postFix.ToUpper()))
            {
                relPath += postFix;
            }

            // Create list item
            var item = new MyListItem(versionNode, tbxBackupFolder.Text + relPath);
            ListViewItem lvItem = ltvFiles.Items.Add(item);
            lvItem.SubItems.Add(item.fileState.ToString());

            return item;
        }

        private void btnBackupFolder_Click(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                tbxBackupFolder.Text = dialog.SelectedPath;
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            nodeToDownload = (MyTreeNode)treeView.SelectedNode;

            if (nodeToDownload == null || nodeToDownload.nodeType != NodeType.Folder)
            {
                MessageBox.Show("Select an A360 folder to back up");
                return;
            }

            if (tbxBackupFolder.Text == "")
            {
                MessageBox.Show("Select a local folder for the backup");
                return;
            }

            // btnBackup.Enabled = false;

            listFolderContents(nodeToDownload, true, true);
        }

        private void btnRetry_Click(object sender, EventArgs e)
        {
            foreach (MyListItem item in ltvFiles.Items)
            {
                if (item.fileState == DownloadState.Failed)
                {
                    startDownload(item.node, item);
                }
            }
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            foreach (MyListItem item in ltvFiles.Items)
            {
                if (item.fileState == DownloadState.Failed ||
                    item.fileState == DownloadState.Downloaded)
                    item.Remove();
            }
        }
    }
    public class LogInInfo
    {
        public string accessToken;
        public string refreshToken;
        public long expiresIn;
        public string clientId;
        public string clientSecret; 
        public Scope[] scopes = new Scope[] {
            Scope.DataRead, Scope.DataWrite, Scope.DataCreate, Scope.DataSearch
        };
    }
}
