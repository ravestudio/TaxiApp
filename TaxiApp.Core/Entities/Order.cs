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

        public int DriverId { get; set; }

        public decimal Ordersumm { get; set; }
        public int Routemeters { get; set; }
        public int Routetime { get; set; }

        public byte Servieces { get; set; }
        public int Carclass { get; set; }

        private IList<OrderRouteItem> route = new List<OrderRouteItem>();

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

        public List<KeyValuePair<string, string>> ConverToKeyValue()
        {
            List<KeyValuePair<string, string>> keyValueData = new List<KeyValuePair<string, string>>();

            //keyValueData.Add(new KeyValuePair<string, string>("enddate", System.DateTime.Now.AddHours(1).ToString("yyyy-mm-dd hh:mm")));

            //keyValueData.Add(new KeyValuePair<string, string>("enddate", System.DateTime.Now.AddHours(1).ToString("yyyy-MM-dd")));

            //keyValueData.Add(new KeyValuePair<string, string>("service", "1023"));
            //keyValueData.Add(new KeyValuePair<string, string>("passengersnum", "3"));


            int i = 0;
            foreach (OrderRouteItem routeItem in this.Route)
            {


                keyValueData.Add(new KeyValuePair<string, string>
                    (string.Format("address[{0}]", i), routeItem.Address));

                keyValueData.Add(new KeyValuePair<string, string>
                    (string.Format("coords[{0}]", i), string.Format("{0},{1}",
                    routeItem.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture),
                    routeItem.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture))));

                keyValueData.Add(new KeyValuePair<string, string>
                    (string.Format("priority[{0}]", i), routeItem.Priority.ToString()));

                i++;
            }

            if (Servieces > 0)
            {
                keyValueData.Add(new KeyValuePair<string, string>("service", Servieces.ToString()));
            }

            if (this.Route.Count > 0)
            {
                keyValueData.Add(new KeyValuePair<string, string>("routemeters", this.Routemeters.ToString()));
                keyValueData.Add(new KeyValuePair<string, string>("routetime", this.Routetime.ToString()));
            }

            //YYYY-MM-DD HH:II
            //string enddate = string.Format("{0}-{1}-{2} {3}:{4}", this.EndDate.Year, this.EndDate.Month,this.EndDate.Day, this.EndTime.Hour, this.EndTime.Minute);
            string enddate = string.Format("{0:yyyy}-{0:MM}-{0:dd} {0:HH}:{0:mm}", this.StartDate);

            keyValueData.Add(new KeyValuePair<string, string>("enddate", enddate));

            return keyValueData;
        }

        public override void ReadData(Windows.Data.Json.JsonObject jsonObj)
        {
            var type = jsonObj["idorder"].ValueType;

            this.Id = (int)jsonObj["idorder"].GetNumber();

            this.StartDate = DateTime.Parse(jsonObj["startdate"].GetString(), System.Globalization.CultureInfo.InvariantCulture);
            this.Ordersumm = decimal.Parse(jsonObj["ordersumm"].GetString(), System.Globalization.CultureInfo.InvariantCulture);

            this.Status = this.ReadValue(jsonObj, "status");
            this.DriverId = this.ReadValue(jsonObj, "iddriver");
            this.Routemeters = this.ReadValue(jsonObj, "routemeters");
            this.Carclass = this.ReadValue(jsonObj, "carclass");
            this.Servieces = (byte)this.ReadValue(jsonObj, "service");

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
        public int Priority { get; set; }
        public double Latitude {get; set; }
        public double Longitude { get; set; }

    }
}
