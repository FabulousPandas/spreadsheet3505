﻿using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class SpreadsheetForm : Form
    {
        /// <summary>
        /// Holds the backing spreadsheet object that handles the logic
        /// </summary>
        private Spreadsheet sheet;

        private SpreadsheetController controller;

        public SpreadsheetForm(SpreadsheetController control)
        {
            InitializeComponent();
            this.controller = control;
            controller.Error += ShowError;
            controller.Connected += OnConnect;

            //sheet = new Spreadsheet(CellValidator, s => s.ToUpper(), "default"); // creates a spreadsheet that normalizes variables to capital letters
            //spreadsheetPanel.SelectionChanged += selectionChanged;
            //spreadsheetPanel.SetSelection(0, 0);
            //selectionChanged(spreadsheetPanel);
        }

        
        /*
         * Keeping for reference
         * 
        /// <summary>
        /// Helper method for creating a save file dialog for saving a spreadsheet file
        /// </summary>
        private void OpenSaveDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Spreadsheet File|*.sprd|All files (*.*)|*.*";
            dialog.Title = "Save a Spreadsheet File";
            try
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (dialog.FilterIndex == 1)
                        dialog.DefaultExt = "sprd";
                    sheet.Save(dialog.FileName);
                }
            }
            catch (SpreadsheetReadWriteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }*/

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cellInputText_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Return:
                    //UpdateCells();
                    e.Handled = true;
                    break;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            spreadsheetPanel.GetSelection(out int col, out int row);

            if (keyData == (Keys.Up))
            {
                //UpdateCells();
                spreadsheetPanel.SetSelection(col, row - 1);
                //selectionChanged(spreadsheetPanel);
                return true;
            }
            if (keyData == (Keys.Down))
            {
                //UpdateCells();
                spreadsheetPanel.SetSelection(col, row + 1);
                //selectionChanged(spreadsheetPanel);
                return true;
            }
            if (keyData == (Keys.Left))
            {
                //UpdateCells();
                spreadsheetPanel.SetSelection(col - 1, row);
                //selectionChanged(spreadsheetPanel);
                return true;
            }
            if (keyData == (Keys.Right))
            {
                //UpdateCells();
                spreadsheetPanel.SetSelection(col + 1, row);
                //selectionChanged(spreadsheetPanel);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SpreadsheetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //TODO: maybe add some socket closing bullshit? 
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpBox help = new HelpBox();
            help.Show();
        }

        private void cellNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Return:
                    string cellName = cellNameTextBox.Text.ToUpper();
                    /*
                    if (CellValidator(cellName))
                    {
                        spreadsheetPanel.SetSelection(GetColumn(cellName), GetRow(cellName));
                        selectionChanged(spreadsheetPanel);
                        e.Handled = true;
                    }
                    else
                        MessageBox.Show("Invalid cell name");
                    */
                    break;
            }
            
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            //TODO: send a request to the server to undo
        }

        private void connectToServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string userName = "", ipAddress = "";
            int port;
 
            ConnectInputDialog input = new ConnectInputDialog();
            if(input.ShowDialog(this) == DialogResult.OK)
            {
                userName = input.UserName;
                ipAddress = input.IPAddress;
                port = input.Port;
            }
            else
            {
                MessageBox.Show("No values provided, cancelling connection to server");
                return;
            }

            controller.Connect(ipAddress, userName, port);
            
        }

        private void ShowError(string errorMessage)
        {
            MessageBox.Show(errorMessage);
            //re enable connect to server if connection failed
            MethodInvoker invoker =
            new MethodInvoker(
                () => { this.connectToServerToolStripMenuItem.Enabled = true; }
            );
            this.Invoke(invoker);
        }

        private void OnConnect()
        {
            MessageBox.Show("Successfully connected to server");
            //disable connecting to another server when already connected to one
            MethodInvoker invoker =
            new MethodInvoker(
                () => {this.connectToServerToolStripMenuItem.Enabled = false; }
            );
            this.Invoke(invoker);
            
        }

        private void ReceivedSpreadsheets()
        {

        }

    }
}
