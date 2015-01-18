using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaxiApp.Core.DataModel;
using TaxiApp.Core.DataModel.Order;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.Controller.Command
{
    public class SelectPointLocationCommand : System.Windows.Input.ICommand
    {
        private OrderController _controller = null;

        public SelectPointLocationCommand(OrderController controller)
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

            LocationItem location = (LocationItem)e.ClickedItem;

            _controller.SearchModel.SelectedPoint.Location = location;

            Frame rootFrame = Window.Current.Content as Frame;

            this._controller.OrderModel.UpdatePoints();

            rootFrame.GoBack();
        }
    }
}
