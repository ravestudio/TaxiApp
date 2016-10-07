using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.Core.UWP.Controls
{
    public class ListPickerFlyout : Flyout
    {
        public ListPickerFlyout()
        {
            this.Content = new Grid()
            {
                Width = 100,
                Height = 200
            };
        }
    }
}
