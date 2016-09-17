using System;
using System.Collections.Generic;
using System.Text;

using TaxiApp.Core.Repository;
using TaxiApp.Core.Managers;
using TaxiApp.Core.ViewModel;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using GalaSoft.MvvmLight.Command;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight.Messaging;
using TaxiApp.Core.Messages;

namespace TaxiApp.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //if (ViewModelBase.IsInDesignModeStatic)
            //{
            //    SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            //}
            //else
            //{
            //    SimpleIoc.Default.Register<IDataService, DataService>();
            //}

            //SimpleIoc.Default.Register<Core.IMap, Core.UWP.Map>();

            SimpleIoc.Default.Register<TaxiApp.Core.WebApiClient>();
            SimpleIoc.Default.Register<UserRepository>();
            SimpleIoc.Default.Register<DriverRepository>();
            SimpleIoc.Default.Register<OrderRepository>();
            SimpleIoc.Default.Register<SystemManager>();
            SimpleIoc.Default.Register<MapPainter>();
            SimpleIoc.Default.Register<ILocationService, Core.UWP.Managers.LocationService>();
            SimpleIoc.Default.Register<LocationManager>();
            SimpleIoc.Default.Register<DataModel.LoginModel>(true);
            SimpleIoc.Default.Register<Core.DataModel.SearchModel>(true);
            SimpleIoc.Default.Register<Core.DataModel.Order.OrderModel>(true);

            SimpleIoc.Default.Register<AuthenticationViewModel>(() =>
            {
                return new AuthenticationViewModel(ServiceLocator.Current.GetInstance<INavigationService>("base"));
            });

            SimpleIoc.Default.Register<EditOrderViewModel>();
            SimpleIoc.Default.Register<MyOrderListViewModel>();

            SimpleIoc.Default.Register<MainViewModel>(() =>
            {
                return new MainViewModel(ServiceLocator.Current.GetInstance<IMenu>(),
                    ServiceLocator.Current.GetInstance<INavigationService>("base"),
                    ServiceLocator.Current.GetInstance<INavigationService>("split"));
            });

            SimpleIoc.Default.Register<MapViewModel>(() =>
            {
                ISuggestBox suggestBox = ServiceLocator.Current.GetInstance<ISuggestBox>();
                MapPainter painter = ServiceLocator.Current.GetInstance<MapPainter>();
                INavigationService navigationService = ServiceLocator.Current.GetInstance<INavigationService>("base");
                var vm = new MapViewModel(navigationService, suggestBox, painter);

                return vm;
            });

            SimpleIoc.Default.Register<ISuggestBox, TaxiApp.Core.UWP.Managers.SuggestBox>();
            SimpleIoc.Default.Register<IMap, TaxiApp.Core.UWP.Managers.Map>();
            SimpleIoc.Default.Register<IMenu, TaxiApp.Core.UWP.Managers.Menu>();
            SimpleIoc.Default.Register<IInitializationFrameStrategy>(() => { return new TaxiApp.Core.ViewModel.InitializationFrameStrategy(); }, "base");
            SimpleIoc.Default.Register<IInitializationFrameStrategy>(() => { return new TaxiApp.Core.UWP.ViewModel.InitializationFrameStrategy(); }, "split");
            SimpleIoc.Default.Register<INavigationService>(GetBaseNavigationService, "base");
            SimpleIoc.Default.Register<INavigationService>(GetSplitNavigationService, "split");
        }

        public ViewModelLocator()
        {
            Messenger.Default.Register<CleanupMessage>(this, (msg) =>
            {
                ViewModelLocator.Cleanup();

            });

        }



        private static INavigationService GetBaseNavigationService()
        {

            IInitializationFrameStrategy frameStrategy = ServiceLocator.Current.GetInstance<IInitializationFrameStrategy>("base");
            var navigationService = new TaxiApp.Core.ViewModel.NavigationService(frameStrategy);

            navigationService.Configure("Registration", typeof(Views.RegistrationPage));
            navigationService.Configure("Authentication", typeof(Views.AuthenticationPage));

            navigationService.Configure("AddPoint", typeof(Views.AddPointPage));
            navigationService.Configure("Main", typeof(Views.MainPage));

            return navigationService;
        }

        private static INavigationService GetSplitNavigationService()
        {

            IInitializationFrameStrategy frameStrategy = ServiceLocator.Current.GetInstance<IInitializationFrameStrategy>("split");
            var navigationService = new TaxiApp.Core.ViewModel.NavigationService(frameStrategy);

            navigationService.Configure("EditOrder", typeof(Views.EditOrderPage));
            navigationService.Configure("OrderList", typeof(Views.OrderListPage));

            return navigationService;
        }

        public AuthenticationViewModel AuthenticationViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AuthenticationViewModel>();
            }
        }

        public EditOrderViewModel EditOrderViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditOrderViewModel>();
            }
        }

        public MyOrderListViewModel MyOrderListViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MyOrderListViewModel>();
            }
        }

        public MapViewModel MapViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MapViewModel>();
            }
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            MapViewModel mapVM = ServiceLocator.Current.GetInstance<MapViewModel>();
            ISuggestBox suggestBox = ServiceLocator.Current.GetInstance<ISuggestBox>();
            IMap map = ServiceLocator.Current.GetInstance<IMap>();
            MapPainter painter = ServiceLocator.Current.GetInstance<MapPainter>();

            SimpleIoc.Default.Unregister<MapViewModel>(mapVM);
            SimpleIoc.Default.Unregister<ISuggestBox>(suggestBox);
            SimpleIoc.Default.Unregister<MapPainter>(painter);
            SimpleIoc.Default.Unregister<IMap>(map);
            
        }
    }
}
