using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.ViewModel.Command
{
    public class SelectServicesCommand : System.Windows.Input.ICommand
    {
        private EditOrderViewModel _viewModel = null;

        public SelectServicesCommand(EditOrderViewModel model)
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
            this._viewModel.SelectedServices =
                this._viewModel.ServicePicker.SelectedItems.Cast<Core.DataModel.Order.OrderOption>().ToList();
        }
    }
}
