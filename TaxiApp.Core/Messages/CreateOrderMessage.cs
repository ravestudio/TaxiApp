using System;

namespace TaxiApp.Core.Messages
{
    /// <summary>
    /// Создать заказ
    /// </summary>
    public class CreateOrderMessage
    {
        public TaxiApp.Core.Entities.Order Order { get; set; }
    }
}
