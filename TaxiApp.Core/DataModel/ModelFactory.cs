using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.DataModel
{
    public class ModelFactory : Singleton<ModelFactory>
    {
        private Order.OrderModel orderModel = null;
        private SearchModel searchModel = null;

        public Order.OrderModel GetOrderModel()
        {
            if (this.orderModel == null)
            {
                this.orderModel = new Order.OrderModel();
            }

            return this.orderModel;
        }

        public SearchModel GetSearchModel()
        {
            if (this.searchModel == null)
            {
                this.searchModel = new SearchModel();
            }

            return this.searchModel;
        }
    }
}
