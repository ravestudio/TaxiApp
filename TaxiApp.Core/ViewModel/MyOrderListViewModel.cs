using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace TaxiApp.Core.ViewModel
{
    public class MyOrderListViewModel : ViewModelBase
    {
        public ObservableCollection<Core.Entities.Order> OrderList { get; set; }

        public RelayCommand CancelOrderCmd { get; set; }
        public RelayCommand ShowOrderDetailCmd { get; set; }
        public RelayCommand<object> SelectMyOrderCmd { get; set; }

        public MyOrderListViewModel()
        {

            this.OrderList = new ObservableCollection<Core.Entities.Order>();

            if (this.IsInDesignMode)
            {
                var order = new Entities.Order()
                {
                    Id = 10,
                    Ordersumm = 450,
                    Routemeters = 7500
                };

                order.Route.Add(new Entities.OrderRouteItem()
                {
                    Address = "Жуковского 33"
                });

                order.Route.Add(new Entities.OrderRouteItem()
                {
                    Address = "Маркса 89"
                });

                this.OrderList.Add(order);

                order = new Entities.Order()
                {
                    Id = 15,
                    Ordersumm = 200,
                    Routemeters = 3000
                };

                order.Route.Add(new Entities.OrderRouteItem()
                {
                    Address = "Ленина 20"
                });

                order.Route.Add(new Entities.OrderRouteItem()
                {
                    Address = "Лермонтова 15"
                });

                this.OrderList.Add(order);
            }

            this.CancelOrderCmd = new RelayCommand(async () =>
            {
                Core.Entities.Order order = this.OrderList.SingleOrDefault(o => o.Selected == true);

                var dlg = new Windows.UI.Popups.MessageDialog("Отменить заказ?");

                dlg.Commands.Add(new Windows.UI.Popups.UICommand("Accept"));
                dlg.Commands.Add(new Windows.UI.Popups.UICommand("Cancel"));

                var dialogResult = await dlg.ShowAsync();

                if (dialogResult.Label == "Accept")
                {
                    Messenger.Default.Send<DeleteOrderMessage>(new DeleteOrderMessage()
                    {
                        OrderId = order.Id
                    });
                }
                
            });

            this.ShowOrderDetailCmd = new RelayCommand(() =>
            {
                Core.Entities.Order order = this.OrderList.SingleOrDefault(o => o.Selected == true);

                Messenger.Default.Send<SelectOrderMessage>(new SelectOrderMessage()
                {
                    Order = order
                });

            });

            this.SelectMyOrderCmd = new RelayCommand<object>((parameter) =>
            {
                Windows.UI.Xaml.Controls.ItemClickEventArgs e = (Windows.UI.Xaml.Controls.ItemClickEventArgs)parameter;
                Core.Entities.Order SelectedOrder = (Core.Entities.Order)e.ClickedItem;

                foreach (Core.Entities.Order order in this.OrderList)
                {
                    order.Selected = false;
                }
                SelectedOrder.Selected = true;
            });

            Messenger.Default.Register<OrderDeletedMessage>(this, (msg) =>
            {
                var order = this.OrderList.Where(o => o.Id == msg.OrderId).SingleOrDefault();

                this.OrderList.Remove(order);

            });

            Messenger.Default.Register<OrderListloadedMessage>(this, (msg) =>
            {
                this.OrderList.Clear();
                foreach (Core.Entities.Order order in msg.orderList)
                {
                    this.OrderList.Add(order);
                }
            });


        }

        public void LoadMyOrders()
        {
            Messenger.Default.Send<LoadOrderListMessage>(new LoadOrderListMessage()
            {
            });
        }


    }
}
