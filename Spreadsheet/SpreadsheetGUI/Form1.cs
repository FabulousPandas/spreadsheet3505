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
        /// <summary>
        /// Holds the backing spreadsheet object that handles the logic
        /// </summary>
        private Spreadsheet sheet;

        /// <summary>
        /// For holding the name of the cell that was last changed
        /// </summary>
        private string previousCellName;
        /// <summary>
        /// For holding the contents of the cell that was last changed
        /// </summary>
        private string previousCellContents;

        public SpreadsheetForm()
        {
            InitializeComponent();

            sheet = new Spreadsheet(CellValidator, s => s.ToUpper(), "ps6"); // creates a spreadsheet that normalizes variables to capital letters
            spreadsheetPanel.SelectionChanged += selectionChanged;
            spreadsheetPanel.SetSelection(0, 0);
            selectionChanged(spreadsheetPanel);
        }

        /// <summary>
        /// Returns true if s is a valid cell in the spreadsheet, returns false otherwise
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private bool CellValidator(string cell)
        {
            string pattern = @"^[a-zA-Z][1-9][0-9]?$";
            return Regex.IsMatch(cell, pattern);
        }

        /// <summary>
        /// Helper method for retrieving the cell name from the given row and column
        /// </summary>
        /// <returns></returns>
        private string GetCellName(int col, int row)
        {
            char letter = (char)('A' + col);
            return "" + letter + (row + 1);
        }

        /// <summary>
        /// Returns the column of the cell in terms of the spreadsheet panel
        /// </summary>
        /// <param name="cellName"></param>
        /// <returns></returns>
        private int GetColumn(string cellName)
        {
            return cellName[0] - 'A';
        }

        /// <summary>
        /// Returns the row of the cell in terms of the spreadsheet panel
        /// </summary>
        /// <param name="cellName"></param>
        /// <returns></returns>
        private int GetRow(string cellName)
        {
            return int.Parse(cellName.Substring(1)) - 1;
        }

        /// <summary>
        /// Given a list of cell names, recalculate and set the text of the cells that are in the list
        /// </summary>
        /// <param name="list"></param>
        private void RecalculateCells(IList<string> list)
        {
            foreach (string s in list)
            {
                spreadsheetPanel.SetValue(GetColumn(s), GetRow(s), GetCellValue(GetColumn(s), GetRow(s)));
            }
        }

        /// <summary>
        /// Helper method for setting a cell
        /// </summary>
        private IList<string> SetCell(int col, int row, string contents)
        {
            try
            {
                string cellName = GetCellName(col, row);

                //storing previous value in case of undo
                previousCellName = cellName;
                previousCellContents = GetCellContents(col, row);

                IList<string> updateList = sheet.SetContentsOfCell(cellName, contents);
                spreadsheetPanel.SetValue(col, row, GetCellValue(col, row));
               
                if(!undoButton.Enabled) 
                    undoButton.Enabled = true;

                return updateList;
            }
            catch(CircularException)
            {
                MessageBox.Show("Cannot create circular dependency");
            }
            catch(FormulaFormatException e)
            {
                MessageBox.Show("Invalid Formula: " + e.Message);
            }
            return new List<string>();
        }

        /// <summary>
        /// Helper method for getting the string representation of a cell's value
        /// </summary>
        /// <returns></returns>
        private string GetCellValue(int col, int row)
        {
            string cellName = GetCellName(col, row);
            object cellValue = sheet.GetCellValue(cellName);
            if (cellValue is FormulaError)
                return ((FormulaError) cellValue).Reason;
            else if (cellValue is Formula)
                return "=" + cellValue.ToString();
            return cellValue.ToString();
        }

        /// <summary>
        /// Helper method for getting the string representation of a cell's contents
        /// </summary>
        /// <returns></returns>
        private string GetCellContents(int col, int row)
        {
            string cellName = GetCellName(col, row);
            object cellContents = sheet.GetCellContents(cellName);
            if (cellContents is Formula)
                return "=" + cellContents.ToString();
            return cellContents.ToString();
        }

        private void selectionChanged(SpreadsheetPanel ssp)
        {
            spreadsheetPanel.GetSelection(out int col, out int row);
            // Changes the cell address text box
            string cellName = GetCellName(col, row);
            cellNameTextBox.Text = cellName;

            // Changes the cell value text box
            string cellValue = GetCellValue(col, row);
            cellValueTextBox.Text = cellValue;

            // Changes the cell input box to whatever is selected
            cellInputText.Text = GetCellContents(col, row);
        }

        private void setCellButton_Click(object sender, EventArgs e)
        {
            // Sets the contents of the selected cell when clicked
            spreadsheetPanel.GetSelection(out int col, out int row);
            IList<string> updateList = SetCell(col, row, cellInputText.Text);

            // Updates cell value text box
            string cellValue = GetCellValue(col, row);
            cellValueTextBox.Text = cellValue;

            // If you make a new change, button can't redo
            undoButton.Text = "Undo";

            // Recalculates cells that depends on the selected cell
            RecalculateCells(updateList);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpreadsheetApplication.getAppContext().RunForm(new SpreadsheetForm());
        }

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
        }

        /// <summary>
        /// Helper method for creating a dialog for when trying to close the spreadsheet with unsaved changes
        /// </summary>
        private void OpenUnsavedChangesDialog()
        {
            if (sheet.Changed)
            {
                DialogResult result = MessageBox.Show("There are unsaved changes, would you like to save?", "Unsaved Changes", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    OpenSaveDialog();
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenSaveDialog();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.Filter = "Spreadsheet File|*.sprd|All files (*.*)|*.*";
                    dialog.Title = "Open a Spreadsheet File";

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        spreadsheetPanel.Clear();
                        string fileName = dialog.FileName;
                        sheet = new Spreadsheet(fileName, CellValidator, s => s.ToUpper(), "ps6");
                        RecalculateCells(new List<string>(sheet.GetNamesOfAllNonemptyCells()));
                        selectionChanged(spreadsheetPanel);
                    }
                }
            }
            catch (SpreadsheetReadWriteException ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
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
                    setCellButton_Click(sender, e);
                    e.Handled = true;
                    break;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            spreadsheetPanel.GetSelection(out int col, out int row);

            if (keyData == (Keys.Up))
            {
                spreadsheetPanel.SetSelection(col, row - 1);
                selectionChanged(spreadsheetPanel);
                return true;
            }
            if (keyData == (Keys.Down))
            {
                spreadsheetPanel.SetSelection(col, row + 1);
                selectionChanged(spreadsheetPanel);
                return true;
            }
            if (keyData == (Keys.Left))
            {
                spreadsheetPanel.SetSelection(col - 1, row);
                selectionChanged(spreadsheetPanel);
                return true;
            }
            if (keyData == (Keys.Right))
            {
                spreadsheetPanel.SetSelection(col + 1, row);
                selectionChanged(spreadsheetPanel);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SpreadsheetForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            OpenUnsavedChangesDialog();
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

                    if (CellValidator(cellName))
                    {
                        spreadsheetPanel.SetSelection(GetColumn(cellName), GetRow(cellName));
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
            SetCell(GetColumn(previousCellName), GetRow(previousCellName), previousCellContents);
            selectionChanged(spreadsheetPanel);

            // Swapping between the buttons
            if(undoButton.Text == "Undo") 
                undoButton.Text = "Redo";
            else
                undoButton.Text = "Undo";
        }
    }
}
