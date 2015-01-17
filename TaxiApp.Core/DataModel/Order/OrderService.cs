using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.DataModel.Order
{

    public class ServiceItem
    {
        public byte id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
}
