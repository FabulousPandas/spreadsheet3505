using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TipCalculator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void computeTipButton_Click(object sender, EventArgs e)
        {
            calculateBill();
        }

        private void inputBillText_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(inputBillText.Text, out _))
                computeTipButton.Enabled = true;
            else
                computeTipButton.Enabled = false;

            calculateBill();
        }

        private void inputPercentText_TextChanged(object sender, EventArgs e)
        {
            if (double.TryParse(inputPercentText.Text, out _))
                computeTipButton.Enabled = true;
            else
                computeTipButton.Enabled = false;

            calculateBill();
        }

        private void calculateBill()
        {
            double.TryParse(inputBillText.Text, out double inputBill);
            double.TryParse(inputPercentText.Text, out double percentage);
            double tip = inputBill * (percentage / 100.0);
            outputTipText.Text = tip.ToString();
            outputBillText.Text = (inputBill + tip).ToString();
        }
    }
}
