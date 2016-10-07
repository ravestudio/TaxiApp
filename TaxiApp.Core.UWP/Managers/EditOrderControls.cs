using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.Managers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.Core.UWP.Managers
{
    public class EditOrderControls : IEditOrderControls
    {
        private Flyout _servicePicker = null;
        private Page _page = null;

        public void OpenServicePicker()
        {
            this.GetServicePicker().ShowAt(_page);
        }

        private Flyout GetServicePicker()
        {
            if (_servicePicker == null)
            {
                SplitView split = TaxiApp.Core.Managers.ChildFinder.FindChild<SplitView>(Window.Current.Content, "panel_splitter");
                var frame = (Frame)split.Content;

                //var frame = (Frame)Window.Current.Content;
                _page = (Page)frame.Content;

                _servicePicker = (Flyout)_page.Resources["ServiceFlyout"];
            }

            return _servicePicker;
        }
    }
}
