using System;

namespace TaxiApp.Core.Messages
{
    /// <summary>
    /// Выбрать заказ для просмотра
    /// </summary>
    public class SelectOrderMessage
    {
		  public TaxiApp.Core.Entities.Order Order { get; set; }
    }
}
