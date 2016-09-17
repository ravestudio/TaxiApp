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
                        Messenger.Default.Send<DeleteOrderMessage>(new DeleteOrderMessage()
                        {
                            OrderId = order.Id
                        });
                    }
                });
            });

            this.ShowOrderDetailCmd = new RelayCommand(() =>
            {
                Core.Entities.Order order = this.OrderList.SingleOrDefault(o => o.Selected == true);

                Messenger.Default.Send<SelectOrderMessage>(new SelectOrderMessage()
                {
                    Order = order
                });

                //Frame rootFrame = Window.Current.Content as Frame;
                //rootFrame.Navigate(typeof(Views.OrderDetailPage));
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

            Messenger.Default.Register<OrderDeletedMessage>(this, (msg) => {
                var order = this.OrderList.Where(o => o.Id == msg.OrderId).SingleOrDefault();
                this.Page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    this.OrderList.Remove(order);
                });
            });


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
