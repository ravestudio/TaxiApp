﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.DataModel;
using TaxiApp.Core.DataModel.Order;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.Controller
{
    public class OrderController : Controller
    {
        public OrderDetail OrderModel { get; set; }
        public SearchModel SearchModel { get; set; }
        public Command.ClickOrderItemCommand ClickOrderItem { get; set; }
        public Command.SelectPointLocationCommand SelectItem { get; set; }

        public Windows.UI.Xaml.Controls.Page Page { get; set; }
        public ListPickerFlyout ServicePicker { get; set; }

        public Dictionary<string, Action<Controller, TaxiApp.Core.DataModel.Order.OrderItem>> Actions = null;

        public OrderController()
        {
            this.OrderModel = new OrderDetail();
            this.SearchModel = new SearchModel();

            this.ClickOrderItem = new Command.ClickOrderItemCommand(this);
            this.SelectItem = new Command.SelectPointLocationCommand(this);

            this.Actions = new Dictionary<string, Action<Controller, TaxiApp.Core.DataModel.Order.OrderItem>>();

            this.Actions.Add("Point", (contrl, item) => {
                TaxiApp.Core.DataModel.Order.OrderPoint orderPoint = (TaxiApp.Core.DataModel.Order.OrderPoint)item;

                Frame rootFrame = Window.Current.Content as Frame;

                OrderController controller = (OrderController)contrl;

                controller.SearchModel.SelectedPoint = orderPoint;
                rootFrame.Navigate(typeof(AddPointPage));
            });

            this.Actions.Add("Services", (contrl, item) =>
            {
                OrderController controller = (OrderController)contrl;

                controller.ServicePicker.ShowAt(controller.Page);

            });

            this.Actions.Add("Now", (contrl, item) =>
            {
                OrderController controller = (OrderController)contrl;

                controller.OrderModel.ShowDateTime();

            });
        }

        public override void Init(Page page)
        {
            this.Page = page;

            this.ServicePicker = (ListPickerFlyout)page.Resources["ServiceFlyout"];

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
    }
}
