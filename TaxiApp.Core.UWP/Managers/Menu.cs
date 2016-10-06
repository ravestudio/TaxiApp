using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TaxiApp.Core.Managers;
using GalaSoft.MvvmLight.Command;

namespace TaxiApp.Core.UWP.Managers
{
    public class Menu : TaxiApp.Core.Managers.IMenu
    {
        private SplitView _splitmenu;

        private IList<IMenuItem> _itemList = null;

        public IList<IMenuItem> GetMenuItems()
        {
            if (_itemList == null)
            {
                Entities.IUser user = TaxiApp.Core.Session.Instance.GetUser();

                _itemList = new List<IMenuItem>();

                _itemList.Add(new MenuItemPersonal()
                {
                    Key = "personal",
                    Text = $"{user.Name} {user.Lastname}",
                    PhoneNumber = user.PhoneNumber
                });

                _itemList.Add(new MenuItem() { Key="home", Text = "Home" });
                _itemList.Add(new MenuItem() { Key="OrderList", Text = "My Orders" });
                _itemList.Add(new MenuItem() { Key="Setings", Text = "Settings" });
            }

            return _itemList;
        }

        public void Open()
        {

            if (this._splitmenu == null)
            {
                this._splitmenu = ChildFinder.FindChild<SplitView>(Window.Current.Content, "panel_splitter");
            }

            this._splitmenu.IsPaneOpen = !this._splitmenu.IsPaneOpen;
        }
    }

    public class MenuItem : TaxiApp.Core.Managers.IMenuItem
    {
        public string Key { get; set; }

        public string Text { get; set; }
    }

    public class MenuItemPersonal : MenuItem, IMenuItemPersonal
    {
        public string PhoneNumber { get; set; }
    }
}
