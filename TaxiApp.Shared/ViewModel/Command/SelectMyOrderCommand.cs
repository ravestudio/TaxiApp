using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.ViewModel.Command
{
    public class SelectMyOrderCommand : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            //Windows.UI.Xaml.Controls.ItemClickEventArgs e = (Windows.UI.Xaml.Controls.ItemClickEventArgs)parameter;

            //Core.Entities.Order SelectedOrder = (Core.Entities.Order)e.ClickedItem;

            //TaxiApp.ViewModel.EditOrderViewModel editOrderViewModel = ViewModelFactory.Instance.GetViewOrderModel();
            //foreach(Core.Entities.Order order in editOrderViewModel.OrderList)
            //{
            //    order.Selected = false;
            //}

            //SelectedOrder.Selected = true;

        }
    }
}
