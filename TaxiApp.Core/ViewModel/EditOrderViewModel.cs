﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.DataModel;
using TaxiApp.Core.DataModel.Order;
using TaxiApp.Core.Messages;

using Windows.Devices.Geolocation;

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TaxiApp.Core;
using GalaSoft.MvvmLight;
using TaxiApp.Core.Managers;

namespace TaxiApp.Core.ViewModel
{
    public class EditOrderViewModel : ViewModelBase
    {
        public OrderModel OrderModel { get; set; }
        public SearchModel SearchModel { get; set; }

        public RelayCommand<object> ClickOrderItem { get; set; }
        
        public RelayCommand SelectServicesCmd { get; set; }
        
        public RelayCommand CreateOrderCmd { get; set; }
        
        public Command.ShowMenuCommand ShowMenuCmd { get; set; }
        public Command.NavigateToOrderListCommand NavToOrderListCmd { get; set; }
        public Command.NavigateToMainPageCommand NavToMainPageCmd { get; set; }
        
        public RelayCommand clickMenuBtn { get; set; }

        
        public IDictionary<Type, Windows.UI.Xaml.Controls.Grid> LayoutRootList { get; set; }

        public bool MenuState { get; set; }

        public IList<OrderOption> SelectedServices = null;

        //public ListPickerFlyout ServicePicker { get; set; }
        //public ListPickerFlyout CarPicker { get; set; }
        //public DatePickerFlyout DatePicker { get; set; }
        //public TimePickerFlyout TimePicker { get; set; }

        public OrderPriceInfo PriceInfo { get; set; }
        public bool LocationReady { get; set; }

        public Dictionary<string, Action<TaxiApp.Core.DataModel.Order.OrderItem>> Actions = null;
        public IList<OrderOption> ServiceList { get; set; }


        private ObservableCollection<OrderItem> _orderItemList = null;

        private TaxiApp.Core.Entities.Order _order = null;

        private IRoute _route = null;

        private IEditOrderControls _editOrderControls = null;

        public ObservableCollection<OrderItem> OrderItemList
        {
            get
            {
                return this._orderItemList;
            }
        }

        public DateTime _EndDate;
        public TimeSpan _EndTime;

        public TimeSpan EndTime {
            get
            {
                return this._EndTime;
            }
            set
            {
                this._EndTime = value;
                OrderItem item = this._orderItemList.Where(i => i.Cmd == "Now").Single();
                item.Title = string.Format("{0:D2}:{0:D2}", value.Hours, value.Minutes);
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this._EndDate;
            }
            set
            {
                this._EndDate = value;
                OrderItem item = this._orderItemList.Where(i => i.Cmd == "Date").Single();
                item.Title = string.Format("{0:yyyy}-{0:MM}-{0:dd}", value);
            }
        }

