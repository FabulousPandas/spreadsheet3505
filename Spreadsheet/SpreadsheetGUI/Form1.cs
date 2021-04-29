using SpreadsheetUtilities;
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
        // Controller object
        private SpreadsheetController controller;

        public SpreadsheetForm(SpreadsheetController control)
        {
            InitializeComponent();
            this.controller = control;
            controller.Error += DisconnectError;
            controller.ErrorND += RegularError;
            controller.Connected += OnConnect;
            controller.SpreadsheetReceived += ReceivedSpreadsheets;
            controller.UpdateReceived += ReceivedUpdate;
            controller.SelectionMade += SelectedCell;

            spreadsheetPanel.SelectionChanged += selectionChanged;
            spreadsheetPanel.SetSelection(0, 0);


        }
        private void selectionChanged(SpreadsheetPanel ssp)
        {
            //UpdateCells(); TODO: make it so when you select a different box, changes are reflected on the spreadsheet
            spreadsheetPanel.GetSelection(out int col, out int row);
            string cellName = controller.GetCellName(col, row);
            controller.SelectCell(cellName);
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cellInputText_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Return:
                    controller.SendEditRequest(cellNameTextBox.Text, cellInputTextBox.Text);
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
                selectionChanged(spreadsheetPanel);
                return true;
            }
            if (keyData == (Keys.Down))
            {
                //UpdateCells();
                spreadsheetPanel.SetSelection(col, row + 1);
                selectionChanged(spreadsheetPanel);
                return true;
            }
            if (keyData == (Keys.Left))
            {
                //UpdateCells();
                spreadsheetPanel.SetSelection(col - 1, row);
                selectionChanged(spreadsheetPanel);
                return true;
            }
            if (keyData == (Keys.Right))
            {
                //UpdateCells();
                spreadsheetPanel.SetSelection(col + 1, row);
                selectionChanged(spreadsheetPanel);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
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
                    if (controller.CellValidator(cellName))
                    {
                        spreadsheetPanel.SetSelection(controller.GetColumn(cellName), controller.GetRow(cellName));
                        selectionChanged(spreadsheetPanel);
                        e.Handled = true;
                    }
                    else
                        MessageBox.Show("Invalid cell name");
                    break;
            }
            
        }

        private void undoButton_Click(object sender, EventArgs e)
        {
            controller.SendUndoRequest();
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

        private void DisconnectError(string errorMessage)
        {
            MessageBox.Show(errorMessage);
            //re enable connect to server if connection failed
            MethodInvoker invoker =
            new MethodInvoker(
                () => 
                { 
                    //if you get disconnected, everything should be cleared and buttons should be disabled besides the connect option
                    this.connectToServerToolStripMenuItem.Enabled = true;
                    this.disconnectToolStripMenuItem.Enabled = false;
                    this.spreadsheetPanel.Clear();
                    this.spreadsheetPanel.Enabled = false;
                    this.cellNameTextBox.Enabled = false;
                    this.cellInputTextBox.Enabled = false;
                    this.setCellButton.Enabled = false;
                    this.undoButton.Enabled = false;
                }
            );
            this.Invoke(invoker);
        }

        private void RegularError(string errorMessage)
        {
            MessageBox.Show(errorMessage);
        }

        private void OnConnect()
        {
            MessageBox.Show("Successfully connected to server");
            //disable connecting to another server when already connected to one
            MethodInvoker invoker =
            new MethodInvoker(
                () => 
                {
                    this.connectToServerToolStripMenuItem.Enabled = false;
                    this.disconnectToolStripMenuItem.Enabled = true;
                }
            );
            this.Invoke(invoker);
            
        }

        private void ReceivedSpreadsheets(List<string> sheetList)
        {
            //opens dialog box for selecting a spreadsheet
            SpreadsheetSelector input = new SpreadsheetSelector(sheetList);
            string chosenSpreadsheet = "";
            MethodInvoker invoker =
            new MethodInvoker(
                () => 
                {
                    if (input.ShowDialog(this) == DialogResult.OK)
                    {
                        chosenSpreadsheet = input.Spreadsheet;
                        spreadsheetPanel.Enabled = true;
                        cellNameTextBox.Enabled = true;
                        undoButton.Enabled = true;
                    }
                }
            );
            this.Invoke(invoker);
            controller.SendSpreadsheet(chosenSpreadsheet);
        }

        private void ReceivedUpdate(int col, int row, IList<string> updateList)
        {
            //update spreadsheet panel
            RecalculateCells(updateList);

            MethodInvoker invoker =
            new MethodInvoker(
                () =>
                {
                    // Changes the cell address text box
                    string cellName = controller.GetCellName(col, row);
                    cellNameTextBox.Text = cellName;

                    // Changes the cell value text box
                    string cellValue = controller.GetCellValue(col, row);
                    cellValueTextBox.Text = cellValue;

                    // Changes the cell input box to whatever is selected
                    cellInputTextBox.Text = controller.GetCellContents(col, row);
                }
            );
            
            
        }

        private void SelectedCell(string cellName, string cellContents)
        {
            MethodInvoker invoker =
            new MethodInvoker(
                () =>
                {
                    //re-enable input boxes if we are able to edit the cell
                    cellInputTextBox.Enabled = true;
                    setCellButton.Enabled = true;

                    //update selected cell text box
                    cellNameTextBox.Text = cellName;
                    cellInputTextBox.Text = cellContents;
                }
            );
            this.Invoke(invoker);
        }

        /// <summary>
        /// Given a list of cell names, recalculate and set the text of the cells that are in the list
        /// </summary>
        /// <param name="list"></param>
        private void RecalculateCells(IList<string> list)
        {
            foreach (string s in list)
            {
                spreadsheetPanel.SetValue(controller.GetColumn(s), controller.GetRow(s), controller.GetCellValue(controller.GetColumn(s), controller.GetRow(s)));
            }
        }

        private void setCellButton_Click(object sender, EventArgs e)
        {
            controller.SendEditRequest(cellNameTextBox.Text, cellInputTextBox.Text);
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.Disconnect();
        }
    }
}
