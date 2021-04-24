using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{
    public class ServerMessage
    {
        public string cellName { get; set; }
        public string contents { get; set; }
        public string message { get; set; }
        public string selector { get; set; }
        public string selectorName { get; set; }
    }

    /*
    ServerMessage cellUpdate = new ServerMessage
    {
        cellName = "A1",
        contents = "Hello"
    };

    string jsonIncludeNullValues = JsonConvert.SerializeObject(cellUpdate, Formatting.Indented);

    Console.WriteLine(jsonIncludeNullValues);
    // {
    //   "cellName": "A1",
    //   "contents": "Hello,
    //   "message": null,
    //   "selector": null
    //   "selectorName": null
    // }
    
    string jsonIgnoreNullValues = JsonConvert.SerializeObject(cellUpdate, Formatting.Indented, new JsonSerializerSettings
    {
       NullValueHandling = NullValueHandling.Ignore
    });

    Console.WriteLine(jsonIgnoreNullValues);
    // {
    //   "cellName": "A1",
    //   "contents": "Hello,
    // }
    */
}
