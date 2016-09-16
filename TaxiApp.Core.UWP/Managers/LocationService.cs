using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;

namespace TaxiApp.Core.UWP.Managers
{
    public class LocationService : TaxiApp.Core.Managers.ILocationService
    {
        public LocationService()
        {
            MapService.ServiceToken = "I0cGrslTbQ3DHrRQvFQx~QJpA302VeNqZlqFjzOu2EA~Al43aNX3jPD5U_v91nVr3mXF6RGb6O8SPJgkJJSqns-1DDS8DxfhmQyoxg3aYqeu";
        }
        public Task<ILocation> GetLocation(Geopoint hintPoint)
        {
            TaskCompletionSource<ILocation> TCS = new TaskCompletionSource<ILocation>();

            Task<MapLocationFinderResult> findLocationTask = MapLocationFinder.FindLocationsAtAsync(hintPoint).AsTask();

            findLocationTask.ContinueWith(t => {
                if (t.Result.Status == MapLocationFinderStatus.Success)
                {
                    TCS.SetResult( new Location(t.Result.Locations[0]));
                }
            });

            return TCS.Task;
        }

        public Task<IList<ILocation>> GetLocationList(Geopoint hintPoint, string searchQuery)
        {
            TaskCompletionSource<IList<ILocation>> TCS = new TaskCompletionSource<IList<ILocation>>();

            Task<MapLocationFinderResult> task = MapLocationFinder.FindLocationsAsync(searchQuery, hintPoint, 3).AsTask();

            task.ContinueWith(t =>
            {
                IList<ILocation> list = null;

                if (t.Result.Locations.Count > 0)
                {
                    list = new List<ILocation>();

                    foreach (MapLocation mapLocation in t.Result.Locations)
                    {
                        list.Add(new Location(mapLocation));
                    }
                }

                TCS.SetResult(list);
            });

            return TCS.Task;
        }

        public Task<IRoute> GetRoute(IEnumerable<Geopoint> points)
        {
            TaskCompletionSource<IRoute> TCS = new TaskCompletionSource<IRoute>();

            

            Task<MapRouteFinderResult> routeTask =
                MapRouteFinder.GetDrivingRouteFromWaypointsAsync(
                    points,
                    MapRouteOptimization.Time,
                    MapRouteRestrictions.None).AsTask();

            routeTask.ContinueWith(t =>
            {
                TCS.SetResult(new Route(t.Result.Route));
            });

            return TCS.Task;
        }
    }
}
