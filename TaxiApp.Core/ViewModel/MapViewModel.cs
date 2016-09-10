using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TaxiApp.Core.Common;
using TaxiApp.Core.DataModel;
using TaxiApp.Core.DataModel.Order;
using TaxiApp.Core.Managers;
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

        private ISuggestBox _suggestBox = null;
        private Managers.MapPainter _painter = null;

        public RelayCommand<SuggestTextChangedArgs> SuggestTextChangedCmd { get; set; }
        private RelayCommand<string> searchCmd { get; set; }

        public RelayCommand ContextChangedCmd { get; set; }

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

        public MapViewModel(ISuggestBox suggestBox, Managers.MapPainter painter)
        {
            this._suggestBox = suggestBox;
            this._painter = painter;
            

            this.RouteChanged = new RouteChangeHandler(() =>
            {
                //Windows.Foundation.IAsyncAction action =
                //RouteMapControl.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                //{

                //    this.OrderModel.ShowRoute(this.mapRoute);
                //});

                this.OrderModel.ShowRoute(this.mapRoute);

            });

            Messenger.Default.Register<FoundLocationsMessage>(this, (msg) => {

                this._suggestBox.SetListItems(msg.LocationItems);

                this._suggestBox.Open();
            });

            this.ContextChangedCmd = new RelayCommand(() =>
            {
                _painter.ShowMyPossitionAsync();
            });

            this.SuggestTextChangedCmd = new RelayCommand<SuggestTextChangedArgs>((args) =>
            {
                if (args.ByUser)
                {
                    searchCmd.Execute(SearchText);
                }
            });

            this.searchCmd = new RelayCommand<string>((text) => {

                this._suggestBox.ClearListItems();

                Messenger.Default.Send<SearchLocationMessage>(new SearchLocationMessage()
                {
                    Text = text
                });
            }, (text) => { return (text.Length > 5); });

        }

    }
}
