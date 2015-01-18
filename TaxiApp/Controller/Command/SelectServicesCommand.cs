using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Controller.Command
{
    public class SelectServicesCommand : System.Windows.Input.ICommand
    {
        private OrderController _controller = null;

        public SelectServicesCommand(OrderController controller)
        {
            this._controller = controller;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this._controller.OrderModel.SelectedServices =
                this._controller.ServicePicker.SelectedItems.Cast<Core.DataModel.Order.OrderOption>().ToList();
        }
    }
}
