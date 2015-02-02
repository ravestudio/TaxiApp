using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.DataModel;
using TaxiApp.Core.DataModel.Order;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.ViewModel
{
    public class OrderViewModel : ViewModel
    {
        public OrderDetail OrderModel { get; set; }
        public SearchModel SearchModel { get; set; }

        public Command.ClickOrderItemCommand ClickOrderItem { get; set; }
        public Command.SelectPointLocationCommand SelectLocationItem { get; set; }
        public Command.SelectServicesCommand SelectServicesCmd { get; set; }
        public Command.CancelOrderCommand CancelOrderCmd { get; set; }

        public Windows.UI.Xaml.Controls.Page Page { get; set; }

        public ListPickerFlyout ServicePicker { get; set; }
        public ListPickerFlyout CarPicker { get; set; }
        public TimePickerFlyout TimePicker { get; set; }

        public Dictionary<string, Action<ViewModel, TaxiApp.Core.DataModel.Order.OrderItem>> Actions = null;

        public ObservableCollection<Core.Entities.Order> OrderList { get; set; }

        public OrderViewModel()
        {
            this.OrderList = new ObservableCollection<Core.Entities.Order>();

            this.OrderModel = new OrderDetail();
            this.SearchModel = new SearchModel();

            this.ClickOrderItem = new Command.ClickOrderItemCommand(this);
            this.SelectLocationItem = new Command.SelectPointLocationCommand(this);
            this.SelectServicesCmd = new Command.SelectServicesCommand(this);
            this.CancelOrderCmd = new Command.CancelOrderCommand(this);

            this.Actions = new Dictionary<string, Action<ViewModel, TaxiApp.Core.DataModel.Order.OrderItem>>();

            this.Actions.Add("Point", (viewModel, item) => {
                TaxiApp.Core.DataModel.Order.OrderPoint orderPoint = (TaxiApp.Core.DataModel.Order.OrderPoint)item;

                Frame rootFrame = Window.Current.Content as Frame;

                OrderViewModel controller = (OrderViewModel)viewModel;

                controller.SearchModel.SelectedPoint = orderPoint;
                rootFrame.Navigate(typeof(Views.AddPointPage));
            });

            this.Actions.Add("Services", (viewModel, item) =>
            {
                OrderViewModel controller = (OrderViewModel)viewModel;

                controller.ServicePicker.ShowAt(controller.Page);

            });

            this.Actions.Add("Now", (viewModel, item) =>
            {
                OrderViewModel controller = (OrderViewModel)viewModel;

                controller.TimePicker.ShowAt(controller.Page);

            });

            this.Actions.Add("Car", (viewModel, item) =>
            {
                OrderViewModel controller = (OrderViewModel)viewModel;

                controller.CarPicker.ShowAt(controller.Page);

            });
        }

        public override void Init(Page page)
        {
            this.Page = page;

            if (page.Name == "mainPage")
            {
                this.ServicePicker = (ListPickerFlyout)page.Resources["ServiceFlyout"];
                this.CarPicker = (ListPickerFlyout)page.Resources["CarFlyout"];
                this.TimePicker = (TimePickerFlyout)page.Resources["TimeFlyout"];
            }
            else
            {
                //this.OrderModel.RouteMapControl = ((Views.AddPointPage)page).RouteMapControl;
            }

            this.OrderModel.Dispatcher = page.Dispatcher;

            base.Init(page);
        }

        public async void CreateOrder()
        {
            TaxiApp.Core.Entities.User user = TaxiApp.Core.Session.Instance.GetUser();

            var postData = this.OrderModel.ConverToKeyValue();

            //var postData = new List<KeyValuePair<string, string>>();

            postData.Add(new KeyValuePair<string, string>("idpassenger", user.Id.ToString()));
            postData.Add(new KeyValuePair<string, string>("token", user.token));
            postData.Add(new KeyValuePair<string, string>("idcompany", "1"));



            TaxiApp.Core.WebApiClient client = new TaxiApp.Core.WebApiClient();

            string url = "http://serv.giddix.ru/api/passenger_setorder/";

            string data = await client.GetData(url, postData);
        }

        public async Task<IList<TaxiApp.Core.Entities.Order>> GetUserOrders()
        {
            TaxiApp.Core.WebApiClient client = new TaxiApp.Core.WebApiClient();       

            TaxiApp.Core.Entities.User user = TaxiApp.Core.Session.Instance.GetUser();

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
    }
}
