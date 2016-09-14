using System;

namespace TaxiApp.Core.Messages
{

    /// <summary>
    /// Выбрать локацию из списка
    /// </summary>
    public class SelectLocationMessage
    {
        public int Priority { get; set; }

        public TaxiApp.Core.DataModel.LocationItem LocationItem { get; set; }
    }
}
