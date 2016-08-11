using System;
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
        public string PhoneNumber { get; set; }
        public string PIN { get; set; }

        public RelayCommand LoginCmd { get; set; }
        public RelayCommand RegisterCmd { get; set; }

        public AuthenticationViewModel()
        {
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
            
            Messenger.Default.Register<UserRegisteredMessage>(this, (msg) => {
                Frame frame = Window.Current.Content as Frame;
                frame.Navigate(typeof(Views.AuthenticationPage));
            });
            
            Messenger.Default.Register<UserAutorizedMessage>(this, (msg) => {
                Frame frame = Window.Current.Content as Frame;
                frame.Navigate(typeof(Views.EditUserProfilePage));
            });
            
            Messenger.Default.Register<AutorizationErrorMessage>(this, (msg) => {
                Frame frame = Window.Current.Content as Frame;
                frame.Navigate(typeof(Views.RegistrationPage));
            });
        }

    }
}
