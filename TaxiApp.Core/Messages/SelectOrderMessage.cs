using System;

namespace TaxiApp.Core.Messages
{
    public class SelectOrderMessage
    {
		  TaxiApp.Core.Entities.Order Order { get; set; }
    }
}
