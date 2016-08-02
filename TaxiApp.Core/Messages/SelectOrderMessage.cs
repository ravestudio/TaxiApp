using System;

namespace TaxiApp.Core.Messages
{
    public class SelectOrderMessage
    {
		  public TaxiApp.Core.Entities.Order Order { get; set; }
    }
}
