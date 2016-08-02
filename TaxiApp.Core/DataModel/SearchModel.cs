using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Devices.Geolocation;

namespace TaxiApp.Core.DataModel
{
    public class SearchModel
    {
        private TaxiApp.Core.Managers.LocationManager _locationMg = null;
        private RelayCommand<string> searchCmd = null;
        private string _searchText = null;

        //private Object thisLock = new Object();

        public Order.OrderPoint SelectedPoint { get; set; }

        //public Windows.UI.Core.CoreDispatcher Dispatcher { get; set; }

        public bool LocationReady
        {
            get
            {
                return this.locationMg.LocationReady;
            }
        }

        public SearchModel(LocationManager locationManager)
        {
            int thread = Environment.CurrentManagedThreadId;

            this._locationMg = locationManager;


            Task initTask = this.locationMg.InitCurrentLocation().ContinueWith((task) =>
                {
                    if (task.Exception == null)
                    {
                        Messenger.Default.Send<LocationChangedMessage>(new LocationChangedMessage() { 
                            Ready = _locationMg.LocationReady,
                            Location = _locationMg.CurrentLocation
                        });
                    }

                });
            
            Messenger.Default.Register<SearchLocationMessage>(this, (msg) => {
                    this.Search(text).ContinueWith((t) => {
                        IList<LocationItem> locationItems = t.Result;
                        Messenger.Default.Send<FindedLocationsMessage>(new FindedLocationsMessage() { 
                            locationItems = locationItems
                        });
                    });
                });
        }


        public Task<IList<LocationItem>> Search(string text)
        {
            TaskCompletitionSource<IList<LocationItem>> TCS = new TaskCompletitionSource<IList<LocationItem>>();
            Task<Geopoint> task = this.locationMg.GetCurrentGeopoint();
            task.ContinueWith((t) => {
                IList<LocationItem> locations = new List<LocationItem>();
                Geopoint hintPoint = t.Result;
                
                ILocation currentLocation = this.locationMg.GetCurrentLocation();

                string town = currentLocation.Town;

                string searchQuery = string.Format("{0} {1}", town, this.SearchText);

                IList<ILocation> SearchResults = this.locationMg.GetLocations(hintPoint, searchQuery).Result;
                
                if (SearchResults != null)
                {
                    foreach (ILocation location in SearchResults)
                    {
                        locations.Add(new LocationItem(location));
                    }
                }
                TCS.SetResult(locations);
            }
            return TCS.Task;
        }
    }

    public class LocationItem
    {
        public LocationItem()
        {
            this.Ready = false;
        }

        public LocationItem(ILocation location)
        {
            this.Address = string.Format("{0}, {1}", location.Street, location.StreetNumber);
            this.FullAddress = string.Format("{0}, {1}, {2}", location.Town, location.Region, location.PostCode);

            this.Latitude = location.Latitude;
            this.Longitude = location.Longitude;

            this.Location = location;

            this.Ready = true;
        }

        public string Address { get; set; }
        public string FullAddress { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ILocation Location { get; set; }

        public bool Ready { get; set; }

    }

}
