using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TaxiApp.ViewModel
{
    public class EditUserProfileViewModel : ViewModel
    {
        public TaxiApp.Core.DataModel.UserInfo UserInfo { get; set; }

        public SaveInfoCommand SaveCmd { get; set; }

        public EditUserProfileViewModel()
        {
            this.UserInfo = new Core.DataModel.UserInfo();
            this.SaveCmd = new SaveInfoCommand();

            this.UserInfo.ModelChanged += OnModelLoaded;
        }

        private void OnModelLoaded()
        {
            NotifyPropertyChanged("UserInfo.Name");
            NotifyPropertyChanged("Surname");
            NotifyPropertyChanged("Lastname");
            NotifyPropertyChanged("Email");
        }

        public override void Init(Page Page)
        {
            base.Init(Page);

            this.UserInfo.dispatcher = Page.Dispatcher;
        }
    }

    public class SaveInfoCommand : System.Windows.Input.ICommand
    {
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            EditUserProfileViewModel viewModel = ViewModelFactory.Instance.GetEditUserProfileViewModel();
            TaxiApp.Core.Entities.IUser user = TaxiApp.Core.Session.Instance.GetUser();
            user.Name = viewModel.UserInfo.Name;
            user.Surname = viewModel.UserInfo.Surname;
            user.Lastname = viewModel.UserInfo.Lastname;
            user.Email = viewModel.UserInfo.Email;

            TaxiApp.Core.WebApiClient client = new TaxiApp.Core.WebApiClient();
            TaxiApp.Core.Repository.UserRepository userRepository = new Core.Repository.UserRepository(client);

            userRepository.SaveMyInfo().ContinueWith(t =>
            {
                string msg = t.Result;

                Windows.Foundation.IAsyncAction action =
                viewModel.Page.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    int thread = Environment.CurrentManagedThreadId;

                    Frame frame = viewModel.Page.Frame;

                    frame.Navigate(typeof(Views.MainPage));
                });
            });
        }
    }
}
