using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;

namespace TaxiApp.Core.Managers
{
    public interface IMap
    {
        void ShowMyPossition(Geopoint myGeopoint);

        void ShowMarker(Geopoint geopoint, int type);

        void ShowRoute(IRoute route);

        void Clear();
    }
}
