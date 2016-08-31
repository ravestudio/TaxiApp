using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaxiApp.Core.ViewModel;
using TaxiApp.Core.DataModel;
using TaxiApp.Core.DataModel.Order;

namespace TaxiApp.ViewModel
{
    public class DetailOrderViewModel : TaxiViewModel
    {
        public OrderModel OrderModel { get; set; }

        public MapViewModel Map { get; set; }

        private IList<OrderOption> _services = null;

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

        public IList<OrderOption> Services
        {
            get
            {
                return this._services;
            }
        }

        private string _carClass = string.Empty;
        public string CarClass
        {
            get
            {
                return _carClass;
            }

            set
            {
                this._carClass = value;
                NotifyPropertyChanged("CarClass");
            }
        }

        public TaxiApp.Core.Entities.Order Order
        {
            get
            {
                return this._order;
            }
        }


        public DetailOrderViewModel()
        {
            this.Map = new MapViewModel();

            this._services = new List<OrderOption>();

        }

        public override void Init(Windows.UI.Xaml.Controls.Page Page)
        {

            base.Init(Page);

            this.DriverInfo = null;

            this._order = this.OrderModel.Detailed;

            this.FillServices(_order.Servieces);

            this.CarClass = this.OrderModel.OrderCarList.Single(c => c.id == this._order.Carclass).Name;


            if (_order.DriverId > 0)
            {
                Task<TaxiApp.Core.Entities.Driver> task = this.OrderModel.GetDriver(_order.DriverId);

                task.ContinueWith((t) =>
                    {
                        this.DriverInfo = t.Result;
                    });
            }

        }

        private void FillServices(byte services)
        {
            this._services.Clear();

            foreach(OrderOption srv in this.OrderModel.OrderServiceList)
            {
                int v = services & (byte)srv.id;
                if (v == srv.id)
                {
                    this._services.Add(srv);
                }
            }

            NotifyPropertyChanged("Services");
        }
    }
}
