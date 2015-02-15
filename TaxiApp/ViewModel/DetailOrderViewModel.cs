using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaxiApp.Core.DataModel;
using TaxiApp.Core.DataModel.Order;

namespace TaxiApp.ViewModel
{
    public class DetailOrderViewModel : ViewModel
    {
        public OrderModel OrderModel { get; set; }

        public MapViewModel Map { get; set; }

        private TaxiApp.Core.Entities.Order _order = null;
        private TaxiApp.Core.Entities.Driver _driver = null;

        public TaxiApp.Core.Entities.Driver DriverInfo
        {
            get
            {
                return this._driver;
            }

            set
            {
                this._driver = value;
                NotifyPropertyChanged("DriverInfo");
            }
        }

        public DetailOrderViewModel()
        {
            this.Map = new MapViewModel();
            this.OrderModel = TaxiApp.Core.DataModel.ModelFactory.Instance.GetOrderModel();

        }

        public override void Init(Windows.UI.Xaml.Controls.Page Page)
        {

            base.Init(Page);

            this.DriverInfo = null;

            this._order = this.OrderModel.Detailed;

            if (_order.DriverId > 0)
            {
                Task<TaxiApp.Core.Entities.Driver> task = this.OrderModel.GetDriver(_order.DriverId);

                task.ContinueWith((t) =>
                    {
                        this.DriverInfo = t.Result;
                    });
            }

        }
    }
}
