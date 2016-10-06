using System;
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
using GalaSoft.MvvmLight;

namespace TaxiApp.Core.ViewModel
{
    public class AuthenticationViewModel : ViewModelBase
    {
        public string PhoneNumber { get; set; }
        public string PIN { get; set; }

        private bool _waitingSMS = false;
        public bool WaitingSMS
        {
            get
            {
                return _waitingSMS;
            }
            set
            {
                _waitingSMS = value;
                this.RaisePropertyChanged("WaitingSMS");
            }
        }

        public RelayCommand LoginCmd { get; set; }
        public RelayCommand RegisterCmd { get; set; }

        private IDictionary<MessageStatus, Action> registrationActions = null;
        private IDictionary<Func<UserAutorizationResultMessage, bool>, Action> autorizationActions = null;

        private INavigationService _navigationService = null;

        public AuthenticationViewModel(INavigationService NavigationService)
        {
            this._navigationService = NavigationService;

            this.registrationActions = new Dictionary<MessageStatus, Action>();
            this.autorizationActions = new Dictionary<Func<UserAutorizationResultMessage, bool>, Action>();

            this.registrationActions.Add(MessageStatus.Success, () =>
            {
                //Messenger.Default.Send<WaitSMSMessage>(new WaitSMSMessage());

                this.WaitingSMS = true;

                this._navigationService.NavigateTo("Authentication");
                
            });

            this.registrationActions.Add(MessageStatus.Faulted, () =>
            {
                var dlg = new Windows.UI.Popups.MessageDialog("Ошибка регистрации");
                dlg.ShowAsync();
            });

            this.autorizationActions.Add(m => { return m.Status == MessageStatus.Success && !m.HasPersonalInfo; }, () =>
            {
                this._navigationService.NavigateTo("EditProfile", "registration");
            });

            this.autorizationActions.Add(m => { return m.Status == MessageStatus.Success && m.HasPersonalInfo; }, () =>
            {
                this._navigationService.NavigateTo("Main");
            });

            this.autorizationActions.Add(m => { return m.Status == MessageStatus.Faulted; }, () =>
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
                this.autorizationActions.Single(a => a.Key.Invoke(msg)).Value.Invoke();
            });

            Messenger.Default.Register<ReadPhoneNumberResultMessage>(this, (msg) =>
            {
                this.PhoneNumber = msg.PhoneNumber;
                this.PIN = msg.PIN;
            });

            Messenger.Default.Register<CaughtSMSResultMessage>(this, (msg) =>
            {
                this.WaitingSMS = false;

                if (msg.Status == MessageStatus.Success)
                {
                    this.PIN = msg.PIN;
                    this.RaisePropertyChanged("PIN");
                }
            });
            
        }

    }
}
