using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{
    /// <summary>
    /// Список заказов загружен
    /// </summary>
    public class OrderListloadedMessage
    {
        public IList<TaxiApp.Core.Entities.Order> orderList { get; set; }
    }
}
