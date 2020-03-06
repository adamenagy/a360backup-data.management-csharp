namespace A360Archiver
{
    partial class LogInForm
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
            this.webPanel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel = new System.Windows.Forms.Panel();
            this.btnLogIn = new System.Windows.Forms.Button();
            this.tbxCallbackUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxClientSecret = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxClientId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel.SuspendLayout();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // webPanel
            // 
            this.webPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webPanel.Location = new System.Drawing.Point(3, 124);
            this.webPanel.MinimumSize = new System.Drawing.Size(20, 20);
            this.webPanel.Name = "webPanel";
            this.webPanel.Size = new System.Drawing.Size(557, 398);
            this.webPanel.TabIndex = 0;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.webPanel, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.panel, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 121F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(563, 525);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // panel
            // 
            this.panel.Controls.Add(this.btnLogIn);
            this.panel.Controls.Add(this.tbxCallbackUrl);
            this.panel.Controls.Add(this.label3);
            this.panel.Controls.Add(this.tbxClientSecret);
            this.panel.Controls.Add(this.label2);
            this.panel.Controls.Add(this.tbxClientId);
            this.panel.Controls.Add(this.label1);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(3, 3);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(557, 115);
            this.panel.TabIndex = 1;
            // 
            // btnLogIn
            // 
            this.btnLogIn.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogIn.Location = new System.Drawing.Point(203, 85);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(131, 23);
            this.btnLogIn.TabIndex = 6;
            this.btnLogIn.Text = "Log In";
            this.btnLogIn.UseVisualStyleBackColor = true;
            this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // tbxCallbackUrl
            // 
            this.tbxCallbackUrl.Location = new System.Drawing.Point(103, 59);
            this.tbxCallbackUrl.Name = "tbxCallbackUrl";
            this.tbxCallbackUrl.Size = new System.Drawing.Size(389, 20);
            this.tbxCallbackUrl.TabIndex = 5;
            this.tbxCallbackUrl.Text = "https://A360Backup.com";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Callback URL";
            // 
            // tbxClientSecret
            // 
            this.tbxClientSecret.Location = new System.Drawing.Point(103, 33);
            this.tbxClientSecret.Name = "tbxClientSecret";
            this.tbxClientSecret.Size = new System.Drawing.Size(389, 20);
            this.tbxClientSecret.TabIndex = 3;
            this.tbxClientSecret.Text = "3BZQqGTnoXI6EARN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Client Secret";
            // 
            // tbxClientId
            // 
            this.tbxClientId.Location = new System.Drawing.Point(103, 7);
            this.tbxClientId.Name = "tbxClientId";
            this.tbxClientId.Size = new System.Drawing.Size(389, 20);
            this.tbxClientId.TabIndex = 1;
            this.tbxClientId.Text = "310h5GqntyNNGIfzZK0CtMDwQHrWWkKG";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Client ID";
            // 
            // LogInForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 525);
            this.Controls.Add(this.tableLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "LogInForm";
            this.Text = "Log In";
            this.tableLayoutPanel.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel webPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Button btnLogIn;
        private System.Windows.Forms.TextBox tbxCallbackUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxClientSecret;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxClientId;
        private System.Windows.Forms.Label label1;
    }
}