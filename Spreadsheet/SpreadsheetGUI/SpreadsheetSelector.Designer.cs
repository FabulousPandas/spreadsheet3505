namespace SpreadsheetGUI
{
    partial class SpreadsheetSelector
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
            this.spreadsheetLabel = new System.Windows.Forms.Label();
            this.spreadsheetInputBox = new System.Windows.Forms.TextBox();
            this.spreadsheetList = new System.Windows.Forms.ListBox();
            this.selectSpreadsheetLabel = new System.Windows.Forms.Label();
            this.otherLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // submitButton
            // 
            this.submitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.submitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.submitButton.Location = new System.Drawing.Point(346, 248);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 23);
            this.submitButton.TabIndex = 24;
            this.submitButton.Text = "Submit";
            this.submitButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // spreadsheetLabel
            // 
            this.spreadsheetLabel.AutoSize = true;
            this.spreadsheetLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.spreadsheetLabel.Location = new System.Drawing.Point(12, 176);
            this.spreadsheetLabel.Name = "spreadsheetLabel";
            this.spreadsheetLabel.Size = new System.Drawing.Size(198, 15);
            this.spreadsheetLabel.TabIndex = 27;
            this.spreadsheetLabel.Text = "Enter the name of the spreadsheet:";
            // 
            // spreadsheetInputBox
            // 
            this.spreadsheetInputBox.Location = new System.Drawing.Point(321, 176);
            this.spreadsheetInputBox.Name = "spreadsheetInputBox";
            this.spreadsheetInputBox.Size = new System.Drawing.Size(100, 20);
            this.spreadsheetInputBox.TabIndex = 28;
            // 
            // spreadsheetList
            // 
            this.spreadsheetList.FormattingEnabled = true;
            this.spreadsheetList.Location = new System.Drawing.Point(15, 25);
            this.spreadsheetList.Name = "spreadsheetList";
            this.spreadsheetList.Size = new System.Drawing.Size(408, 108);
            this.spreadsheetList.TabIndex = 30;
            this.spreadsheetList.SelectedValueChanged += new System.EventHandler(this.spreadsheetList_SelectedValueChanged);
            // 
            // selectSpreadsheetLabel
            // 
            this.selectSpreadsheetLabel.AutoSize = true;
            this.selectSpreadsheetLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectSpreadsheetLabel.Location = new System.Drawing.Point(12, 9);
            this.selectSpreadsheetLabel.Name = "selectSpreadsheetLabel";
            this.selectSpreadsheetLabel.Size = new System.Drawing.Size(188, 15);
            this.selectSpreadsheetLabel.TabIndex = 31;
            this.selectSpreadsheetLabel.Text = "Select a spreadsheet from the list";
            // 
            // otherLabel
            // 
            this.otherLabel.AutoSize = true;
            this.otherLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.otherLabel.Location = new System.Drawing.Point(12, 153);
            this.otherLabel.Name = "otherLabel";
            this.otherLabel.Size = new System.Drawing.Size(341, 15);
            this.otherLabel.TabIndex = 32;
            this.otherLabel.Text = "or create a new spreadsheet by entering a name not in the list";
            // 
            // SpreadsheetSelector
            // 
            this.AcceptButton = this.submitButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 283);
            this.Controls.Add(this.otherLabel);
            this.Controls.Add(this.selectSpreadsheetLabel);
            this.Controls.Add(this.spreadsheetList);
            this.Controls.Add(this.spreadsheetInputBox);
            this.Controls.Add(this.spreadsheetLabel);
            this.Controls.Add(this.submitButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpreadsheetSelector";
            this.Padding = new System.Windows.Forms.Padding(9);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select a Spreadsheet";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Label spreadsheetLabel;
        private System.Windows.Forms.TextBox spreadsheetInputBox;
        private System.Windows.Forms.ListBox spreadsheetList;
        private System.Windows.Forms.Label selectSpreadsheetLabel;
        private System.Windows.Forms.Label otherLabel;
    }
}
