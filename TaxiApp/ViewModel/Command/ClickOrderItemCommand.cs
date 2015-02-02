using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.ViewModel.Command
{
    public class ClickOrderItemCommand : System.Windows.Input.ICommand
    {
        private OrderViewModel _viewModel = null;

        public ClickOrderItemCommand(OrderViewModel model)
        {
            this._viewModel = model;
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

            _viewModel.Actions[orderItem.Cmd].Invoke(_viewModel, orderItem);
        }
    }
}
