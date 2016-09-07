using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace TaxiApp.Core.ViewModel
{
    public class MainViewModel
    {

        public RelayCommand ClickMenuCmd { get; set; }
        public RelayCommand ContextChangedCmd { get; set; }


        private TaxiApp.Core.Managers.IMenu _menu = null;
        private INavigationService _appNavigationServie = null;
        private INavigationService _childNavigationServie = null;

        public MainViewModel(TaxiApp.Core.Managers.IMenu menu, INavigationService appNavigationService, INavigationService childNavigationService)
        {
            this._menu = menu;
            this._appNavigationServie = appNavigationService;
            this._childNavigationServie = childNavigationService;

            this.ClickMenuCmd = new RelayCommand(() =>
            {
                this._menu.Open();
            });

            this.ContextChangedCmd = new RelayCommand(() =>
            {
                this._childNavigationServie.NavigateTo("EditOrder");
            });

            

        }
    }
}
