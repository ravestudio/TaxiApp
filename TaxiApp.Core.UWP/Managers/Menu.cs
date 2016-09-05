using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using TaxiApp.Core.Managers;

namespace TaxiApp.Core.UWP.Managers
{
    public class Menu : TaxiApp.Core.Managers.IMenu
    {
        private SplitView _splitmenu;

        public void Open()
        {

            if (this._splitmenu == null)
            {
                this._splitmenu = ChildFinder.FindChild<SplitView>(Window.Current.Content, "panel_splitter");
            }

            this._splitmenu.IsPaneOpen = !this._splitmenu.IsPaneOpen;
        }
    }
}
