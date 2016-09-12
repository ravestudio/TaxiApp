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

        private IInitializationFrameStrategy _frameStrategy = null;

        public NavigationService(IInitializationFrameStrategy frameStrategy)
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
            NavigateTo(pageKey, null);
        }


        public void NavigateTo(string pageKey, object parameter)
        {
            Frame navigationFrame = null;

            navigationFrame = this._frameStrategy.GetFrame();

            navigationFrame.Navigate(this.pageList[pageKey], parameter);
        }
    }
}
