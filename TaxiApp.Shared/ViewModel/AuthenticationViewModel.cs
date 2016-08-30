using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiApp.Core.Messages;

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.ViewModel
{
    public class AuthenticationViewModel : ViewModel
    {
        public string PhoneNumber { get; set; }
        public string PIN { get; set; }

        public RelayCommand LoginCmd { get; set; }
        public RelayCommand RegisterCmd { get; set; }

        private IDictionary<MessageStatus, Action> registrationActions = null;
        private IDictionary<MessageStatus, Action> autorizationActions = null;

        public AuthenticationViewModel()
        {

            this.registrationActions = new Dictionary<MessageStatus, Action>();
            this.autorizationActions = new Dictionary<MessageStatus, Action>();

            this.registrationActions.Add(MessageStatus.Success, () =>
            {
                Frame frame = Window.Current.Content as Frame;
                frame.Navigate(typeof(Views.AuthenticationPage));
            });

            this.registrationActions.Add(MessageStatus.Faulted, () =>
            {
                var dlg = new Windows.UI.Popups.MessageDialog("Ошибка регистрации");
                dlg.ShowAsync();
            });

            this.autorizationActions.Add(MessageStatus.Success, () =>
            {
                Frame frame = Window.Current.Content as Frame;
                frame.Navigate(typeof(Views.MainPage));
            });

            this.autorizationActions.Add(MessageStatus.Faulted, () =>
            {
                Frame frame = Window.Current.Content as Frame;
                frame.Navigate(typeof(Views.RegistrationPage));
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
