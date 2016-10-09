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
            StackPanel panel = new StackPanel()
            {
                HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch,
                VerticalAlignment = Windows.UI.Xaml.VerticalAlignment.Stretch
            };

            //panel.ch
        }
    }
}
