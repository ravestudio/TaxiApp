using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.ViewModel.Command
{
    public class ShowOrderDetailCommand : System.Windows.Input.ICommand
    {
        private EditOrderViewModel _viewModel = null;

        public ShowOrderDetailCommand(EditOrderViewModel viewModel)
        {
            this._viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            //Windows.UI.Xaml.Controls.ItemClickEventArgs e = (Windows.UI.Xaml.Controls.ItemClickEventArgs)parameter;

            //Core.Entities.Order order = (Core.Entities.Order)e.ClickedItem;

            TaxiApp.ViewModel.EditOrderViewModel editOrderViewModel = ViewModelFactory.Instance.GetViewOrderModel();

            Core.Entities.Order order = editOrderViewModel.OrderList.SingleOrDefault(o => o.Selected == true);

            //TaxiApp.Core.DataModel.ModelFactory.Instance.GetOrderModel().Detailed = order;

            Frame rootFrame = Window.Current.Content as Frame;


            rootFrame.Navigate(typeof(Views.OrderDetailPage));
        }
    }
}
