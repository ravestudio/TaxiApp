using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace TaxiApp.Core.DataModel
{
    public class UserInfo : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Surname {get; set;}
        public string Lastname {get; set;}
        public string Email { get; set; }

        public delegate void DataLoaded();
        public DataLoaded ModelChanged;

        public Windows.UI.Core.CoreDispatcher dispatcher { get; set; }

        public UserInfo()
        {
            this.Name = string.Empty;
            this.Surname = string.Empty;
            this.Lastname = string.Empty;
            this.Email = string.Empty;

            this.ReadData();
        }

        public void ReadData()
        {
            TaxiApp.Core.WebApiClient client = new TaxiApp.Core.WebApiClient();

            TaxiApp.Core.Repository.UserRepository userRepository = new Core.Repository.UserRepository(client);

            userRepository.GetMyInfo().ContinueWith(t =>
                {
                    Entities.User user = TaxiApp.Core.Session.Instance.GetUser();
                    this.Name = user.Name;
                    this.Surname = user.Surname;
                    this.Lastname = user.Lastname;
                    this.Email = user.Email;

                    //if (this.ModelChanged != null)
                    //{
                    //    this.ModelChanged();
                    //}
                    NotifyPropertyChanged("Name");
                    NotifyPropertyChanged("Surname");
                    NotifyPropertyChanged("Lastname");
                    NotifyPropertyChanged("Email");
                });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {

                Windows.Foundation.IAsyncAction action =
                    dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
                    });
            }
        }
    }
}
