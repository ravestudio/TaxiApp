using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.ViewModel.Command
{
    public class CreateOrderCommand : System.Windows.Input.ICommand
    {

        //private EditOrderViewModel _viewOrderModel = null;

        //public CreateOrderCommand(EditOrderViewModel model)
        //{
        //    this._viewOrderModel = model;
        //}


        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            //TaxiApp.Core.Entities.Order order = _viewOrderModel.GetEntity();

            //_viewOrderModel.OrderModel.CreateOrder(order);

            ////_viewOrderModel.Pivot.SelectedIndex = 1;

            //_viewOrderModel.LoadMyOrders();
        }
    }
}
