using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;

namespace TaxiApp.Core.Managers
{
    public class LocationManager
    {

        private Geolocator locator = new Geolocator();

        private ILocation _currentLocation = null;

        private ILocationService _locationService = null;

        //private Object thisLock = new Object();



        public bool LocationReady
        {
            get;
            set;
        }

        public LocationManager(ILocationService locationService)
        {
            this._locationService = locationService;
            this.LocationReady = false;
            //this.Init();
        }

        public Task InitCurrentLocation()
        {
            

            this.LocationReady = false;

            Task<Geopoint> task = this.GetCurrentGeopoint();

            return task.ContinueWith(t =>
            {
                Geopoint currentGeopoint = t.Result;
                var locationTask = this._locationService.GetLocation(currentGeopoint);

                this._currentLocation = locationTask.Result;

                this.LocationReady = true;
            });
        }



        public Task<Geopoint> GetCurrentGeopoint()
        {
            TaskCompletionSource<Geopoint> TCS = new TaskCompletionSource<Geopoint>();

            //locator.DesiredAccuracy = PositionAccuracy.High;
            
            Task<Geoposition> task = locator.GetGeopositionAsync().AsTask();

            task.ContinueWith(t =>
            {
                var pos = t.Result;

                Geopoint myGeopoint = new Geopoint(new BasicGeoposition()
                {
                    Latitude = pos.Coordinate.Point.Position.Latitude,
                    Longitude = pos.Coordinate.Point.Position.Longitude
                });

                TCS.SetResult(myGeopoint);
            });

            return TCS.Task;
        }


        public ILocation GetCurrentLocation()
        {
            return this._currentLocation;
        }

        public Task<IList<ILocation>> GetLocations(Geopoint hintPoint, string searchQuery)
        {
            return _locationService.GetLocationList(hintPoint, searchQuery);
        }

        public Task<IRoute> GetRoute(IEnumerable<Geopoint> points)
        {
            return _locationService.GetRoute(points);
        }

    }
}
