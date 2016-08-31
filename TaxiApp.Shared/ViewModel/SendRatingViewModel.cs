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
    public class SendRatingViewModel : TaxiViewModel
    {
        public OrderModel OrderModel { get; set; }

        private TaxiApp.Core.Entities.Order _order = null;
        private TaxiApp.Core.Entities.Driver _driver = null;

        private IList<RatingStar> _carRating = null;
        private IList<RatingStar> _driverRating = null;

        public Command.ClickRatingStarCommand ClickStarItem { get; set; }

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

        public IList<RatingStar> CarRating
        {
            get
            {
                return this._carRating;
            }
        }

        public IList<RatingStar> DriverRating
        {
            get
            {
                return this._driverRating;
            }
        }

        public TaxiApp.Core.Entities.Order Order
        {
            get
            {
                return this._order;
            }
        }

        public SendRatingViewModel()
        {
            this.ClickStarItem = new Command.ClickRatingStarCommand(this);

            this._carRating = new List<RatingStar>();
            this._driverRating = new List<RatingStar>();

            for(byte i = 1; i <= 5; i++)
            {
                this._carRating.Add(new RatingStar() { id = i, Rating = "CAR" });
                this._driverRating.Add(new RatingStar() { id = i, Rating = "DRIVER" });
            }
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

    public class RatingStar
    {
        public byte id { get; set; }


        private bool Active = false;

        private Windows.UI.Xaml.Media.ImageSource _IconActive = null;
        private Windows.UI.Xaml.Media.ImageSource _IconInactive = null;

        public string Rating { get; set; }


        public Windows.UI.Xaml.Media.ImageSource IconSource
        {
            get
            {
                return Active ? _IconActive : _IconInactive;
            }
        }

        public RatingStar()
        {
            this._IconActive = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/rating1.png"));
            this._IconInactive = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri("ms-appx:///Assets/rating2.png"));
        }

        public void Activate()
        {
            this.Active = true;
        }

        public void Deactivate()
        {
            this.Active = false;
        }

    }
}
