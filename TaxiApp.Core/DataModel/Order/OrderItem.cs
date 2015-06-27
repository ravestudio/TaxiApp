using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.DataModel.Order
{
    public class OrderItem : System.ComponentModel.INotifyPropertyChanged
    {
        public int Priority { get; set; }
        public string Cmd { get; set; }
        public Windows.UI.Xaml.Media.ImageSource IconSource { get; set; }

        protected string _title = string.Empty;
        protected string _subtitle = string.Empty;

        public string Title
        {
            get
            {
                return this.GetTitle();
            }

            set
            {
                this._title = value;
                NotifyPropertyChanged("Title");
            }
        }

        public string Subtitle
        {
            get
            {
                return this.GetSubtitle();
            }

            set
            {
                this._subtitle = value;
                NotifyPropertyChanged("Subtitle");
                NotifyPropertyChanged("SubheaderVisible");
            }
        }

        public Windows.UI.Xaml.Visibility SubheaderVisible
        {
            get
            {
                return string.IsNullOrEmpty(this.GetSubtitle()) ? Windows.UI.Xaml.Visibility.Collapsed : Windows.UI.Xaml.Visibility.Visible;
            }
        }
        

        protected virtual string GetTitle()
        {
            return this._title;
        }

        protected virtual string GetSubtitle()
        {
            return this._subtitle;
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
