using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{
    public class ServerMessage
    {
        public string messageType { get; set; }
        public string cellName { get; set; }
        public string contents { get; set; }
        public string message { get; set; }
        public int? selector { get; set; }
        public string selectorName { get; set; }
        public int? user { get; set; }
    }
}
