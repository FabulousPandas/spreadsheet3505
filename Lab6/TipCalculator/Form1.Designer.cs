namespace TipCalculator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.enterBillLabel = new System.Windows.Forms.Label();
            this.inputBillText = new System.Windows.Forms.TextBox();
            this.outputTipText = new System.Windows.Forms.TextBox();
            this.computeTipButton = new System.Windows.Forms.Button();
            this.enterPercentageLabel = new System.Windows.Forms.Label();
            this.inputPercentText = new System.Windows.Forms.TextBox();
            this.totalBillLabel = new System.Windows.Forms.Label();
            this.outputBillText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // enterBillLabel
            // 
            this.enterBillLabel.AutoSize = true;
            this.enterBillLabel.Location = new System.Drawing.Point(100, 94);
            this.enterBillLabel.Name = "enterBillLabel";
            this.enterBillLabel.Size = new System.Drawing.Size(84, 15);
            this.enterBillLabel.TabIndex = 0;
            this.enterBillLabel.Text = "Enter Total Bill:";
            // 
            // inputBillText
            // 
            this.inputBillText.Location = new System.Drawing.Point(234, 91);
            this.inputBillText.Name = "inputBillText";
            this.inputBillText.Size = new System.Drawing.Size(100, 23);
            this.inputBillText.TabIndex = 1;
            this.inputBillText.TextChanged += new System.EventHandler(this.inputBillText_TextChanged);
            // 
            // outputTipText
            // 
            this.outputTipText.Location = new System.Drawing.Point(234, 181);
            this.outputTipText.Name = "outputTipText";
            this.outputTipText.Size = new System.Drawing.Size(100, 23);
            this.outputTipText.TabIndex = 2;
            this.outputTipText.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // computeTipButton
            // 
            this.computeTipButton.Location = new System.Drawing.Point(100, 180);
            this.computeTipButton.Name = "computeTipButton";
            this.computeTipButton.Size = new System.Drawing.Size(94, 23);
            this.computeTipButton.TabIndex = 3;
            this.computeTipButton.Text = "Compute Tip: ";
            this.computeTipButton.UseVisualStyleBackColor = true;
            this.computeTipButton.Click += new System.EventHandler(this.computeTipButton_Click);
            // 
            // enterPercentageLabel
            // 
            this.enterPercentageLabel.AutoSize = true;
            this.enterPercentageLabel.Location = new System.Drawing.Point(100, 138);
            this.enterPercentageLabel.Name = "enterPercentageLabel";
            this.enterPercentageLabel.Size = new System.Drawing.Size(118, 15);
            this.enterPercentageLabel.TabIndex = 4;
            this.enterPercentageLabel.Text = "Enter Tip Percentage:";
            // 
            // inputPercentText
            // 
            this.inputPercentText.Location = new System.Drawing.Point(234, 135);
            this.inputPercentText.Name = "inputPercentText";
            this.inputPercentText.Size = new System.Drawing.Size(100, 23);
            this.inputPercentText.TabIndex = 5;
            this.inputPercentText.TextChanged += new System.EventHandler(this.inputPercentText_TextChanged);
            // 
            // totalBillLabel
            // 
            this.totalBillLabel.AutoSize = true;
            this.totalBillLabel.Location = new System.Drawing.Point(100, 226);
            this.totalBillLabel.Name = "totalBillLabel";
            this.totalBillLabel.Size = new System.Drawing.Size(57, 15);
            this.totalBillLabel.TabIndex = 6;
            this.totalBillLabel.Text = "Total Bill: ";
            // 
            // outputBillText
            // 
            this.outputBillText.Location = new System.Drawing.Point(234, 223);
            this.outputBillText.Name = "outputBillText";
            this.outputBillText.Size = new System.Drawing.Size(100, 23);
            this.outputBillText.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 340);
            this.Controls.Add(this.outputBillText);
            this.Controls.Add(this.totalBillLabel);
            this.Controls.Add(this.inputPercentText);
            this.Controls.Add(this.enterPercentageLabel);
            this.Controls.Add(this.computeTipButton);
            this.Controls.Add(this.outputTipText);
            this.Controls.Add(this.inputBillText);
            this.Controls.Add(this.enterBillLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label enterBillLabel;
        private System.Windows.Forms.TextBox inputBillText;
        private System.Windows.Forms.TextBox outputTipText;
        private System.Windows.Forms.Button computeTipButton;
        private System.Windows.Forms.Label enterPercentageLabel;
        private System.Windows.Forms.TextBox inputPercentText;
        private System.Windows.Forms.Label totalBillLabel;
        private System.Windows.Forms.TextBox outputBillText;
    }
}

