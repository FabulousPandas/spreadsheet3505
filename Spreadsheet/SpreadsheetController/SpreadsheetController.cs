using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NetworkUtil;
using SpreadsheetUtilities;

namespace SS
{
    public class SpreadsheetController
    {
        public delegate void ErrorHandler(string err);
        public event ErrorHandler Error;

        public delegate void ConnectedHandler();
        public event ConnectedHandler Connected;

        public delegate void SpreadsheetsReceivedHandler(List<string> list);
        public event SpreadsheetsReceivedHandler SpreadsheetReceived;

        private List<string> spreadsheetList;

        private Spreadsheet sheet;
        private SocketState server;
        private string username;

        public void Connect(string addr, string name, int port)
        {
            Networking.ConnectToServer(OnConnect, addr, port);
            username = name;
        }

        private void OnConnect(SocketState state)
        {
            if (state.ErrorOccured)
            {
                Error(state.ErrorMessage);
                return;
            }

            server = state;
            spreadsheetList = new List<string>();
            Networking.Send(server.TheSocket, username + "\n");

            Connected();
            state.OnNetworkAction = ReceiveSpreadsheetData;
            Networking.GetData(state);
        }

        private void ReceiveSpreadsheetData(SocketState state)
        {
            if (state.ErrorOccured)
            {
                // inform the view
                Error(state.ErrorMessage);
                return;
            }

            ProcessSpreadsheetData(state);

            Networking.GetData(state);
        }

        private void ProcessSpreadsheetData(SocketState state)
        {
            string totalData = state.GetData();

            Console.WriteLine(totalData.Substring(totalData.Length - 2));
            if (totalData.Substring(totalData.Length - 2) != "\n\n")  //checking if we have received all the spreadsheets yet
                return;

            string[] parts = Regex.Split(totalData, @"(?<=[\n])");
            
            foreach (string p in parts)
            {
                // Ignore empty strings added by the regex splitter
                if (p.Length == 0)
                    continue;
                // The regex splitter will include the last string even if it doesn't end with a '\n',
                // So we need to ignore it if this happens. 
                if (p[p.Length - 1] != '\n')
                    break;

                spreadsheetList.Add(p);

                state.RemoveData(0, p.Length);
            }

            state.OnNetworkAction = ReceiveEdits;
            SpreadsheetReceived(spreadsheetList);
        }

        private void ReceiveEdits(SocketState state)
        {
            if (state.ErrorOccured)
            {
                // inform the view
                Error(state.ErrorMessage);
                return;
            }

            Networking.GetData(state);
        }

        /*
         * Sends the given spreadsheet name to the server for the handshake
         */
        public void SendSpreadsheet(string sheetName)
        {
            Networking.Send(server.TheSocket, sheetName + "\n");
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
                //spreadsheetPanel.SetValue(GetColumn(s), GetRow(s), GetCellValue(GetColumn(s), GetRow(s)));
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

                IList<string> updateList = sheet.SetContentsOfCell(cellName, contents);
                //spreadsheetPanel.SetValue(col, row, GetCellValue(col, row));

                //if(!undoButton.Enabled) 
                //  undoButton.Enabled = true;

                return updateList;
            }
            catch (CircularException)
            {
                //MessageBox.Show("Cannot create circular dependency");
            }
            catch (FormulaFormatException e)
            {
                //MessageBox.Show("Invalid Formula: " + e.Message);
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
                return ((FormulaError)cellValue).Reason;
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

        /*
        private void selectionChanged(SpreadsheetPanel ssp)
        {
            //UpdateCells(); TODO: make it so when you select a different box, changes are reflected on the spreadsheet

            spreadsheetPanel.GetSelection(out int col, out int row);
            // Changes the cell address text box
            string cellName = GetCellName(col, row);
            cellNameTextBox.Text = cellName;

            // Changes the cell value text box
            string cellValue = GetCellValue(col, row);
            cellValueTextBox.Text = cellValue;

            // Changes the cell input box to whatever is selected
            cellInputText.Text = GetCellContents(col, row);
        }*/

        /*
         * Helper method for updating all the cells within the spreadsheet.
         */
        private void UpdateCells()
        {
            // Sets the contents of the selected cell when clicked
            //spreadsheetPanel.GetSelection(out int col, out int row);
            //IList<string> updateList = SetCell(col, row, cellInputText.Text);

            // Updates cell value text box
            //string cellValue = GetCellValue(col, row);
            //cellValueTextBox.Text = cellValue;

            // If you make a new change, button can't redo
            //undoButton.Text = "Undo";

            // Recalculates cells that depends on the selected cell
            //RecalculateCells(updateList);
        }
    }
}
