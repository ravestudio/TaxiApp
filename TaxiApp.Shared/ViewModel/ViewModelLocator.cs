using System;
using System.Collections.Generic;
using System.Text;

using TaxiApp.Core.Repository;
using TaxiApp.Core.Managers;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
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
            
        }

        public AuthenticationViewModel AuthenticationViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AuthenticationViewModel>();
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
