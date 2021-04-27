using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NetworkUtil;
using SpreadsheetUtilities;
using Newtonsoft.Json;

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

        public delegate void UpdateReceivedHandler(int col, int row, IList<string> updateList);
        public event UpdateReceivedHandler UpdateReceived;

        public delegate void SelectionMadeHandler(string cellName);
        public event SelectionMadeHandler SelectionMade;

        private List<string> spreadsheetList;

        private Spreadsheet sheet;
        private SocketState server;
        private string username;
        private int userID;

        public void Connect(string addr, string name, int port)
        {
            username = name;
            Networking.ConnectToServer(OnConnect, addr, port);
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

            if (totalData.Length - 2 > 0 && totalData.Substring(totalData.Length - 2) != "\n\n")  //checking if we have received all the spreadsheets yet
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

            // Creates our spreadsheet with our validator, normalizing cell values to capital letters, and with version "default"
            sheet = new Spreadsheet(CellValidator, s => s.ToUpper(), "default");
            state.OnNetworkAction = ReceiveUpdates;
            SpreadsheetReceived(spreadsheetList);
        }

        private void ReceiveUpdates(SocketState state)
        {
            if (state.ErrorOccured)
            {
                // inform the view
                Error(state.ErrorMessage);
                return;
            }

            ProcessUpdates(state);
            Networking.GetData(state);
        }

        private void ProcessUpdates(SocketState state)
        {
            string totalData = state.GetData();

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

                ServerMessage message = JsonConvert.DeserializeObject<ServerMessage>(p);
                if (message == null)
                    continue;
                if(message.messageType == "cellUpdated")
                {
                    Console.WriteLine("Received cell update message");
                    int col = GetColumn(message.cellName), row = GetRow(message.cellName);
                    IList<string> updateList = SetCell(col, row, message.contents);
                    UpdateReceived(col, row, updateList);
                }
                else if(message.messageType == "cellSelected")
                {
                    if(message.selectorName == username)
                    {
                        SelectionMade(message.cellName);
                        userID = (int) message.selector;
                    }
                }
                else if (message.messageType == "disconnected")
                {
                    if (message.user == userID)
                        Error("Disconnected from server");
                }
                else if (message.messageType == "requestError")
                {
                    Error("Error in cell: " + message.cellName + "\nReason: " + message.message);
                }
                else if (message.messageType == "serverError")
                {
                    Error(message.message);
                }

                state.RemoveData(0, p.Length);
            }
        }

        /*
         * Sends the given spreadsheet name to the server for the handshake
         */
        public void SendSpreadsheet(string sheetName)
        {
            Networking.Send(server.TheSocket, sheetName + "\n");
        }

        public void SelectCell(string cellName)
        {
            Console.WriteLine("Cell selected: " + cellName);
            ClientRequest request = new ClientRequest { requestType = "selectCell", cellName = cellName };
            string requestString = JsonConvert.SerializeObject(request, Formatting.None, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore });
            Console.WriteLine(requestString);
            Networking.Send(server.TheSocket, requestString + "\n");
        }

        public void SendEditRequest(string cellName, string cellContents)
        {
            Console.WriteLine("Edit request of cell: " + cellName + " to contents: " + cellContents);
            ClientRequest request = new ClientRequest { requestType = "editCell", cellName = cellName, contents = cellContents };
            string requestString = JsonConvert.SerializeObject(request, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            Networking.Send(server.TheSocket, requestString + "\n");
        }

        public void SendUndoRequest()
        {
            Console.WriteLine("Undo request sent to server");
            ClientRequest request = new ClientRequest { requestType = "undo" };
            string requestString = JsonConvert.SerializeObject(request, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            Networking.Send(server.TheSocket, requestString + "\n");
        }

        /// <summary>
        /// Returns true if s is a valid cell in the spreadsheet, returns false otherwise
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool CellValidator(string cell)
        {
            string pattern = @"^[a-zA-Z][1-9][0-9]?$";
            return Regex.IsMatch(cell, pattern);
        }

        /// <summary>
        /// Helper method for retrieving the cell name from the given row and column
        /// </summary>
        /// <returns></returns>
        public string GetCellName(int col, int row)
        {
            char letter = (char)('A' + col);
            return "" + letter + (row + 1);
        }

        /// <summary>
        /// Returns the column of the cell in terms of the spreadsheet panel
        /// </summary>
        /// <param name="cellName"></param>
        /// <returns></returns>
        public int GetColumn(string cellName)
        {
            return cellName[0] - 'A';
        }

        /// <summary>
        /// Returns the row of the cell in terms of the spreadsheet panel
        /// </summary>
        /// <param name="cellName"></param>
        /// <returns></returns>
        public int GetRow(string cellName)
        {
            return int.Parse(cellName.Substring(1)) - 1;
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

                return updateList;
            }
            catch (FormulaFormatException)
            {
                Error("Invalid formula given");
            }
            return new List<string>();
        }

        /// <summary>
        /// Helper method for getting the string representation of a cell's value
        /// </summary>
        /// <returns></returns>
        public string GetCellValue(int col, int row)
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
        public string GetCellContents(int col, int row)
        {
            string cellName = GetCellName(col, row);
            object cellContents = sheet.GetCellContents(cellName);
            if (cellContents is Formula)
                return "=" + cellContents.ToString();
            return cellContents.ToString();
        }
        
    }
}
