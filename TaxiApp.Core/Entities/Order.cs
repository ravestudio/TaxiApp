using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TaxiApp.Core.Entities
{
    public class Order: Entity<int>, INotifyPropertyChanged
    {
        private Dictionary<int, string> _statusValue = new Dictionary<int, string>();

        public DateTime StartDate { get; set; }

        public decimal Ordersumm { get; set; }
        public int Routemeters { get; set; }

        public IList<OrderRouteItem> route = new List<OrderRouteItem>();

        public Order()
        {
            this.Status = 0;

            this._statusValue.Add(0, "Created");
            this._statusValue.Add(1, "Booked");
            this._statusValue.Add(2, "In Driving");
            this._statusValue.Add(3, "Arrived");
            this._statusValue.Add(4, "Performed");
            this._statusValue.Add(99, "Canceled");
            this._statusValue.Add(100, "Complete");
        }

        private int _status = 0;

        public int Status {
            get
            {
                return this._status;
            }

            set
            {
                this._status = value;
                NotifyPropertyChanged("Status");
                NotifyPropertyChanged("StatusText");
            }
        }

        public string StatusText
        {
            get
            {
                return this._statusValue[Status];
            }
        }

        public IList<OrderRouteItem> Route
        {
            get
            {
                return this.route;
            }
        }

        public string Title
        {
            get
            {
                return string.Format("Order {0}", this.StartDate);
            }
        }

        public string DistanceAndPrice
        {
            get
            {
                return string.Format("{0}km, {1}$", (float)this.Routemeters/1000, this.Ordersumm);
            }
        }

        public override void ReadData(Windows.Data.Json.JsonObject jsonObj)
        {
            var type = jsonObj["idorder"].ValueType;

            this.Id = (int)jsonObj["idorder"].GetNumber();

            this.StartDate = DateTime.Parse(jsonObj["startdate"].GetString(), System.Globalization.CultureInfo.InvariantCulture);
            this.Ordersumm = decimal.Parse(jsonObj["ordersumm"].GetString(), System.Globalization.CultureInfo.InvariantCulture);

            var statusType = jsonObj["status"].ValueType;

            if (statusType == Windows.Data.Json.JsonValueType.Number)
            {
                this.Status = (int)jsonObj["status"].GetNumber();
            }

            if (statusType == Windows.Data.Json.JsonValueType.String)
            {
                this.Status = int.Parse(jsonObj["status"].GetString(), System.Globalization.CultureInfo.InvariantCulture);
            }


            var routemetersType = jsonObj["routemeters"].ValueType;

            if (routemetersType == Windows.Data.Json.JsonValueType.Number)
            {
                this.Routemeters = (int)jsonObj["routemeters"].GetNumber();
            }

            if (routemetersType == Windows.Data.Json.JsonValueType.String)
            {
                this.Routemeters = int.Parse(jsonObj["routemeters"].GetString(), System.Globalization.CultureInfo.InvariantCulture);
            }

            var routeArray = jsonObj["routes"].GetArray();

            for (int i = 0; i < routeArray.Count; i++)
            {
                string addr = routeArray[i].GetObject()["address"].GetString();
                string coords = routeArray[i].GetObject()["coords"].GetString();

                coords = coords.Trim();
                string[] coordsArray = coords.Split(',');

                OrderRouteItem routeItem = new OrderRouteItem()
                {
                    Address = addr,

                    Latitude = double.Parse(coordsArray[0], System.Globalization.CultureInfo.InvariantCulture),
                    Longitude = double.Parse(coordsArray[1], System.Globalization.CultureInfo.InvariantCulture)
                };

                this.route.Add(routeItem);
            }

            base.ReadData(jsonObj);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    

    public class OrderRouteItem
    {
        public string Address { get; set; }

        public double Latitude {get; set; }
        public double Longitude { get; set; }

    }
}
