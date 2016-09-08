using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;

namespace TaxiApp.Core.Managers
{
    public class MapPainter
    {
        private IMap _mapImpl = null;

        public MapPainter(IMap map)
        {
            _mapImpl = map;
        }


        public async Task ShowMyPossitionAsync()
        {
            TaxiApp.Core.Managers.LocationManager locationMG = null;
            //TaxiApp.Core.Managers.LocationManager locationMG = TaxiApp.Core.Managers.ManagerFactory.Instance.GetLocationManager();

            Geopoint myGeopoint = await locationMG.GetCurrentGeopoint();

            _mapImpl.ShowMyPossition(myGeopoint);


        }

        public void ShowRoute(IRoute route)
        {
            _mapImpl.ShowRoute(route);
        }
    }
}
