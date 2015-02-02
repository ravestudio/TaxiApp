using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Socket
{
    public class SocketResponse
    {
        public string request { get; set; }
        public int idorder { get; set; }
        public int orderstatus { get; set; }
    }
}
