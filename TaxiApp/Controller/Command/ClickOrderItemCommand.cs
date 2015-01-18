using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Controller.Command
{
    public class ClickOrderItemCommand : System.Windows.Input.ICommand
    {
        private OrderController _controller = null;

        public ClickOrderItemCommand(OrderController controller)
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
            Windows.UI.Xaml.Controls.ItemClickEventArgs e = (Windows.UI.Xaml.Controls.ItemClickEventArgs)parameter;

            TaxiApp.Core.DataModel.Order.OrderItem orderItem = (TaxiApp.Core.DataModel.Order.OrderItem)e.ClickedItem;

            _controller.Actions[orderItem.Cmd].Invoke(_controller, orderItem);
        }
    }
}
