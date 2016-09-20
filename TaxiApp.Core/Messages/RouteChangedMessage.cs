using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.DataModel;
using Windows.Devices.Geolocation;

namespace TaxiApp.Core.Messages
{
    /// <summary>
    /// Маршрут изменен
    /// </summary>
    public class RouteChangedMessage
    {
        public IEnumerable<Geopoint> points { get; set; }
        public IRoute route { get; set; }
    }
}
