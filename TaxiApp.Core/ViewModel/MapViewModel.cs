using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaxiApp.Core.DataModel;
using TaxiApp.Core.DataModel.Order;
using TaxiApp.Core.Messages;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.Core.ViewModel
{
    public class MapViewModel
    {
        public delegate void RouteChangeHandler();
        public RouteChangeHandler RouteChanged;

        public OrderModel OrderModel { get; set; }

        private TaxiApp.Core.IRoute mapRoute = null;

        public ICommand SuggestTextChangedCmd { get; set; }
        private RelayCommand<string> searchCmd { get; set; }

        public string SearchText { get; set; }

        public TaxiApp.Core.IRoute MapRoute
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

            this.RouteChanged = new RouteChangeHandler(() =>
            {
                //Windows.Foundation.IAsyncAction action =
                //RouteMapControl.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                //{

                //    this.OrderModel.ShowRoute(this.mapRoute);
                //});

                this.OrderModel.ShowRoute(this.mapRoute);

            });

            this.searchCmd = new RelayCommand<string>((text) => {
                Messenger.Default.Send<SearchLocationMessage>(new SearchLocationMessage()
                {
                    Text = text
                });
            }, (text) => { return (text.Length > 5); });

        }

        public void SetTextChangedCmd(ICommand cmd)
        {
            this.SuggestTextChangedCmd = cmd;
        }

        public void ExecuteQuery()
        {
            searchCmd.Execute(SearchText);

        }
    }
}
