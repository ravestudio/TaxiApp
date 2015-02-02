using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TaxiApp.Core.DataModel.Order
{
    public class OrderPriceInfo: INotifyPropertyChanged
    {

        private bool _visible;
        private string _price;

        public bool Visible
        {
            get
            {
                return this._visible;
            }
            set
            {
                this._visible = value;
                NotifyPropertyChanged("Visible");
            }
        }
        public string Price
        {
            get
            {
                return this._price;
            }
            set
            {
                this._price = value;
                NotifyPropertyChanged("Price");
            }
        }

        private string _destination;
        public string Destination { get
        {
            return _destination;
        }
            set
            {
                this._destination = value;
                NotifyPropertyChanged("Destination");
            }
        }

        private string _time;
        public string Time
        {
            get
            {
                return this._time;
            }
            set
            {
                this._time = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
