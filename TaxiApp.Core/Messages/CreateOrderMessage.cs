using System;

namespace TaxiApp.Core.Messages
{
    public class CreateOrderMessage
    {
        public TaxiApp.Core.Entities.Order Order { get; set; }
    }
}
