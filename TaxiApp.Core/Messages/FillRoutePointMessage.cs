using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{

    /// <summary>
    /// Заполнить пункт маршрута
    /// </summary>
    public class FillRoutePointMessage
    {
        public TaxiApp.Core.DataModel.Order.OrderPoint Point { get; set; }
    }
}
