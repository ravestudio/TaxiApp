using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.DataModel.Order;

namespace TaxiApp.Core.Messages
{

    /// <summary>
    /// Заполнить пункт маршрута
    /// </summary>
    public class FillRoutePointMessage
    {
        public OrderPoint Point { get; set; }
    }
}