        public EditOrderViewModel(IEditOrderControls editOrderControls)
        {
            this._editOrderControls = editOrderControls;

            this.PriceInfo = new OrderPriceInfo();

            this._EndDate = DateTime.Now;
            this._EndTime = DateTime.Now.TimeOfDay;

            this.SelectedServices = new List<OrderOption>();

            Messenger.Default.Register<GetOptionsResultMessage>(this, (msg) =>
            {
                this.ServiceList = msg.ServiceList;
            });

            Messenger.Default.Send<GetOptionsMessage>(new GetOptionsMessage());



            this.ClickOrderItem = new RelayCommand<object>((parameter) =>
            {

                Windows.UI.Xaml.Controls.ItemClickEventArgs e = (Windows.UI.Xaml.Controls.ItemClickEventArgs)parameter;
                TaxiApp.Core.DataModel.Order.OrderItem orderItem = (TaxiApp.Core.DataModel.Order.OrderItem)e.ClickedItem;

                this.Actions[orderItem.Cmd].Invoke(orderItem);
            });
            
            
            this.SelectServicesCmd = new RelayCommand(() =>
            {
                //this.SelectedServices =
                //this.ServicePicker.SelectedItems.Cast<Core.DataModel.Order.OrderOption>().ToList();
            });
           
            
            this.CreateOrderCmd = new RelayCommand(() =>
            {

                this.SelectedServices = this._editOrderControls.GetServices();

                TaxiApp.Core.Entities.Order order = this.GetEntity();
                
                Messenger.Default.Send<CreateOrderMessage>(new CreateOrderMessage() { 
                  Order = order
                });
                
            });
            
            
            this.ShowMenuCmd = new Command.ShowMenuCommand(this);
            this.NavToOrderListCmd = new Command.NavigateToOrderListCommand();
            this.NavToMainPageCmd = new Command.NavigateToMainPageCommand();
            

            
            Messenger.Default.Register<LocationChangedMessage>(this, (msg) => {
                this.LocationReady = msg.Ready;
                //this.RaisePropertyChanged("LocationReady");
            });

            Messenger.Default.Register<RouteChangedMessage>(this, (msg) => {

                this._route = msg.route;
            });

            //this.LayoutRootList = new Dictionary<Type, Windows.UI.Xaml.Controls.Grid>();

            this.MenuState = false;

            this._orderItemList = new ObservableCollection<OrderItem>();

            OrderPoint pointfrom = new OrderPoint();
            pointfrom.Priority = 0;
            pointfrom.Title = "Откуда?";
            pointfrom.Location = new LocationItem() { Address = "Input address" };
            pointfrom.IconSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/AddressPoint.png"));

            OrderPoint pointSecond = new OrderPoint();
            pointSecond.Priority = 1;
            pointSecond.Title = "Куда?";
            pointSecond.Location = new LocationItem() { Address = "Input address" };
            pointSecond.IconSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/AddressPointTo.png"));

            this._orderItemList.Add(pointfrom);
            this._orderItemList.Add(pointSecond);

            this._orderItemList.Add(new OrderItem()
            {
                Priority = 9,
                Title = "Сегодня",
                Cmd = "Date",
                IconSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/time.png"))
            });


            this._orderItemList.Add(new OrderItem()
            {
                Priority = 10,
                Title = "Сейчас",
                Cmd = "Now",
                IconSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/time.png"))
            });

            this._orderItemList.Add(new OrderItem()
            {
                Priority = 11,
                Title = "Дополнительно",
                Cmd = "Services",
                IconSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/options.png"))
            });

            //this._orderItemList.Add(new OrderItem()
            //{
            //    Priority = 12,
            //    Title = "Car",
            //    Cmd = "Car"
            //    //IconSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/carclass.png"))
            //});

            this.Actions = new Dictionary<string, Action<TaxiApp.Core.DataModel.Order.OrderItem>>();

            this.Actions.Add("Point", (item) => {
                TaxiApp.Core.DataModel.Order.OrderPoint orderPoint = (TaxiApp.Core.DataModel.Order.OrderPoint)item;

                //SearchModel.SelectedPoint = orderPoint;

                Messenger.Default.Send<FillRoutePointMessage>(new FillRoutePointMessage()
                {
                    Point = orderPoint
                });
            });

            this.Actions.Add("Services", (item) =>
            {
                this._editOrderControls.OpenServicePicker(ServiceList);

            });

            this.Actions.Add("Date", (item) =>
            {
                //viewModel.DatePicker.ShowAt(viewModel.Page);
            });

            this.Actions.Add("Now", (item) =>
            {
                //viewModel.TimePicker.ShowAt(viewModel.Page);
            });

            this.Actions.Add("Car", (item) =>
            {
                //viewModel.CarPicker.ShowAt(viewModel.Page);
            });

            //OrderModel.SocketOnMessage = new OrderModel.SocketHandler((resp) =>
            //{
            //    if (resp.request == "neworderstatus")
            //    {
            //        var order = this.OrderList.SingleOrDefault(o => o.Id == resp.idorder);

            //        if (order!= null)
            //        {

            //            this.Page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //            {
            //                order.Status = resp.orderstatus;
            //            });
            //        }
            //    }
            //});
            
        }

        //public override void Init(Page page)
        //{
        //    base.Init(page);

        //    this.MenuState = false;

        //    //if (this.LayoutRootList.Keys.Contains(page.GetType()))
        //    //{
        //    //    this.LayoutRootList.Remove(page.GetType());
        //    //}
        //    //this.LayoutRootList.Add(page.GetType(), (Windows.UI.Xaml.Controls.Grid)page.FindName("LayoutRoot"));

