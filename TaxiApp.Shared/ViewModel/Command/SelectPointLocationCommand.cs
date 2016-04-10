using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaxiApp.Core.DataModel;
using TaxiApp.Core.DataModel.Order;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.ViewModel.Command
{
    public class SelectPointLocationCommand : System.Windows.Input.ICommand
    {
        private EditOrderViewModel _viewModel = null;

        public SelectPointLocationCommand(EditOrderViewModel model)
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

            LocationItem location = (LocationItem)e.ClickedItem;

            _viewModel.SearchModel.SelectedPoint.Location = location;

            Frame rootFrame = Window.Current.Content as Frame;

            this._viewModel.UpdatePoints();

            rootFrame.GoBack();
        }
    }
}
