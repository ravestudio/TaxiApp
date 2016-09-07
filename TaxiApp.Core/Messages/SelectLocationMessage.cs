using System;

namespace TaxiApp.Core.Messages
{

    /// <summary>
    /// Выбрать локацию из списка
    /// </summary>
    public class SelectLocationMessage
    {
		  public TaxiApp.Core.DataModel.LocationItem LocationItem { get; set; }
    }
}
