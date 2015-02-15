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

        protected virtual string GetTitle()
        {
            return this._title;
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
