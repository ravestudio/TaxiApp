using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.ViewModel.Command
{
    public class ShowMenuCommand : System.Windows.Input.ICommand
    {
        private EditOrderViewModel _viewModel = null;

        public ShowMenuCommand(EditOrderViewModel viewModel)
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
            _viewModel.ShowMenu();
        }
    }
}
