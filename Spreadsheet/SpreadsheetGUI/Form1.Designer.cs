namespace SpreadsheetGUI
{
    partial class SpreadsheetForm
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
            this.verticleSplitContainer = new System.Windows.Forms.SplitContainer();
            this.undoButton = new System.Windows.Forms.Button();
            this.cellAddressLabel = new System.Windows.Forms.Label();
            this.cellValueTextBox = new System.Windows.Forms.TextBox();
            this.cellValueLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.setCellLabel = new System.Windows.Forms.Label();
            this.cellInputTextBox = new System.Windows.Forms.TextBox();
            this.setCellButton = new System.Windows.Forms.Button();
            this.cellNameTextBox = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.spreadsheetPanel = new SS.SpreadsheetPanel();
            ((System.ComponentModel.ISupportInitialize)(this.verticleSplitContainer)).BeginInit();
            this.verticleSplitContainer.Panel1.SuspendLayout();
            this.verticleSplitContainer.Panel2.SuspendLayout();
            this.verticleSplitContainer.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // verticleSplitContainer
            // 
            this.verticleSplitContainer.BackColor = System.Drawing.SystemColors.Control;
            this.verticleSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.verticleSplitContainer.IsSplitterFixed = true;
            this.verticleSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.verticleSplitContainer.Name = "verticleSplitContainer";
            this.verticleSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // verticleSplitContainer.Panel1
            // 
            this.verticleSplitContainer.Panel1.Controls.Add(this.undoButton);
            this.verticleSplitContainer.Panel1.Controls.Add(this.cellAddressLabel);
            this.verticleSplitContainer.Panel1.Controls.Add(this.cellValueTextBox);
            this.verticleSplitContainer.Panel1.Controls.Add(this.cellValueLabel);
            this.verticleSplitContainer.Panel1.Controls.Add(this.panel1);
            this.verticleSplitContainer.Panel1.Controls.Add(this.cellNameTextBox);
            this.verticleSplitContainer.Panel1.Controls.Add(this.menuStrip1);
            // 
            // verticleSplitContainer.Panel2
            // 
            this.verticleSplitContainer.Panel2.Controls.Add(this.spreadsheetPanel);
            this.verticleSplitContainer.Size = new System.Drawing.Size(800, 450);
            this.verticleSplitContainer.SplitterDistance = 83;
            this.verticleSplitContainer.TabIndex = 0;
            // 
            // undoButton
            // 
            this.undoButton.Enabled = false;
            this.undoButton.Location = new System.Drawing.Point(713, 32);
            this.undoButton.Name = "undoButton";
            this.undoButton.Size = new System.Drawing.Size(75, 20);
            this.undoButton.TabIndex = 6;
            this.undoButton.Text = "Undo";
            this.undoButton.UseVisualStyleBackColor = true;
            this.undoButton.Click += new System.EventHandler(this.undoButton_Click);
            // 
            // cellAddressLabel
            // 
            this.cellAddressLabel.AutoSize = true;
            this.cellAddressLabel.Location = new System.Drawing.Point(30, 35);
            this.cellAddressLabel.Name = "cellAddressLabel";
            this.cellAddressLabel.Size = new System.Drawing.Size(72, 13);
            this.cellAddressLabel.TabIndex = 5;
            this.cellAddressLabel.Text = "Selected Cell:";
            // 
            // cellValueTextBox
            // 
            this.cellValueTextBox.Enabled = false;
            this.cellValueTextBox.Location = new System.Drawing.Point(303, 32);
            this.cellValueTextBox.Name = "cellValueTextBox";
            this.cellValueTextBox.Size = new System.Drawing.Size(390, 20);
            this.cellValueTextBox.TabIndex = 3;
            // 
            // cellValueLabel
            // 
            this.cellValueLabel.AutoSize = true;
            this.cellValueLabel.Location = new System.Drawing.Point(188, 35);
            this.cellValueLabel.Name = "cellValueLabel";
            this.cellValueLabel.Size = new System.Drawing.Size(109, 13);
            this.cellValueLabel.TabIndex = 3;
            this.cellValueLabel.Text = "Selected Cell\'s Value:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.setCellLabel);
            this.panel1.Controls.Add(this.cellInputTextBox);
            this.panel1.Controls.Add(this.setCellButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 21);
            this.panel1.TabIndex = 2;
            // 
            // setCellLabel
            // 
            this.setCellLabel.AutoSize = true;
            this.setCellLabel.Location = new System.Drawing.Point(30, 4);
            this.setCellLabel.Name = "setCellLabel";
            this.setCellLabel.Size = new System.Drawing.Size(49, 13);
            this.setCellLabel.TabIndex = 1;
            this.setCellLabel.Text = "Set Cell: ";
            // 
            // cellInputTextBox
            // 
            this.cellInputTextBox.Enabled = false;
            this.cellInputTextBox.Location = new System.Drawing.Point(85, 1);
            this.cellInputTextBox.Name = "cellInputTextBox";
            this.cellInputTextBox.Size = new System.Drawing.Size(608, 20);
            this.cellInputTextBox.TabIndex = 0;
            this.cellInputTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cellInputText_KeyPress);
            // 
            // setCellButton
            // 
            this.setCellButton.Enabled = false;
            this.setCellButton.Location = new System.Drawing.Point(713, 0);
            this.setCellButton.Name = "setCellButton";
            this.setCellButton.Size = new System.Drawing.Size(75, 21);
            this.setCellButton.TabIndex = 0;
            this.setCellButton.Text = "Set Cell";
            this.setCellButton.UseVisualStyleBackColor = true;
            this.setCellButton.Click += new System.EventHandler(this.setCellButton_Click);
            // 
            // cellNameTextBox
            // 
            this.cellNameTextBox.Enabled = false;
            this.cellNameTextBox.Location = new System.Drawing.Point(108, 32);
            this.cellNameTextBox.Name = "cellNameTextBox";
            this.cellNameTextBox.Size = new System.Drawing.Size(68, 20);
            this.cellNameTextBox.TabIndex = 4;
            this.cellNameTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cellNameTextBox_KeyPress);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToServerToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // connectToServerToolStripMenuItem
            // 
            this.connectToServerToolStripMenuItem.Name = "connectToServerToolStripMenuItem";
            this.connectToServerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.connectToServerToolStripMenuItem.Text = "Connect to Server";
            this.connectToServerToolStripMenuItem.Click += new System.EventHandler(this.connectToServerToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // spreadsheetPanel
            // 
            this.spreadsheetPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetPanel.Enabled = false;
            this.spreadsheetPanel.Location = new System.Drawing.Point(0, 0);
            this.spreadsheetPanel.Name = "spreadsheetPanel";
            this.spreadsheetPanel.Size = new System.Drawing.Size(800, 363);
            this.spreadsheetPanel.TabIndex = 0;
            // 
            // SpreadsheetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.verticleSplitContainer);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "SpreadsheetForm";
            this.Text = "Spreadsheet";
            this.verticleSplitContainer.Panel1.ResumeLayout(false);
            this.verticleSplitContainer.Panel1.PerformLayout();
            this.verticleSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.verticleSplitContainer)).EndInit();
            this.verticleSplitContainer.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer verticleSplitContainer;
        private SS.SpreadsheetPanel spreadsheetPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.TextBox cellInputTextBox;
        private System.Windows.Forms.Button setCellButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox cellNameTextBox;
        private System.Windows.Forms.TextBox cellValueTextBox;
        private System.Windows.Forms.Label cellValueLabel;
        private System.Windows.Forms.Label cellAddressLabel;
        private System.Windows.Forms.Label setCellLabel;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Button undoButton;
        private System.Windows.Forms.ToolStripMenuItem connectToServerToolStripMenuItem;
    }
}

