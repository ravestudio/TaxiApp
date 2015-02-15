using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

namespace TaxiApp.Core.DataModel.Order
{
    public class OrderModel
    {

        public delegate void SocketHandler(Socket.SocketResponse resp);

        public SocketHandler SocketOnMessage;

        private IList<OrderOption> _orderServiceList = null;
        private IList<OrderOption> _orderCarList = null;

        private TaxiApp.Core.SocketClient SocketClient = new Core.SocketClient();
        private TaxiApp.Core.Socket.SocketManager socketMG = null;


        public TaxiApp.Core.Entities.Order Detailed { get; set; }
        //public Windows.UI.Core.CoreDispatcher Dispatcher { get; set; }

        //public Windows.UI.Xaml.Controls.Primitives.Popup ServicePopup { get; set; }
        //public Windows.UI.Xaml.Controls.Primitives.Popup DateTimePopup { get; set; }

        public OrderModel()
        {
            socketMG = new Core.Socket.SocketManager(this.SocketClient);

            InitOptions();

            //this.PriceInfo.Time = "40min";
            //this.PriceInfo.Price = "$7.5";
            //this.PriceInfo.Destination = "7.3km";

            //this.MapRouteChanged += OrderModel_MapRouteChanged;

            string Host = "194.58.102.129";
            string Port = "9090";

            SocketClient.ConnectAsync(Host, Port).ContinueWith(t =>
            {
                string res = "ok";

                socketMG.Start();

            });

            SocketClient.OnMessage += SocketClient_OnMessage;
        }

        void SocketClient_OnMessage(object sender, EventArgs e)
        {
            TaxiApp.Core.SocketClient client = (TaxiApp.Core.SocketClient)sender;
            string msg = client.Message;

            Task.Delay(2000).ContinueWith((task) =>
                {
                    ProcSocketResp(msg);
                });
        }

        void ProcSocketResp(string msg)
        {
            if (msg == "40")
            {
                SocketAuth();
            }

            if (msg.StartsWith("42"))
            {
                Socket.SocketResponse resp = socketMG.ProcSocketResp(msg);

                if (this.SocketOnMessage != null)
                {
                    this.SocketOnMessage(resp);
                }
            }
        }

        void SocketAuth()
        {
            socketMG.Auth().ContinueWith(w =>
            {
                string res = "ok";
            });
        }

        private void InitOptions()
        {
            this._orderServiceList = new List<OrderOption>();
            this._orderServiceList.Add(new OrderOption() { id = 1, Name = "Багаж"});
            this._orderServiceList.Add(new OrderOption() { id = 2, Name = "Можно курить"});
            this._orderServiceList.Add(new OrderOption() { id = 4, Name = "Водитель не курит"});
            this._orderServiceList.Add(new OrderOption() { id = 8, Name = "Детское кресло"});
            this._orderServiceList.Add(new OrderOption() { id = 16, Name = "Удобства для инвалидов"});
            this._orderServiceList.Add(new OrderOption() { id = 32, Name = "Перевозка животных "});

            this._orderCarList = new List<OrderOption>();
            this._orderCarList.Add(new OrderOption() { id = 0, Name = "Любой" });
            this._orderCarList.Add(new OrderOption() { id = 1, Name = "Sedan" });
            this._orderCarList.Add(new OrderOption() { id = 2, Name = "Universal" });
            this._orderCarList.Add(new OrderOption() { id = 3, Name = "Minivan" });
            this._orderCarList.Add(new OrderOption() { id = 4, Name = "Offroad" });
            this._orderCarList.Add(new OrderOption() { id = 5, Name = "Pickup" });
            this._orderCarList.Add(new OrderOption() { id = 6, Name = "Limousine" });
            this._orderCarList.Add(new OrderOption() { id = 7, Name = "Cabriolet" });
            this._orderCarList.Add(new OrderOption() { id = 8, Name = "Sport car" });
            this._orderCarList.Add(new OrderOption() { id = 9, Name = "Rickshaw" });
        }

        //void OrderModel_MapRouteChanged(object sender, EventArgs e)
        //{
        //    int thread = Environment.CurrentManagedThreadId;

        //    this.ShowRoute();

        //}

        public void ShowRoute(MapControl mapControl, Windows.Services.Maps.MapRoute route)
        {
            if (route != null)
            {
                Core.Managers.MapPainter painter = Core.Managers.ManagerFactory.Instance.GetMapPainter();

                Task showRoutTask = painter.ShowRoute(mapControl, route).ContinueWith(t =>
                {
                    string msg = "route showed";
                });
            }
        }

        //private Windows.Services.Maps.MapRoute mapRoute = null;
        //public Windows.Services.Maps.MapRoute MapRoute
        //{
        //    get
        //    { return this.mapRoute; }
        //    set
        //    {
        //        this.mapRoute = value;

        //        NotifyMapRouteChanged();
        //    }
        //}

        //public event EventHandler MapRouteChanged;

