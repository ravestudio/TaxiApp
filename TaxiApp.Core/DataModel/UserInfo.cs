using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using TaxiApp.Core.Repository;
using TaxiApp.Core.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace TaxiApp.Core.DataModel
{
    public class UserInfo
    {
        public string Name { get; set; }
        public string Surname {get; set;}
        public string Lastname {get; set;}
        public string Email { get; set; }

        public delegate void DataLoaded();
        public DataLoaded ModelChanged;

        private UserRepository _userRepository = null;

        public Windows.UI.Core.CoreDispatcher dispatcher { get; set; }

        public UserInfo(UserRepository userRepository)
        {
            this._userRepository = userRepository;

            this.Name = string.Empty;
            this.Surname = string.Empty;
            this.Lastname = string.Empty;
            this.Email = string.Empty;

            Messenger.Default.Register<RegisterUserMessage>(this, async (msg) => { });
        }

        public Task ReadData()
        {

            return _userRepository.GetMyInfo().ContinueWith(t =>
                {
                    Entities.IUser user = TaxiApp.Core.Session.Instance.GetUser();
                    this.Name = user.Name;
                    this.Surname = user.Surname;
                    this.Lastname = user.Lastname;
                    this.Email = user.Email;
                });
        }

        public Task<SavePersonalInfoResultMessage> SaveMyInfo(string Name, string Surname, string Lastname, string Email)
        {
            var tcs = new TaskCompletionSource<SavePersonalInfoResultMessage>();

            TaxiApp.Core.Entities.IUser user = TaxiApp.Core.Session.Instance.GetUser();
            user.Name = Name;
            user.Surname = Surname;
            user.Lastname = Lastname;
            user.Email = Email;

            var task = this._userRepository.SaveMyInfo();

            task.ContinueWith(t =>
            {

                tcs.SetResult(new SavePersonalInfoResultMessage() { Status = MessageStatus.Success });
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            task.ContinueWith(t =>
            {

                tcs.SetResult(new SavePersonalInfoResultMessage() { Status = MessageStatus.Faulted });

            }, TaskContinuationOptions.OnlyOnFaulted);

            return tcs.Task;
        }
    }
}
