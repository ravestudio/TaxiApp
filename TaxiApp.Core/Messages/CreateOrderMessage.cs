using System;

namespace TaxiApp.Core.Messages
{
    public class CreateOrderMessage
    {
        TaxiApp.Core.Entities.Order Order { get; set; }
    }
}
