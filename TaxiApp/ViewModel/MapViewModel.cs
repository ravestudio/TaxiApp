using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaxiApp.Core.DataModel;
using TaxiApp.Core.DataModel.Order;

namespace TaxiApp.ViewModel
{
    public class MapViewModel
    {
        public delegate void RouteChangeHandler();
        public RouteChangeHandler RouteChanged;

        public OrderModel OrderModel { get; set; }

        public Windows.UI.Xaml.Controls.Maps.MapControl RouteMapControl { get; set; }
        private Windows.Services.Maps.MapRoute mapRoute = null;

        public Windows.Services.Maps.MapRoute MapRoute
        {
            get
            {
                return this.mapRoute;
            }
            set
            {
                this.mapRoute = value;

                NotifyMapRouteChanged();
            }
        }

        public void NotifyMapRouteChanged()
        {
            if (RouteChanged != null)
            {
                RouteChanged();
            }
        }

        public MapViewModel()
        {
            this.OrderModel = TaxiApp.Core.DataModel.ModelFactory.Instance.GetOrderModel();

            this.RouteChanged = new RouteChangeHandler(() =>
            {
                this.OrderModel.ShowRoute(this.RouteMapControl, this.mapRoute);
            });
        }
    }
}
