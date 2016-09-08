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
            SimpleIoc.Default.Register<SystemManager>();
            SimpleIoc.Default.Register<DataModel.LoginModel>(true);

            SimpleIoc.Default.Register<AuthenticationViewModel>(() => {
                return new AuthenticationViewModel(ServiceLocator.Current.GetInstance<INavigationService>("base"));
            });

            SimpleIoc.Default.Register<EditOrderViewModel>();

            SimpleIoc.Default.Register<MainViewModel>(() =>
            {
                return new MainViewModel(ServiceLocator.Current.GetInstance<IMenu>(),
                    ServiceLocator.Current.GetInstance<INavigationService>("base"),
                    ServiceLocator.Current.GetInstance<INavigationService>("split"));
            });

            SimpleIoc.Default.Register<MapViewModel>(() =>
            {
                return new MapViewModel();
            });

            SimpleIoc.Default.Register<IMenu, TaxiApp.Core.UWP.Managers.Menu>();
            SimpleIoc.Default.Register<IInitializationFrameStrategy>(() => { return new TaxiApp.Core.ViewModel.InitializationFrameStrategy(); }, "base");
            SimpleIoc.Default.Register<IInitializationFrameStrategy>(() => { return new TaxiApp.Core.UWP.ViewModel.InitializationFrameStrategy(); }, "split");
            SimpleIoc.Default.Register<INavigationService>(GetBaseNavigationService, "base");
            SimpleIoc.Default.Register<INavigationService>(GetSplitNavigationService, "split");

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
        }
    }
}
