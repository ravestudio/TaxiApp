﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.DataModel;
using TaxiApp.Core.DataModel.Order;

using Windows.Devices.Geolocation;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.ViewModel
{
    public class EditOrderViewModel : ViewModel
    {
        public OrderModel OrderModel { get; set; }
        public SearchModel SearchModel { get; set; }

        public RelayCommand<object> ClickOrderItem { get; set; }
        public RelayCommand<object> SelectLocationItem { get; set; }
        public RelayCommand SelectServicesCmd { get; set; }
        public RelayCommand CancelOrderCmd { get; set; }
        public RelayCommand CreateOrderCmd { get; set; }
        public RelayCommand ShowOrderDetailCmd { get; set; }
        public Command.ShowMenuCommand ShowMenuCmd { get; set; }
        public Command.NavigateToOrderListCommand NavToOrderListCmd { get; set; }
        public Command.NavigateToMainPageCommand NavToMainPageCmd { get; set; }
        public RelayCommand<object> SelectMyOrderCmd { get; set; }

        public IDictionary<Type, Windows.UI.Xaml.Controls.Grid> LayoutRootList { get; set; }

        public bool MenuState { get; set; }

        public IList<OrderOption> SelectedServices = null;

        public ListPickerFlyout ServicePicker { get; set; }
        public ListPickerFlyout CarPicker { get; set; }
        public DatePickerFlyout DatePicker { get; set; }
        public TimePickerFlyout TimePicker { get; set; }

        public OrderPriceInfo PriceInfo { get; set; }
        public bool LocationReady {
            get
            {
                return this.SearchModel.LocationReady;
            }
        }

        public Dictionary<string, Action<EditOrderViewModel, TaxiApp.Core.DataModel.Order.OrderItem>> Actions = null;

        public ObservableCollection<Core.Entities.Order> OrderList { get; set; }

        private ObservableCollection<OrderItem> _orderItemList = null;

        public MapViewModel Map { get; set; }

        private TaxiApp.Core.Entities.Order _order = null;
        

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

        public EditOrderViewModel()
        {
            this.Map = new MapViewModel();
            this.PriceInfo = new OrderPriceInfo();

            this._EndDate = DateTime.Now;
            this._EndTime = DateTime.Now.TimeOfDay;

            this.SelectedServices = new List<OrderOption>();

            this.OrderList = new ObservableCollection<Core.Entities.Order>();

            this.OrderModel = TaxiApp.Core.DataModel.ModelFactory.Instance.GetOrderModel();
            this.SearchModel = TaxiApp.Core.DataModel.ModelFactory.Instance.GetSearchModel();

            this.ClickOrderItem = new RelayCommand((parameter) =>
            {
                Windows.UI.Xaml.Controls.ItemClickEventArgs e = (Windows.UI.Xaml.Controls.ItemClickEventArgs)parameter;
                TaxiApp.Core.DataModel.Order.OrderItem orderItem = (TaxiApp.Core.DataModel.Order.OrderItem)e.ClickedItem;

                this.Actions[orderItem.Cmd].Invoke(this, orderItem);
            });
            
            this.SelectLocationItem = new RelayCommand((parameter) =>
            {
                Windows.UI.Xaml.Controls.ItemClickEventArgs e = (Windows.UI.Xaml.Controls.ItemClickEventArgs)parameter;
                LocationItem location = (LocationItem)e.ClickedItem;
                
                Messenger.Default.Send<SelectLocationMessage>(new SelectLocationMessage() { 
                  Location = location;
                });
                Frame rootFrame = Window.Current.Content as Frame;

                this.UpdatePoints();
                rootFrame.GoBack();
            });
            
            this.SelectServicesCmd = new RelayCommand(() =>
            {
                this.SelectedServices =
                this.ServicePicker.SelectedItems.Cast<Core.DataModel.Order.OrderOption>().ToList();
            });
            
            this.CancelOrderCmd = new RelayCommand(() =>
            {
                Core.Entities.Order order = this.OrderList.SingleOrDefault(o => o.Selected == true);

                var dlg = new Windows.UI.Popups.MessageDialog("Отменить заказ?");

                dlg.Commands.Add(new Windows.UI.Popups.UICommand("Accept"));
                dlg.Commands.Add(new Windows.UI.Popups.UICommand("Cancel"));

                Task<Windows.UI.Popups.IUICommand> dlgTask = dlg.ShowAsync().AsTask();

                dlgTask.ContinueWith((dialogResult) =>
                {
                    if (dialogResult.Result.Label == "Accept")
                    {
                        Messenger.Default.Send<DeleteOrderMessage>(new DeleteOrderMessage() { 
                            OrderId = order.Id;
                        });
                    }
                });
            });
            
            this.CreateOrderCmd = new RelayCommand(() =>
            {
                TaxiApp.Core.Entities.Order order = this.GetEntity();
                
                Messenger.Default.Send<CreateOrderMessage>(new CreateOrderMessage() { 
                  Order = order;
                });
                
                this.LoadMyOrders();
            });
            
            this.ShowOrderDetailCmd = new RelayCommand(() =>
            {
                Core.Entities.Order order = this.OrderList.SingleOrDefault(o => o.Selected == true);

                Messenger.Default.Send<SelectOrderMessage>(new SelectOrderMessage() { 
                  Order = order;
                });

                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(Views.OrderDetailPage));
            });
            
            this.ShowMenuCmd = new Command.ShowMenuCommand(this);
            this.NavToOrderListCmd = new Command.NavigateToOrderListCommand();
            this.NavToMainPageCmd = new Command.NavigateToMainPageCommand();
            
            this.SelectMyOrderCmd = new RelayCommand((parameter) =>
            {
                Windows.UI.Xaml.Controls.ItemClickEventArgs e = (Windows.UI.Xaml.Controls.ItemClickEventArgs)parameter;
                Core.Entities.Order SelectedOrder = (Core.Entities.Order)e.ClickedItem;
                
                foreach(Core.Entities.Order order in this.OrderList)
                {
                    order.Selected = false;
                }
                SelectedOrder.Selected = true;
            });

            this.LayoutRootList = new Dictionary<Type, Windows.UI.Xaml.Controls.Grid>();

            this.MenuState = false;

            this.SearchModel.LocationReadyChanged = new SearchModel.LocationReadyDelegate((ready) =>
            {
                this.NotifyPropertyChanged("LocationReady");
            });

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

            this.Actions = new Dictionary<string, Action<EditOrderViewModel, TaxiApp.Core.DataModel.Order.OrderItem>>();

            this.Actions.Add("Point", (viewModel, item) => {
                TaxiApp.Core.DataModel.Order.OrderPoint orderPoint = (TaxiApp.Core.DataModel.Order.OrderPoint)item;

                Frame rootFrame = Window.Current.Content as Frame;

                viewModel.SearchModel.SelectedPoint = orderPoint;
                rootFrame.Navigate(typeof(Views.AddPointPage));
            });

            this.Actions.Add("Services", (viewModel, item) =>
            {
                viewModel.ServicePicker.ShowAt(viewModel.Page);

            });

            this.Actions.Add("Date", (viewModel, item) =>
            {
                viewModel.DatePicker.ShowAt(viewModel.Page);
            });

            this.Actions.Add("Now", (viewModel, item) =>
            {
                viewModel.TimePicker.ShowAt(viewModel.Page);
            });

            this.Actions.Add("Car", (viewModel, item) =>
            {
                viewModel.CarPicker.ShowAt(viewModel.Page);
            });

            OrderModel.SocketOnMessage = new OrderModel.SocketHandler((resp) =>
            {
                if (resp.request == "neworderstatus")
                {
                    var order = this.OrderList.SingleOrDefault(o => o.Id == resp.idorder);

                    if (order!= null)
                    {

                        this.Page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                        {
                            order.Status = resp.orderstatus;
                        });
                    }
                }
            });
            
        }

        public override void Init(Page page)
        {
            base.Init(page);

            this.MenuState = false;

            if (this.LayoutRootList.Keys.Contains(page.GetType()))
            {
                this.LayoutRootList.Remove(page.GetType());
            }
            this.LayoutRootList.Add(page.GetType(), (Windows.UI.Xaml.Controls.Grid)page.FindName("LayoutRoot"));

            if (page is TaxiApp.Views.MainPage)
            {
                this.ServicePicker = (ListPickerFlyout)page.Resources["ServiceFlyout"];
                this.CarPicker = (ListPickerFlyout)page.Resources["CarFlyout"];
                this.DatePicker = (DatePickerFlyout)page.Resources["DateFlyout"];
                this.TimePicker = (TimePickerFlyout)page.Resources["TimeFlyout"];

                

                //this.LayoutRootList.Add(page.GetType(), (Windows.UI.Xaml.Controls.Grid)page.FindName("LayoutRoot"));

                //this.LayoutRoot = (Windows.UI.Xaml.Controls.Grid)page.FindName("LayoutRoot");

                foreach(OrderOption service in this.SelectedServices)
                {
                    this.ServicePicker.SelectedItems.Add(service);
                }
            }


            //if (page is TaxiApp.Views.AddPointPage)
            //{
            //    this.Map.RouteMapControl = (Windows.UI.Xaml.Controls.Maps.MapControl)page.FindName("RouteMapControl");
            //}

            //this.OrderModel.Dispatcher = page.Dispatcher;

            
        }

        public async Task<IList<TaxiApp.Core.Entities.Order>> GetUserOrders()
        {
            TaxiApp.Core.WebApiClient client = new TaxiApp.Core.WebApiClient();       

            TaxiApp.Core.Entities.IUser user = TaxiApp.Core.Session.Instance.GetUser();

            TaxiApp.Core.Repository.OrderRepository orderRepository = new Core.Repository.OrderRepository(client);

            IList<TaxiApp.Core.Entities.Order> orderList = await orderRepository.GetUserOrders(user);

            return orderList;
        }

        public async void LoadMyOrders()
        {
            IList<Core.Entities.Order> orderList = await this.GetUserOrders();

            //Windows.Foundation.IAsyncAction action =
            //    this.Page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //    {
            //        this.OrderList.Clear();
            //        foreach (Core.Entities.Order order in orderList)
            //        {
            //            this.OrderList.Add(order);
            //        }
            //    });

            this.OrderList.Clear();
            foreach (Core.Entities.Order order in orderList)
            {
                this.OrderList.Add(order);
            }

        }

        public void UpdatePoints()
        {

            IEnumerable<Geopoint> geopoints = this._orderItemList.OfType<OrderPoint>().Where(p => p.IsDataReady())
                .OrderBy(p => p.Priority)
                .Select(p => new Geopoint(new BasicGeoposition()
                {
                    Latitude = p.Location.Latitude,
                    Longitude = p.Location.Longitude
                }));

            Task<Core.IRoute> FindRouteTask = this.OrderModel.FindRoute(geopoints);

            FindRouteTask.ContinueWith(t =>
            {
                if (t.Status == TaskStatus.RanToCompletion)
                {

                    this.Map.MapRoute = t.Result;

                    Windows.Foundation.IAsyncAction action =
                    this.Page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        var dlg = new Windows.UI.Popups.MessageDialog("Маршрут найден");
                        dlg.ShowAsync();

                        TaxiApp.Core.Entities.Order order = null;
                        order = this.GetEntity();

                        this.OrderModel.GetPriceInfo(order, this.PriceInfo);
                    });
                }
                else
                {

                    Windows.Foundation.IAsyncAction action =
                    this.Page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        var dlg = new Windows.UI.Popups.MessageDialog("Ошибка при поиске маршрута");
                        dlg.ShowAsync();
                    });
                }
            });

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

        public void ShowMenu()
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (!this.MenuState)
            {
                this.LayoutRootList[rootFrame.SourcePageType].Margin = new Windows.UI.Xaml.Thickness(0, 0, 0, 0);
                this.MenuState = true;
            }
            else
            {
                this.LayoutRootList[rootFrame.SourcePageType].Margin = new Windows.UI.Xaml.Thickness(-160, 0, 0, 0);
                this.MenuState = false;
            }
        }

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

            if (this.Map.MapRoute != null)
            {
                _order.Routemeters = (int)this.Map.MapRoute.LengthInMeters;
                _order.Routetime = this.Map.MapRoute.EstimatedDuration;
            }

            _order.StartDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, EndTime.Hours, EndTime.Minutes, EndTime.Seconds);

            return this._order;
        }

    }
}