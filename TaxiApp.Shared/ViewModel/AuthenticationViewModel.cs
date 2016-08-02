﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.ViewModel
{
    public class AuthenticationViewModel : ViewModel
    {
        public DataModel.LoginModel LoginModel { get; set; }

        public LoginCommand LoginCmd { get; set; }
        public RegisterCommand RegisterCmd { get; set; }

        public Windows.UI.Xaml.Controls.Page Page { get; set; }

        public AuthenticationViewModel()
        {
            this.LoginModel = new DataModel.LoginModel();
            this.LoginCmd = new LoginCommand(this);
            this.RegisterCmd = new RegisterCommand(this);
        }

    }

    public class RegisterCommand : System.Windows.Input.ICommand
    {
        private AuthenticationViewModel _controller = null;

        public RegisterCommand(AuthenticationViewModel controller)
        {
            this._controller = controller;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            TaxiApp.Core.WebApiClient client = new TaxiApp.Core.WebApiClient();

            TaxiApp.Core.Repository.UserRepository userRepository = new Core.Repository.UserRepository(client);

            DataModel.LoginModel model = _controller.LoginModel;

            userRepository.RegisterUser(model.PhoneNumber).ContinueWith(t =>
                {
                    model.SaveNumber();

                    string res = t.Result;

                    Windows.Foundation.IAsyncAction action =
                    this._controller.Page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        int thread = Environment.CurrentManagedThreadId;

                        Frame frame = _controller.Page.Frame;

                        frame.Navigate(typeof(Views.AuthenticationPage));
                    });

                });
        }
    }

    public class LoginCommand : System.Windows.Input.ICommand
    {
        private AuthenticationViewModel _controller = null;

        public LoginCommand(AuthenticationViewModel controller)
        {
            this._controller = controller;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public async void Execute(object parameter)
        {
            TaxiApp.Core.WebApiClient client = new TaxiApp.Core.WebApiClient();

            TaxiApp.Core.Repository.UserRepository userRepository = new Core.Repository.UserRepository(client);

            DataModel.LoginModel model = _controller.LoginModel;

            int thread = Environment.CurrentManagedThreadId;

            string deviceId = TaxiApp.Core.Managers.ManagerFactory.Instance.GetSystemManager().GetDeviceId();

            var task = userRepository.GetUser(model.PhoneNumber, model.PIN, deviceId);

            try
            {
                TaxiApp.Core.Session.Instance.SetUSer(await task);

                model.SavePIN();

                Frame frame = _controller.Page.Frame;
                frame.Navigate(typeof(Views.EditUserProfilePage));
            }
            catch(Exception ex)
            {
                string msg = ex.Message;

                model.ClearData();

                Frame frame = _controller.Page.Frame;
                frame.Navigate(typeof(Views.RegistrationPage));
            }


            //task.ContinueWith(t => {

            //    thread = Environment.CurrentManagedThreadId;

            //    string msg = "err";


            //}, TaskContinuationOptions.OnlyOnFaulted);

            //task.ContinueWith(t =>
            //    {
            //        model.SaveData();

            //        TaxiApp.Core.Session.Instance.SetUSer(t.Result);

            //        Windows.Foundation.IAsyncAction action =
            //        this._controller.Page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //        {
            //            thread = Environment.CurrentManagedThreadId;

            //            Frame frame = _controller.Page.Frame;

            //            frame.Navigate(typeof(Views.EditUserProfilePage));
            //        });
            //    }, TaskContinuationOptions.OnlyOnRanToCompletion);
            
        }
    }
}