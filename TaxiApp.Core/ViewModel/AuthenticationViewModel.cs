﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.Messages;

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.Core.ViewModel
{
    public class AuthenticationViewModel : TaxiViewModel
    {
        public string PhoneNumber { get; set; }
        public string PIN { get; set; }

        public RelayCommand LoginCmd { get; set; }
        public RelayCommand RegisterCmd { get; set; }

        private IDictionary<MessageStatus, Action> registrationActions = null;
        private IDictionary<MessageStatus, Action> autorizationActions = null;

        private INavigationService _navigationService = null;

        public AuthenticationViewModel(INavigationService NavigationService)
        {
            this._navigationService = NavigationService;

            this.registrationActions = new Dictionary<MessageStatus, Action>();
            this.autorizationActions = new Dictionary<MessageStatus, Action>();

            this.registrationActions.Add(MessageStatus.Success, () =>
            {
                this._navigationService.NavigateTo("Authentication");
                
            });

            this.registrationActions.Add(MessageStatus.Faulted, () =>
            {
                var dlg = new Windows.UI.Popups.MessageDialog("Ошибка регистрации");
                dlg.ShowAsync();
            });

            this.autorizationActions.Add(MessageStatus.Success, () =>
            {
                this._navigationService.NavigateTo("Main");
            });

            this.autorizationActions.Add(MessageStatus.Faulted, () =>
            {
                this._navigationService.NavigateTo("Registration");
            });

            this.LoginCmd = new RelayCommand(() =>
            {
                Messenger.Default.Send<LoginUserMessage>(new LoginUserMessage() { 
                  PhoneNumber = this.PhoneNumber,
                  PIN = this.PIN
                });
            });
            
            this.RegisterCmd = new RelayCommand(() =>
            {
                Messenger.Default.Send<RegisterUserMessage>(new RegisterUserMessage() { 
                  PhoneNumber = this.PhoneNumber
                });
            });
            
            Messenger.Default.Register<UserRegistrationResultMessage>(this, (msg) => {
                this.registrationActions[msg.Status].Invoke();
            });
            
            Messenger.Default.Register<UserAutorizationResultMessage>(this, (msg) => {
                this.autorizationActions[msg.Status].Invoke();
            });

            Messenger.Default.Register<ReadPhoneNumberResultMessage>(this, (msg) =>
            {
                this.PhoneNumber = msg.PhoneNumber;
                this.PIN = msg.PIN;
            });
            
        }

    }
}