        //public void NotifyMapRouteChanged()
        //{
        //    if (MapRouteChanged != null)
        //    {
        //        EventArgs args = new EventArgs();
        //        MapRouteChanged(this, args);
        //    }
        //}

        public IList<OrderOption> OrderServiceList
        {
            get
            {
                return this._orderServiceList;
            }
        }

        public IList<OrderOption> OrderCarList
        {
            get
            {
                return this._orderCarList;
            }
        }

        public async Task<Windows.Services.Maps.MapRoute> FindRoute(IEnumerable<Geopoint> geopoints)
        {
            Windows.Services.Maps.MapRoute route = null;

            int thread = Environment.CurrentManagedThreadId;

            Managers.LocationManager locationMG = Managers.ManagerFactory.Instance.GetLocationManager();

            if (geopoints.Count() > 1)
            {
                Windows.Services.Maps.MapRouteFinderResult routeResult = null;
                Task<Windows.Services.Maps.MapRouteFinderResult> routeTask = locationMG.GetRoute(geopoints);

                routeResult = await routeTask;

                if (routeResult.Status == Windows.Services.Maps.MapRouteFinderStatus.Success)
                {
                    route = routeResult.Route;
                }

            }

            return route;
        }

        public async Task ShowMyPossitionAsync(MapControl mapControl)
        {
            Managers.MapPainter painter = Managers.ManagerFactory.Instance.GetMapPainter();
            await painter.ShowMyPossitionAsync(mapControl);
        }

        //public void ShowServices()
        //{
        //    this.ServicePopup.IsOpen = true;
        //}

        //public void ShowDateTime()
        //{
        //    this.DateTimePopup.IsOpen = true;
        //}

        public async void CreateOrder(TaxiApp.Core.Entities.Order order)
        {
            TaxiApp.Core.Entities.User user = TaxiApp.Core.Session.Instance.GetUser();

            var postData = order.ConverToKeyValue();

            //var postData = new List<KeyValuePair<string, string>>();

            postData.Add(new KeyValuePair<string, string>("idpassenger", user.Id.ToString()));
            postData.Add(new KeyValuePair<string, string>("token", user.token));
            postData.Add(new KeyValuePair<string, string>("idcompany", "1"));



            TaxiApp.Core.WebApiClient client = new TaxiApp.Core.WebApiClient();

            string url = "http://serv.giddix.ru/api/passenger_setorder/";

            string data = await client.GetData(url, postData);


        }

        public Task<Entities.Driver> GetDriver(int DriverId)
        {
            var tcs = new TaskCompletionSource<Entities.Driver>();

            TaxiApp.Core.WebApiClient client = new TaxiApp.Core.WebApiClient();

            TaxiApp.Core.Repository.DriverRepository driverRepository = new Core.Repository.DriverRepository(client);

            driverRepository.GetById(DriverId).ContinueWith((task) =>
                {
                    tcs.SetResult(task.Result);
                });

            return tcs.Task;
        }

        public async void GetPriceInfo(TaxiApp.Core.Entities.Order order, OrderPriceInfo priceInfo)
        {
            TaxiApp.Core.Entities.User user = TaxiApp.Core.Session.Instance.GetUser();

            List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();

            string distance = "0";
            string minutes = "0";

            priceInfo.Destination = string.Empty;
            priceInfo.Time = string.Empty;

            if (order.Route.Count > 0)
            {
                distance = order.Routemeters.ToString();
                minutes = order.Routetime.ToString();

                priceInfo.Destination = string.Format("{0}km", order.Routemeters/1000);
                priceInfo.Time = string.Format("{0}min", order.Routetime);
            }

            postData.Add(new KeyValuePair<string, string>("distance", distance));
            postData.Add(new KeyValuePair<string, string>("minutes", minutes));

            //YYYY-MM-DD HH:II
            string enddate = string.Format("{0:yyyy}-{0:MM}-{0:dd} {0:HH}:{0:mm}", order.StartDate);

            postData.Add(new KeyValuePair<string, string>("orderenddate", enddate));

            postData.Add(new KeyValuePair<string, string>("carclass", "0"));
            //var postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("passengersnum", "1"));
            

            postData.Add(new KeyValuePair<string, string>("idpassenger", user.Id.ToString()));
            postData.Add(new KeyValuePair<string, string>("token", user.token));
            //postData.Add(new KeyValuePair<string, string>("idcompany", "1"));



            TaxiApp.Core.WebApiClient client = new TaxiApp.Core.WebApiClient();

            string url = "http://serv.giddix.ru/api/passenger_getprice/";

            string data = await client.GetData(url, postData);

            var Obj = Windows.Data.Json.JsonValue.Parse(data).GetObject();

            var Resp = Obj["response"].GetObject();

            int price = (int)Resp["price"].GetNumber();

            priceInfo.Price = string.Format("${0}", price);

            priceInfo.Visible = true;
        }
    }
}
