using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.ViewModel
{
    public class ViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public Windows.UI.Xaml.Controls.Page Page { get; set; }

        public virtual void Init(Windows.UI.Xaml.Controls.Page Page)
        {
            this.Page = Page;
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                Windows.Foundation.IAsyncAction action =
                this.Page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                });

            }
        }
    }
}
