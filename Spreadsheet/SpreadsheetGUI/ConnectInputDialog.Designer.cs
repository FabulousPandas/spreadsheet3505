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
            this.submitButton = new System.Windows.Forms.Button();
            this.usernameTextBox = new System.Windows.Forms.Label();
            this.usernameInputBox = new System.Windows.Forms.TextBox();
            this.ipAddressTextBox = new System.Windows.Forms.Label();
            this.ipAddressInputBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.portTextBox = new System.Windows.Forms.Label();
            this.portInputBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // submitButton
            // 
            this.submitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.submitButton.Location = new System.Drawing.Point(259, 248);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 3;
            this.submitButton.Text = "Submit";
            this.submitButton.Click += new System.EventHandler(this.okButton_Click);
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
            // 
            // usernameInputBox
            // 
            this.usernameInputBox.Location = new System.Drawing.Point(301, 21);
            this.usernameInputBox.Name = "usernameInputBox";
            this.usernameInputBox.Size = new System.Drawing.Size(100, 20);
            this.usernameInputBox.TabIndex = 0;
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
            this.ipAddressInputBox.Location = new System.Drawing.Point(301, 64);
            this.ipAddressInputBox.Name = "ipAddressInputBox";
            this.ipAddressInputBox.Size = new System.Drawing.Size(100, 20);
            this.ipAddressInputBox.TabIndex = 1;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(348, 248);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // portTextBox
            // 
            this.portTextBox.AutoSize = true;
            this.portTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.portTextBox.Location = new System.Drawing.Point(35, 107);
            this.portTextBox.Name = "portTextBox";
            this.portTextBox.Size = new System.Drawing.Size(126, 15);
            this.portTextBox.TabIndex = 30;
            this.portTextBox.Text = "Port (default is 1100): ";
            // 
            // portInputBox
            // 
            this.portInputBox.Location = new System.Drawing.Point(301, 106);
            this.portInputBox.Name = "portInputBox";
            this.portInputBox.Size = new System.Drawing.Size(100, 20);
            this.portInputBox.TabIndex = 2;
            // 
            // ConnectInputDialog
            // 
            this.AcceptButton = this.submitButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 283);
            this.Controls.Add(this.portInputBox);
            this.Controls.Add(this.portTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.ipAddressInputBox);
            this.Controls.Add(this.ipAddressTextBox);
            this.Controls.Add(this.usernameInputBox);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.submitButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectInputDialog";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to Server";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ConnectInputDialog_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Label usernameTextBox;
        private System.Windows.Forms.TextBox usernameInputBox;
        private System.Windows.Forms.Label ipAddressTextBox;
        private System.Windows.Forms.TextBox ipAddressInputBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label portTextBox;
        private System.Windows.Forms.TextBox portInputBox;
    }
}
