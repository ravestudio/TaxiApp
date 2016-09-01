using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.UWP.Managers
{
    public class Menu : TaxiApp.Core.Managers.IMenu
    {
        private Windows.UI.Xaml.Controls.SplitView _splitmenu;

        public void Init(Windows.UI.Xaml.Controls.SplitView splitmenu)
        {
            this._splitmenu = splitmenu;
        }

        public void Open()
        {
            this._splitmenu.IsPaneOpen = !this._splitmenu.IsPaneOpen;
        }
    }
}
