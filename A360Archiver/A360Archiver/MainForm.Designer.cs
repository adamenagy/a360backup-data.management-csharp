namespace A360Archiver
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.treeView = new System.Windows.Forms.TreeView();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.btnBackup = new System.Windows.Forms.Button();
            this.btnBackupFolder = new System.Windows.Forms.Button();
            this.tbxBackupFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ltvFiles = new System.Windows.Forms.ListView();
            this.chFilePath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRetry = new System.Windows.Forms.Button();
            this.btnClearList = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.treeView);
            this.splitContainer.Panel1.Padding = new System.Windows.Forms.Padding(3);
            this.splitContainer.Panel1MinSize = 100;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanel);
            this.splitContainer.Panel2MinSize = 300;
            this.splitContainer.Size = new System.Drawing.Size(757, 442);
            this.splitContainer.SplitterDistance = 237;
            this.splitContainer.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Location = new System.Drawing.Point(3, 3);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(231, 436);
            this.treeView.TabIndex = 0;
            this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 6;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel.Controls.Add(this.btnClearList, 3, 2);
            this.tableLayoutPanel.Controls.Add(this.btnRetry, 2, 2);
            this.tableLayoutPanel.Controls.Add(this.btnBackup, 1, 2);
            this.tableLayoutPanel.Controls.Add(this.btnBackupFolder, 5, 1);
            this.tableLayoutPanel.Controls.Add(this.tbxBackupFolder, 1, 1);
            this.tableLayoutPanel.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.ltvFiles, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 3;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(516, 442);
            this.tableLayoutPanel.TabIndex = 0;
            // 
            // btnBackup
            // 
            this.btnBackup.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBackup.Location = new System.Drawing.Point(103, 414);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(94, 23);
            this.btnBackup.TabIndex = 2;
            this.btnBackup.Text = "Start backup";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // btnBackupFolder
            // 
            this.btnBackupFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBackupFolder.Location = new System.Drawing.Point(487, 383);
            this.btnBackupFolder.Name = "btnBackupFolder";
            this.btnBackupFolder.Size = new System.Drawing.Size(26, 20);
            this.btnBackupFolder.TabIndex = 3;
            this.btnBackupFolder.Text = "...";
            this.btnBackupFolder.UseVisualStyleBackColor = true;
            this.btnBackupFolder.Click += new System.EventHandler(this.btnBackupFolder_Click);
            // 
            // tbxBackupFolder
            // 
            this.tableLayoutPanel.SetColumnSpan(this.tbxBackupFolder, 4);
            this.tbxBackupFolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.tbxBackupFolder.Location = new System.Drawing.Point(103, 383);
            this.tbxBackupFolder.Name = "tbxBackupFolder";
            this.tbxBackupFolder.ReadOnly = true;
            this.tbxBackupFolder.Size = new System.Drawing.Size(378, 20);
            this.tbxBackupFolder.TabIndex = 1;
            this.tbxBackupFolder.Text = "C:\\temp\\A360";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(3, 380);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 6, 6, 0);
            this.label1.Size = new System.Drawing.Size(94, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Backup folder";
            // 
            // ltvFiles
            // 
            this.ltvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFilePath,
            this.chStatus});
            this.tableLayoutPanel.SetColumnSpan(this.ltvFiles, 6);
            this.ltvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ltvFiles.FullRowSelect = true;
            this.ltvFiles.GridLines = true;
            this.ltvFiles.Location = new System.Drawing.Point(3, 3);
            this.ltvFiles.Name = "ltvFiles";
            this.ltvFiles.Size = new System.Drawing.Size(510, 374);
            this.ltvFiles.TabIndex = 4;
            this.ltvFiles.UseCompatibleStateImageBehavior = false;
            this.ltvFiles.View = System.Windows.Forms.View.Details;
            // 
            // chFilePath
            // 
            this.chFilePath.Text = "File path";
            this.chFilePath.Width = 306;
            // 
            // chStatus
            // 
            this.chStatus.Text = "Status";
            // 
            // btnRetry
            // 
            this.btnRetry.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRetry.Location = new System.Drawing.Point(203, 414);
            this.btnRetry.Name = "btnRetry";
            this.btnRetry.Size = new System.Drawing.Size(94, 23);
            this.btnRetry.TabIndex = 5;
            this.btnRetry.Text = "Retry";
            this.btnRetry.UseVisualStyleBackColor = true;
            this.btnRetry.Click += new System.EventHandler(this.btnRetry_Click);
            // 
            // btnClearList
            // 
            this.btnClearList.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnClearList.Location = new System.Drawing.Point(303, 414);
            this.btnClearList.Name = "btnClearList";
            this.btnClearList.Size = new System.Drawing.Size(94, 23);
            this.btnClearList.TabIndex = 6;
            this.btnClearList.Text = "Clear list";
            this.btnClearList.UseVisualStyleBackColor = true;
            this.btnClearList.Click += new System.EventHandler(this.btnClearList_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 442);
            this.Controls.Add(this.splitContainer);
            this.Name = "MainForm";
            this.Text = "A360 Backup";
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.TextBox tbxBackupFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBackupFolder;
        private System.Windows.Forms.ListView ltvFiles;
        private System.Windows.Forms.ColumnHeader chFilePath;
        private System.Windows.Forms.ColumnHeader chStatus;
        private System.Windows.Forms.Button btnClearList;
        private System.Windows.Forms.Button btnRetry;
    }
}

