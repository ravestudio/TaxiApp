﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;

using TaxiApp.Core.Repository;
using TaxiApp.Core.Messages;
using Windows.Devices.Geolocation;
using GalaSoft.MvvmLight.Messaging;
using TaxiApp.Core.Managers;

namespace TaxiApp.Core.DataModel.Order
{
    public class OrderModel
    {

        public delegate void SocketHandler(Socket.SocketResponse resp);

        public SocketHandler SocketOnMessage;

        public IList<OrderOption> _orderServiceList = null;
        public IList<OrderOption> _orderCarList = null;

        private TaxiApp.Core.SocketClient SocketClient = new Core.SocketClient();
        private TaxiApp.Core.Socket.SocketManager socketMG = null;

        private IDictionary<int,LocationItem> tempLocations = null;

        public TaxiApp.Core.Entities.Order Detailed { get; set; }
        //public Windows.UI.Core.CoreDispatcher Dispatcher { get; set; }

        //public Windows.UI.Xaml.Controls.Primitives.Popup ServicePopup { get; set; }
        //public Windows.UI.Xaml.Controls.Primitives.Popup DateTimePopup { get; set; }

        private OrderRepository _orderRepository = null;
        private DriverRepository _driverRepository = null;
        private LocationManager _locationMG = null;

        public OrderModel(OrderRepository orderRepository, DriverRepository driverRepository, LocationManager locationManager)
        {
            this.tempLocations = new Dictionary<int, LocationItem>();

            this._orderRepository = orderRepository;
            this._driverRepository = driverRepository;
            this._locationMG = locationManager;
            
            socketMG = new Core.Socket.SocketManager(this.SocketClient);

            InitOptions();

            //this.PriceInfo.Time = "40min";
            //this.PriceInfo.Price = "$7.5";
            //this.PriceInfo.Destination = "7.3km";

            //this.MapRouteChanged += OrderModel_MapRouteChanged;

            string Host = "178.208.77.21";
            string Port = "9090";

            //SocketClient.ConnectAsync(Host, Port).ContinueWith(t =>
            //{
            //    string res = "ok";

            //    socketMG.Start();

            //});

            SocketClient.OnMessage += SocketClient_OnMessage;

            Messenger.Default.Register<LoadOrderListMessage>(this, async (msg) =>
            {
                TaxiApp.Core.Entities.IUser user = TaxiApp.Core.Session.Instance.GetUser();

                IList<TaxiApp.Core.Entities.Order> orderList = await this._orderRepository.GetUserOrders(user);

                Messenger.Default.Send<OrderListloadedMessage>(new OrderListloadedMessage()
                {
                    orderList = orderList
                });
            });
            
            Messenger.Default.Register<CreateOrderMessage>(this, async (msg) => {
                    string data = await this._orderRepository.CreateOrder(msg.Order);
                });
                
            Messenger.Default.Register<DeleteOrderMessage>(this, async (msg) => {
                    bool deleted = await _orderRepository.DeleteOrder(msg.OrderId);
                    if (deleted)
                    {
                        Messenger.Default.Send<OrderDeletedMessage>(new OrderDeletedMessage() { 
                            OrderId = msg.OrderId
                        });
                    }
                });

                
            Messenger.Default.Register<SelectOrderMessage>(this, (msg) => {
                    this.Detailed = msg.Order;
                });

            Messenger.Default.Register<GetOptionsMessage>(this, (msg) => {
                Messenger.Default.Send<GetOptionsResultMessage>(new GetOptionsResultMessage()
                {
                    ServiceList = this._orderServiceList
                });
            });

            Messenger.Default.Register<SelectLocationMessage>(this, async (msg) =>
            {
                if (this.tempLocations.ContainsKey(msg.Priority))
                {
                    this.tempLocations[msg.Priority] = msg.LocationItem;
                }
                else
                {
                    this.tempLocations.Add(msg.Priority, msg.LocationItem);
                }

                IEnumerable<Geopoint> geopoints = this.tempLocations.Values
                .Select(p => new Geopoint(new BasicGeoposition()
                {
                    Latitude = p.Location.Latitude,
                    Longitude = p.Location.Longitude
                }));

                IRoute route = await this.FindRoute(geopoints);

                Messenger.Default.Send<RouteChangedMessage>(new RouteChangedMessage()
                {
                    points = geopoints,
                    route = route
                });
            });
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
            this._orderServiceList.Add(new OrderOption() { id = 1, Name = "Багаж", IconSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/service/s1.png")) });
            this._orderServiceList.Add(new OrderOption() { id = 2, Name = "Можно курить", IconSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/service/s2.png")) });
            this._orderServiceList.Add(new OrderOption() { id = 4, Name = "Водитель не курит", IconSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/service/s4.png")) });
            this._orderServiceList.Add(new OrderOption() { id = 8, Name = "Детское кресло", IconSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/service/s8.png")) });
            this._orderServiceList.Add(new OrderOption() { id = 16, Name = "Удобства для инвалидов", IconSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/service/s16.png")) });
            this._orderServiceList.Add(new OrderOption() { id = 32, Name = "Перевозка животных ", IconSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/service/s32.png")) });

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

        public void ShowRoute(IRoute route)
        {
            if (route != null)
            {
                //Core.Managers.MapPainter painter = Core.Managers.ManagerFactory.Instance.GetMapPainter(mapControl);
                Core.Managers.MapPainter painter = null;

                //Task showRoutTask = painter.ShowRoute(mapControl, route).ContinueWith(t =>
                //{
                //    string msg = "route showed";
                //});

                painter.ShowRoute(route);
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

        public Task<IRoute> FindRoute(IEnumerable<Geopoint> geopoints)
        {
            TaskCompletionSource<IRoute> TCS = new TaskCompletionSource<IRoute>();

            int thread = Environment.CurrentManagedThreadId;

            //Managers.LocationManager locationMG = Managers.ManagerFactory.Instance.GetLocationManager();

            if (geopoints.Count() > 1)
            {
                TCS.SetResult(this._locationMG.GetRoute(geopoints).Result);
            }
            else
            {
                TCS.SetResult(null);
            }

            return TCS.Task;
        }

        //public void ShowServices()
        //{
        //    this.ServicePopup.IsOpen = true;
        //}

        //public void ShowDateTime()
        //{
        //    this.DateTimePopup.IsOpen = true;
        //}
        

        public Task<Entities.Driver> GetDriver(int DriverId)
        {
            var tcs = new TaskCompletionSource<Entities.Driver>();

            this._driverRepository.GetById(DriverId).ContinueWith((task) =>
                {
                    tcs.SetResult(task.Result);
                });

            return tcs.Task;
        }

        public async void GetPriceInfo(TaxiApp.Core.Entities.Order order, OrderPriceInfo priceInfo)
        {
            TaxiApp.Core.Entities.IUser user = TaxiApp.Core.Session.Instance.GetUser();

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

            string url = "http://serv.cabswap.com/api/passenger_getprice/";

            string data = await client.GetData(url, postData);

            var Obj = Windows.Data.Json.JsonValue.Parse(data).GetObject();

            var Resp = Obj["response"].GetObject();

            int price = (int)Resp["price"].GetNumber();

            priceInfo.Price = string.Format("${0}", price);

            priceInfo.Visible = true;
        }
    }
}
