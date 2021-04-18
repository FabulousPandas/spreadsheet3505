namespace SpreadsheetGUI
{
    partial class ConnectInputDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.okButton = new System.Windows.Forms.Button();
            this.usernameTextBox = new System.Windows.Forms.Label();
            this.usernameInputBox = new System.Windows.Forms.TextBox();
            this.ipAddressTextBox = new System.Windows.Forms.Label();
            this.ipAddressInputBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(348, 248);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 24;
            this.okButton.Text = "&OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.AutoSize = true;
            this.usernameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.usernameTextBox.Location = new System.Drawing.Point(35, 21);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(68, 15);
            this.usernameTextBox.TabIndex = 25;
            this.usernameTextBox.Text = "Username:";
            this.usernameTextBox.Click += new System.EventHandler(this.label1_Click);
            // 
            // usernameInputBox
            // 
            this.usernameInputBox.Location = new System.Drawing.Point(111, 21);
            this.usernameInputBox.Name = "usernameInputBox";
            this.usernameInputBox.Size = new System.Drawing.Size(100, 20);
            this.usernameInputBox.TabIndex = 26;
            // 
            // ipAddressTextBox
            // 
            this.ipAddressTextBox.AutoSize = true;
            this.ipAddressTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.ipAddressTextBox.Location = new System.Drawing.Point(35, 64);
            this.ipAddressTextBox.Name = "ipAddressTextBox";
            this.ipAddressTextBox.Size = new System.Drawing.Size(68, 15);
            this.ipAddressTextBox.TabIndex = 27;
            this.ipAddressTextBox.Text = "IP Address:";
            // 
            // ipAddressInputBox
            // 
            this.ipAddressInputBox.Location = new System.Drawing.Point(111, 63);
            this.ipAddressInputBox.Name = "ipAddressInputBox";
            this.ipAddressInputBox.Size = new System.Drawing.Size(100, 20);
            this.ipAddressInputBox.TabIndex = 28;
            // 
            // ConnectInputDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 283);
            this.Controls.Add(this.ipAddressInputBox);
            this.Controls.Add(this.ipAddressTextBox);
            this.Controls.Add(this.usernameInputBox);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectInputDialog";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to Server";
            this.Load += new System.EventHandler(this.ConnectInputDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label usernameTextBox;
        private System.Windows.Forms.TextBox usernameInputBox;
        private System.Windows.Forms.Label ipAddressTextBox;
        private System.Windows.Forms.TextBox ipAddressInputBox;
    }
}
