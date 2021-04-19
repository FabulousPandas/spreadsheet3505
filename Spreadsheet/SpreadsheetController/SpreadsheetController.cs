using System;
using System.Text.RegularExpressions;
using NetworkUtil;

namespace SS
{
    public class SpreadsheetController
    {
        public delegate void ErrorHandler(string err);
        public event ErrorHandler Error;

        public delegate void ConnectedHandler();
        public event ConnectedHandler Connected;

        private Spreadsheet spreadsheet;
        private SocketState server;
        private string username;

        public void Connect(string addr)
        {
            Networking.ConnectToServer(OnConnect, addr, 1100);
        }

        private void OnConnect(SocketState state)
        {
            if (state.ErrorOccured)
            {
                Error(state.ErrorMessage);
                return;
            }

            server = state;

            Connected();
            state.OnNetworkAction = ReceiveData;
            Networking.GetData(state);
        }

        private void ReceiveData(SocketState state)
        {
            if (state.ErrorOccured)
            {
                // inform the view
                Error(state.ErrorMessage);
                return;
            }

            ProcessData(state);

            Networking.GetData(state);
        }

        private void ProcessData(SocketState state)
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

              
                state.RemoveData(0, p.Length);
            }
            //UpdateReceived();
            //ProcessInputs();
        }
    }
}
