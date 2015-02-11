using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.ViewModel.Command
{
    public class CancelOrderCommand : System.Windows.Input.ICommand
    {
        private EditOrderViewModel _viewOrderModel = null;

        public CancelOrderCommand(EditOrderViewModel model)
        {
            this._viewOrderModel = model;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            TaxiApp.Core.Entities.Order order = (TaxiApp.Core.Entities.Order)parameter;

            var dlg = new Windows.UI.Popups.MessageDialog("Отменить заказ?");

            dlg.Commands.Add(new Windows.UI.Popups.UICommand("Accept"));
            dlg.Commands.Add(new Windows.UI.Popups.UICommand("Cancel"));

            Task<Windows.UI.Popups.IUICommand> dlgTask = dlg.ShowAsync().AsTask();

            dlgTask.ContinueWith((dialogResult) =>
                {
                    if (dialogResult.Result.Label == "Accept")
                    {
                        Delete(order.Id);
                    }
                });


        }

        private void Delete(int id)
        {
            TaxiApp.Core.WebApiClient client = new TaxiApp.Core.WebApiClient();

            TaxiApp.Core.Repository.OrderRepository orderRepository = new Core.Repository.OrderRepository(client);

            Task<bool> deleteTask = orderRepository.DeleteOrder(id);

            deleteTask.ContinueWith((res) =>
            {
                if (res.Result)
                {
                    RemoveOrder(id);
                }
            });
        }

        private void RemoveOrder(int id)
        {
            var order = _viewOrderModel.OrderList.Where(o => o.Id == id).SingleOrDefault();

            this._viewOrderModel.OrderModel.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                _viewOrderModel.OrderList.Remove(order);
            });
        }
    }
}
