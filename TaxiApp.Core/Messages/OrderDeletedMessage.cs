using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{
    /// <summary>
    /// Заказ удален
    /// </summary>
    public class OrderDeletedMessage
    {
        public int OrderId { get; set; }
    }
}
