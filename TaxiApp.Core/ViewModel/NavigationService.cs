using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalaSoft.MvvmLight.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using Windows.UI.Xaml.Media;

namespace TaxiApp.Core.ViewModel
{
    public class NavigationService : INavigationService
    {

        private Dictionary<string, Type> pageList;

        private INavigationFrameStrategy _frameStrategy = null;

        public NavigationService(INavigationFrameStrategy frameStrategy)
        {
            this._frameStrategy = frameStrategy;
            this.pageList = new Dictionary<string, Type>();
        }

        public void Configure(string key, Type type)
        {
            this.pageList.Add(key, type);
        }

        public string CurrentPageKey
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void GoBack()
        {
            throw new NotImplementedException();
        }

        public void NavigateTo(string pageKey)
        {
            Frame navigationFrame = this._frameStrategy.GetFrame();

            if (navigationFrame == null)
            {
                navigationFrame = Window.Current.Content as Frame;
            }

            navigationFrame.Navigate(this.pageList[pageKey]);

        }


        public void NavigateTo(string pageKey, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
