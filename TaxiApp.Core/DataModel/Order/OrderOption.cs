using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.DataModel.Order
{

    public class OrderOption
    {
        public byte id { get; set; }
        public string Name { get; set; }
        public Windows.UI.Xaml.Media.ImageSource IconSource { get; set; }

    }
}
