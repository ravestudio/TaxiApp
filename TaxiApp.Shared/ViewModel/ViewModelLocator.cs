﻿using System;
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

            SimpleIoc.Default.Register<AuthenticationViewModel>();
            SimpleIoc.Default.Register<EditOrderViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();

            SimpleIoc.Default.Register<IMenu, TaxiApp.Core.UWP.Managers.Menu>();
            SimpleIoc.Default.Register<INavigationFrameStrategy, TaxiApp.Core.UWP.ViewModel.NavigationFrameStrategy>();
            SimpleIoc.Default.Register<INavigationService>(GetNavigationService);            

        }

        private static INavigationService GetNavigationService()
        {

            INavigationFrameStrategy frameStrategy = ServiceLocator.Current.GetInstance<INavigationFrameStrategy>();
            var navigationService = new TaxiApp.Core.ViewModel.NavigationService(frameStrategy);

            navigationService.Configure("Registration", typeof(Views.RegistrationPage));
            navigationService.Configure("Authentication", typeof(Views.AuthenticationPage));
            navigationService.Configure("EditOrder", typeof(Views.EditOrderPage));
            navigationService.Configure("Main", typeof(Views.MainPage));

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
