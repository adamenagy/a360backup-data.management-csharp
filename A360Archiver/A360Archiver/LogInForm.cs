using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using Autodesk.Forge;
using System.Diagnostics;
using A360Archiver.Properties;
using System.Configuration;

namespace A360Archiver
{
    public partial class LogInForm : Form
    {
        private LogInInfo logInInfo;
        private CefSharp.WinForms.ChromiumWebBrowser webBrowser = null;

        public LogInForm(ref LogInInfo info)
        {
            InitializeComponent();

            logInInfo = info;

            // Add webBrowser control here instead of in the 
            // UI editor so that the UI editor will work :-/
            this.webBrowser = new CefSharp.WinForms.ChromiumWebBrowser((string)"");
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(3, 124);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(557, 227);
            this.webBrowser.TabIndex = 0;
            this.webBrowser.LoadError += WebBrowser_LoadError;
            this.webPanel.Controls.Add(this.webBrowser);

            // Get settings
            Settings props = Settings.Default;
            tbxClientId.Text = props.clientId;
            tbxClientSecret.Text = props.clientSecret;
            tbxCallbackUrl.Text = props.callbackUrl;
        }

        public delegate void DelegateCloseDialog();
        private void closeDialog()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new DelegateCloseDialog(this.closeDialog));
            }
            else
            {
                // Save settings
                Settings props = Settings.Default;
                props.clientId = tbxClientId.Text;
                props.clientSecret = tbxClientSecret.Text;
                props.callbackUrl = tbxCallbackUrl.Text;
                props.Save();

                DialogResult = DialogResult.OK;
                this.Close();
            }

        }

        private void WebBrowser_LoadError(object sender, CefSharp.LoadErrorEventArgs e)
        {
            Uri uri = new Uri(e.FailedUrl);

            Debug.Print("WebBrowser_LoadError >> e.FailedUrl : " + e.FailedUrl);

            // get th host name from the full URL, e.g.:
            // https://myhost.com >> myhost.com
            string host = tbxCallbackUrl.Text.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)[1].ToLower();
            if (uri.Host.ToLower() == host)
            {
                // Get the code 
                string code = HttpUtility.ParseQueryString(uri.Query).Get("code");

                // Navigate to something else to remove the
                // bad page
                webBrowser.Load(null);

                // Turn the code into access token 
                var authApi = new ThreeLeggedApi();
                var response = authApi.Gettoken(tbxClientId.Text, tbxClientSecret.Text, "authorization_code", code, tbxCallbackUrl.Text);
                logInInfo.accessToken = response["access_token"];
                logInInfo.refreshToken = response["refresh_token"];
                logInInfo.expiresIn = response["expires_in"];
                logInInfo.clientId = tbxClientId.Text;
                logInInfo.clientSecret = tbxClientSecret.Text;

                //this.DialogResult = DialogResult.OK;
                //this.Close();
                closeDialog();
            }
        }

        private void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            Debug.Print("WebBrowser_Navigated >> e.Url : " + e.Url.ToString());

            // get th host name from the full URL, e.g.:
            // https://myhost.com >> myhost.com
            string host = tbxCallbackUrl.Text.Split(new string[] { "//" }, StringSplitOptions.RemoveEmptyEntries)[1].ToLower();
            if (e.Url.Host.ToLower() == host)
            {
                // Get the code 
                string code = HttpUtility.ParseQueryString(e.Url.Query).Get("code");

                // Navigate to something else to remove the
                // bad page
                //webBrowser.Url = null;

                // Turn the code into access token 
                var authApi = new ThreeLeggedApi();
                var response = authApi.Gettoken(tbxClientId.Text, tbxClientSecret.Text, "authorization_code", code, tbxCallbackUrl.Text);
                logInInfo.accessToken = response["access_token"];
                logInInfo.refreshToken = response["refresh_token"];
                logInInfo.expiresIn = response["expires_in"];
                logInInfo.clientId = tbxClientId.Text;
                logInInfo.clientSecret = tbxClientSecret.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            var authApi = new ThreeLeggedApi();
            var urlPath = authApi.Authorize(tbxClientId.Text, "code", tbxCallbackUrl.Text, logInInfo.scopes);

            //webBrowser.Url = new Uri(urlPath);
            webBrowser.Load(urlPath);
        }
    }
}
