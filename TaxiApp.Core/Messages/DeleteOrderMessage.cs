using System;

namespace TaxiApp.Core.Messages
{
    /// <summary>
    /// ”далить заказ
    /// </summary>
    public class DeleteOrderMessage
    {
		  public int OrderId { get; set; }
    }
}
