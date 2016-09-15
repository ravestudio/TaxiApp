using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
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
    public class MapViewModel : ViewModelBase
    {
        public delegate void RouteChangeHandler();
        public RouteChangeHandler RouteChanged;

        public OrderModel OrderModel { get; set; }

        private TaxiApp.Core.IRoute mapRoute = null;

        private ISuggestBox _suggestBox = null;
        private Managers.MapPainter _painter = null;

        public RelayCommand<SuggestTextChangedArgs> SuggestTextChangedCmd { get; set; }
        private RelayCommand<string> searchCmd { get; set; }
        public RelayCommand<SuggestionChosenArgs> SelectLocationItem { get; set; }
        public RelayCommand confirmCmd { get; set; }

        public string SearchText { get; set; }

        private OrderPoint _currentPoint = null;

        private LocationItem _templocationItem = null;

        private INavigationService _appNavigationServie = null;



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

        public MapViewModel(INavigationService appNavigationService, ISuggestBox suggestBox, Managers.MapPainter painter)
        {
            this._appNavigationServie = appNavigationService;

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

            Messenger.Default.Register<RouteChangedMessage>(this, (msg) => {

                if (msg.route != null)
                {
                    this._painter.ShowRoute(msg.route);
                }
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

            this.confirmCmd = new RelayCommand(() =>
            {
                this._currentPoint.Location = this._templocationItem;
                this._appNavigationServie.GoBack();
            });

            this.SelectLocationItem = new RelayCommand<SuggestionChosenArgs>((parameter) =>
            {
                LocationItem locationItem = parameter.Selectedlocation;

                Messenger.Default.Send<SelectLocationMessage>(new SelectLocationMessage()
                {
                    Priority = this._currentPoint.Priority,
                    LocationItem = locationItem
                });

                this._templocationItem = locationItem;
                this._painter.ShowMarker(locationItem.Location);
                //Frame rootFrame = Window.Current.Content as Frame;

                //this.UpdatePoints();
                //rootFrame.GoBack();
            });

        }

        public void SetOrderPoint(OrderPoint point)
        {
            this._currentPoint = point;
        }

        public void Refresh()
        {
            _painter.ShowMyPossitionAsync();
        }

        public override void Cleanup()
        {
            base.Cleanup();

            Messenger.Default.Send<CleanupMessage>(new CleanupMessage()
            {
                 view = 1
            });

            //MapViewModel mapVM = ServiceLocator.Current.GetInstance<MapViewModel>();
            //SimpleIoc.Default.Unregister<MapViewModel>(mapVM);
        }

    }
}
