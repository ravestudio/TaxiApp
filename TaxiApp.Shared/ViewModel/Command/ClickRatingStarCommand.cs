using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.ViewModel.Command
{
    public class ClickRatingStarCommand : System.Windows.Input.ICommand
    {
        private SendRatingViewModel _viewModel = null;

        public ClickRatingStarCommand(SendRatingViewModel model)
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

            RatingStar star = (RatingStar)e.ClickedItem;

            if (star.Rating == "CAR")
            {
                foreach(RatingStar s in _viewModel.CarRating)
                {
                    s.Activate();
                }

                _viewModel.RaisePropertyChanged("CarRating");
            }

            if (star.Rating == "DRIVER")
            {
                foreach (RatingStar s in _viewModel.DriverRating)
                {
                    s.Activate();
                }

                _viewModel.RaisePropertyChanged("DriverRating");
            }

        }
    }
}
