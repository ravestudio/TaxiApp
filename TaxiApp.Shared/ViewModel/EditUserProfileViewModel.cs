using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaxiApp.Core.ViewModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;
using TaxiApp.Core.Messages;
using TaxiApp.Core.Entities;
using GalaSoft.MvvmLight.Views;

namespace TaxiApp.ViewModel
{
    public class EditUserProfileViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }

        public RelayCommand SaveCmd { get; set; }

        private IDictionary<MessageStatus, Action> actions = null;

        private INavigationService _navigationService = null;

        public EditUserProfileViewModel(INavigationService NavigationService)
        {
            this.actions = new Dictionary<MessageStatus, Action>();


            this.actions.Add(MessageStatus.Success, () =>
            {
                this._navigationService.NavigateTo("Main");
            });

            this.SaveCmd = new RelayCommand(() =>
            {
                IUser personalInfo = new User()
                {
                    Name = Name,
                    Surname = Surname,
                    Lastname = Lastname,
                    Email = Email
                };

                Messenger.Default.Send<SavePersonalInfoMessage>(new SavePersonalInfoMessage()
                {
                    PersonalInfo = personalInfo
                });

                Messenger.Default.Register<SavePersonalInfoResultMessage>(this, (msg) => {
                    this.actions[msg.Status].Invoke();
                });
            });


        }

    }

}
