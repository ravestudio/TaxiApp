using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.ViewModel
{
    public class ViewModelFactory : TaxiApp.Core.Singleton<ViewModelFactory>
    {
        private EditOrderViewModel viewOrderModel = null;
        private AuthenticationViewModel viewAuthenticationModel = null;
        private DetailOrderViewModel detailOrderViewModel = null;
        private SendRatingViewModel sendRatingViewModel = null;
        private EditUserProfileViewModel editUserProfileViewModel = null;

        public EditOrderViewModel GetViewOrderModel()
        {
            //if (this.viewOrderModel == null)
            //{
            //    this.viewOrderModel = new EditOrderViewModel();
            //}

            return this.viewOrderModel;
        }

        public DetailOrderViewModel GetDetailOrderViewModel()
        {
            if (this.detailOrderViewModel == null)
            {
                this.detailOrderViewModel = new DetailOrderViewModel();
            }

            return this.detailOrderViewModel;
        }

        public AuthenticationViewModel GetViewAuthenticationModel()
        {
            if (this.viewAuthenticationModel == null)
            {
                this.viewAuthenticationModel = new AuthenticationViewModel();
            }

            return this.viewAuthenticationModel;
        }

        public SendRatingViewModel GetSendRatingViewModel()
        {
            if (this.sendRatingViewModel == null)
            {
                this.sendRatingViewModel = new SendRatingViewModel();
            }

            return this.sendRatingViewModel;
        }

        public EditUserProfileViewModel GetEditUserProfileViewModel()
        {
            if (this.editUserProfileViewModel == null)
            {
                this.editUserProfileViewModel = new EditUserProfileViewModel();
            }

            return this.editUserProfileViewModel;
        }
    }
}
