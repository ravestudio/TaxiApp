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

        private IDictionary<Func<SavePersonalInfoResultMessage, bool>, Action> actions = null;

        private INavigationService _appNavigationService = null;
        private INavigationService _childNavigationService = null;

        private bool _edit = false;

        public EditUserProfileViewModel(INavigationService appNavigationService, INavigationService childNavigationService)
        {
            this._appNavigationService = appNavigationService;
            this._childNavigationService = childNavigationService;

            this.actions = new Dictionary<Func<SavePersonalInfoResultMessage, bool>, Action>();


            this.actions.Add(m => { return m.Status == MessageStatus.Success && !m.Edit; }, () =>
            {
                this._appNavigationService.NavigateTo("Main");
            });

            this.actions.Add(m => { return m.Status == MessageStatus.Success && m.Edit; }, () =>
            {
                this._childNavigationService.NavigateTo("EditOrder");
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
                    PersonalInfo = personalInfo,
                    Edit = this._edit
                });

                Messenger.Default.Register<SavePersonalInfoResultMessage>(this, (msg) => {
                    this.actions.Single(a => a.Key.Invoke(msg)).Value.Invoke();
                });
            });


        }

        public void LoadInfo(bool edit)
        {
            this._edit = edit;
            IUser user = TaxiApp.Core.Session.Instance.GetUser();

            this.Name = user.Name;
            this.Surname = user.Surname;
            this.Lastname = user.Lastname;
            this.Email = user.Email;

        }

    }

}
