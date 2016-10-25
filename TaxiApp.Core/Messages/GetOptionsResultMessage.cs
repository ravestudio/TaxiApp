using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.DataModel.Order;

namespace TaxiApp.Core.Messages
{
    public class GetOptionsResultMessage
    {
        public IList<OrderOption> ServiceList { get; set; }
    }
}
