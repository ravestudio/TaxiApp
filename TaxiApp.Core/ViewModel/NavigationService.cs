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

        public NavigationService()
        {
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
            Frame navigationFrame = GetDescendantFromName(Window.Current.Content, "mainFrame") as Frame;

            if (navigationFrame == null)
            {
                navigationFrame = Window.Current.Content as Frame;
            }

            navigationFrame.Navigate(this.pageList[pageKey]);
        }

        private static FrameworkElement GetDescendantFromName(DependencyObject parent, string name)
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);

            if (count < 1)
            {
                return null;
            }

            for (var i = 0; i < count; i++)
            {
                var frameworkElement = VisualTreeHelper.GetChild(parent, i) as FrameworkElement;
                if (frameworkElement != null)
                {
                    if (frameworkElement.Name == name)
                    {
                        return frameworkElement;
                    }

                    frameworkElement = GetDescendantFromName(frameworkElement, name);
                    if (frameworkElement != null)
                    {
                        return frameworkElement;
                    }
                }
            }

            return null;
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
