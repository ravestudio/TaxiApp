using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using GalaSoft.MvvmLight.Messaging;
using TaxiApp.Core.Messages;
using TaxiApp.Core.Managers;

namespace TaxiApp.Core.ViewModel
{
    public class MainViewModel
    {

        public RelayCommand ClickMenuCmd { get; set; }
        public RelayCommand ContextChangedCmd { get; set; }

        public RelayCommand<object> MenuSelectionChanged { get; set; }

        private TaxiApp.Core.Managers.IMenu _menu = null;
        private INavigationService _appNavigationServie = null;
        private INavigationService _childNavigationServie = null;

        public string PersonName { get; set; }
        public string PersonePhone { get; set; }

        public IList<IMenuItem> MenuItems { get { return this._menu.GetMenuItems(); } }
        private IDictionary<string, Action> _menuActions = null;

        public MainViewModel(TaxiApp.Core.Managers.IMenu menu, INavigationService appNavigationService, INavigationService childNavigationService)
        {
            this._menu = menu;
            this._appNavigationServie = appNavigationService;
            this._childNavigationServie = childNavigationService;

            _menuActions = new Dictionary<string, Action>();
            _menuActions.Add("home", () =>
            {
                this._childNavigationServie.NavigateTo("EditOrder");
            });

            _menuActions.Add("OrderList", () =>
            {
                this._childNavigationServie.NavigateTo("OrderList");
            });

            _menuActions.Add("personal", () =>
            {
                this._childNavigationServie.NavigateTo("EditProfile");

            });

            this.ClickMenuCmd = new RelayCommand(() =>
            {
                this._menu.Open();
            });
            

            this.MenuSelectionChanged = new RelayCommand<object>((obj) =>
            {
                var menu_item = (IMenuItem)obj;

                _menuActions[menu_item.Key].Invoke();
            });

            this.ContextChangedCmd = new RelayCommand(() =>
            {
                this._childNavigationServie.NavigateTo("EditOrder");
            });

            Messenger.Default.Register<FillRoutePointMessage>(this, (msg) => {

                this._appNavigationServie.NavigateTo("AddPoint", msg.Point);

            });

        }

    }
}
