using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.ViewModel
{
    public class ViewModelFactory : TaxiApp.Core.Singleton<ViewModelFactory>
    {
        private OrderViewModel viewOrderModel = null;
        private AuthenticationViewModel viewAuthenticationModel = null;

        public OrderViewModel GetViewOrderModel()
        {
            if (this.viewOrderModel == null)
            {
                this.viewOrderModel = new OrderViewModel();
            }

            return this.viewOrderModel;
        }

        public AuthenticationViewModel GetViewAuthenticationModel()
        {
            if (this.viewAuthenticationModel == null)
            {
                this.viewAuthenticationModel = new AuthenticationViewModel();
            }

            return this.viewAuthenticationModel;
        }
    }
}
