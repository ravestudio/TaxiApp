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
        private TaxiApp.Core.Managers.LocationManager locationMg = null;
        private SearchCommand search_cmd = null;
        private string _searchText = null;

        //private Object thisLock = new Object();

        public Order.OrderPoint SelectedPoint { get; set; }

        //public Windows.UI.Core.CoreDispatcher Dispatcher { get; set; }

        public delegate void LocationReadyDelegate(bool ready);
        public LocationReadyDelegate LocationReadyChanged;

        public bool LocationReady
        {
            get
            {
                return this.locationMg.LocationReady;
            }
        }

        public SearchModel()
        {
            int thread = Environment.CurrentManagedThreadId;

            this.search_cmd = new SearchCommand(this);

            this.locationMg = Managers.ManagerFactory.Instance.GetLocationManager();


            Task initTask = this.locationMg.InitCurrentLocation().ContinueWith((task) =>
                {
                    if (task.Exception == null)
                    {
                        this.NotifyLocationReadyChanged();
                    }

                });

            this._searchText = string.Empty;

            this.Locations = new System.Collections.ObjectModel.ObservableCollection<LocationItem>();
        }

        public string SearchText {
            get { return this._searchText; }
            set
            {
                this._searchText = value;

                if (this.SearchChanged.CanExecute(null))
                {
                    this.SearchChanged.Execute(null);
                }
                else
                {
                    this.Locations.Clear();
                }
            }
        }

        public SearchCommand SearchChanged { get { return this.search_cmd; } }

        //public Windows.Services.Maps.MapLocationFinderResult SearchResults
        //{
        //    get;
        //    set;
        //}

        public System.Collections.ObjectModel.ObservableCollection<LocationItem> Locations { get; set; }

        public async void Search()
        {
            Geopoint hintPoint = await this.locationMg.GetCurrentGeopoint();

            ILocation currentLocation = this.locationMg.GetCurrentLocation();

            string town = currentLocation.Town;

            string searchQuery = string.Format("{0} {1}", town, this.SearchText);

            IList<ILocation> SearchResults = await this.locationMg.GetLocations(hintPoint, searchQuery);

            //lock (this.thisLock)
            //{
            //    this.FillLocations(SearchResults);

            //}

            this.FillLocations(SearchResults);
        }

        private void FillLocations(IList<ILocation> SearchResults)
        {
            this.Locations.Clear();

            if (SearchResults != null)
            {
                foreach (ILocation location in SearchResults)
                {
                    this.Locations.Add(new LocationItem(location));
                }
            }
        }

        public void NotifyLocationReadyChanged()
        {
            if (LocationReadyChanged != null)
            {
                LocationReadyChanged(this.locationMg.LocationReady);

            }
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

    public class SearchCommand : System.Windows.Input.ICommand
    {
        private SearchModel _model = null;

        public SearchCommand(SearchModel model)
        {
            this._model = model;
        }

        public bool CanExecute(object parameter)
        {
            bool res = false;
            
            if (this._model.SearchText.Length > 5)
            {
                res = true;
            }

            return res;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this._model.Search();
        }
    }


}
