﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.DataModel;
using Windows.Devices.Geolocation;

namespace TaxiApp.Core.Managers
{
    public class MapPainter
    {
        private IMap _mapImpl = null;
        private TaxiApp.Core.Managers.LocationManager _locationMG = null;

        public MapPainter(IMap map, TaxiApp.Core.Managers.LocationManager locationMG)
        {
            _mapImpl = map;
            _locationMG = locationMG;
        }


        public async void ShowMyPossitionAsync()
        {
            Geopoint myGeopoint = await _locationMG.GetCurrentGeopoint();

            _mapImpl.ShowMyPossition(myGeopoint);
        }

        public void ShowMarker(Geopoint point, int type)
        {
            _mapImpl.ShowMarker(point, type);
        }

        public void ShowRoute(IRoute route)
        {
            _mapImpl.ShowRoute(route);
        }

        public void Clear()
        {
            _mapImpl.Clear();
        }
    }
}