        //    //if (page is TaxiApp.Views.MainPage)
        //    //{
        //    //    //this.ServicePicker = (ListPickerFlyout)page.Resources["ServiceFlyout"];
        //    //    //this.CarPicker = (ListPickerFlyout)page.Resources["CarFlyout"];
        //    //    //this.DatePicker = (DatePickerFlyout)page.Resources["DateFlyout"];
        //    //    //this.TimePicker = (TimePickerFlyout)page.Resources["TimeFlyout"];

                

        //    //    //this.LayoutRootList.Add(page.GetType(), (Windows.UI.Xaml.Controls.Grid)page.FindName("LayoutRoot"));

        //    //    //this.LayoutRoot = (Windows.UI.Xaml.Controls.Grid)page.FindName("LayoutRoot");

        //    //    foreach(OrderOption service in this.SelectedServices)
        //    //    {
        //    //        //this.ServicePicker.SelectedItems.Add(service);
        //    //    }
        //    //}


        //    //if (page is TaxiApp.Views.AddPointPage)
        //    //{
        //    //    this.Map.RouteMapControl = (Windows.UI.Xaml.Controls.Maps.MapControl)page.FindName("RouteMapControl");
        //    //}

        //    //this.OrderModel.Dispatcher = page.Dispatcher;

            
        //}




        public void UpdatePoints()
        {

            if (this._orderItemList.OfType<OrderPoint>().Count() == this._orderItemList.OfType<OrderPoint>().Where(p => p.IsDataReady()).Count())
            {
                OrderPoint newPoint = new OrderPoint();
                newPoint.Priority = this._orderItemList.OfType<OrderPoint>().Count();
                newPoint.Title = string.Format("Address {0}", newPoint.Priority);
                newPoint.Location = new LocationItem() { Address = string.Empty };
                newPoint.IconSource = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/AddressPointTo.png"));

                this._orderItemList.Add(newPoint);

                List<OrderItem> sortedList = this.OrderItemList.OrderBy(i => i.Priority).ToList();

                this.OrderItemList.Clear();

                foreach (OrderItem item in sortedList)
                {
                    this.OrderItemList.Add(item);
                }


            }
        }

        //public void ShowMenu()
        //{
        //    Frame rootFrame = Window.Current.Content as Frame;

        //    if (!this.MenuState)
        //    {
        //        this.LayoutRootList[rootFrame.SourcePageType].Margin = new Windows.UI.Xaml.Thickness(0, 0, 0, 0);
        //        this.MenuState = true;
        //    }
        //    else
        //    {
        //        this.LayoutRootList[rootFrame.SourcePageType].Margin = new Windows.UI.Xaml.Thickness(-160, 0, 0, 0);
        //        this.MenuState = false;
        //    }
        //}

        public TaxiApp.Core.Entities.Order GetEntity()
        {
            this._order = new Core.Entities.Order();

            byte servieces = 0;
                
            
            foreach (OrderOption service in this.SelectedServices)
            {
                servieces = (byte)((byte)servieces | (byte)service.id);
            }

            this._order.Servieces = servieces;

            foreach (OrderPoint orderPoint in this._orderItemList.OfType<OrderPoint>().Where(p => p.IsDataReady()))
            {
                TaxiApp.Core.Entities.OrderRouteItem routeItem = new TaxiApp.Core.Entities.OrderRouteItem();
                routeItem.Address = string.Format("{0}, {1}, {2} {3}",
                        orderPoint.Location.Location.Street,
                        orderPoint.Location.Location.StreetNumber,
                        orderPoint.Location.Location.Town,
                        orderPoint.Location.Location.Country
                        );
                routeItem.Latitude = orderPoint.Location.Latitude;
                routeItem.Longitude = orderPoint.Location.Longitude;

                _order.Route.Add(routeItem);
            }

            if (this._route != null)
            {
                _order.Routemeters = (int)this._route.LengthInMeters;
                _order.Routetime = this._route.EstimatedDuration;
            }

            _order.StartDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds);

            return this._order;
        }

    }
}
