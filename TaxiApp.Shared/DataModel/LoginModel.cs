using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.DataModel
{
    public class LoginModel
    {
        public string PhoneNumber { get; set; }
        public string PIN { get; set; }
        
        private TaxiApp.Core.Repository.UserRepository _userRepository = null;

        public LoginModel(TaxiApp.Core.Repository.UserRepository userRepository, SystemManager)
        {
            this._userRepository = userRepository;
            this.PhoneNumber = string.Empty;
            this.PIN = string.Empty;

            this.ReadData();
            
             Messenger.Default.Register<RegisterUserMessage>(this, async (msg) => {
                 string result = await this.RegisterUser(msg.PhoneNumber);
                 
                 if (result == "success")
                 {
                    Messenger.Default.Send<UserRegisteredMessage>(new UserRegisteredMessage());
                 }
             });
             
            Messenger.Default.Register<LoginUserMessage>(this, async (msg) => {
                 string result = await this.Login(msg.PhoneNumber, msg.PIN);
                 
                 if (result == "success")
                 {
                    Messenger.Default.Send<UserAutorizedMessage>(new UserAutorizedMessage());
                 }
                 
                 if (result == "fail")
                 {
                     Messenger.Default.Send<AutorizationErrorMessage>(new AutorizationErrorMessage());
                 }
             });
        }

        public void SaveNumber()
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["PhoneNumber"] = this.PhoneNumber;
        }

        public void SavePIN()
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["pin"] = this.PIN;
        }

        public void ReadData()
        {
            object phone = Windows.Storage.ApplicationData.Current.LocalSettings.Values["PhoneNumber"];
            object pin = Windows.Storage.ApplicationData.Current.LocalSettings.Values["pin"];

            if (phone != null)
            {
                this.PhoneNumber = Windows.Storage.ApplicationData.Current.LocalSettings.Values["PhoneNumber"].ToString();
            }

            if (pin != null)
            {
                this.PIN = Windows.Storage.ApplicationData.Current.LocalSettings.Values["pin"].ToString();
            }
        }

        public void ClearData()
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values.Clear();
        }
        
        private Task<string> RegisterUser(string phoneNumber)
        {
            var tcs = new TaskCompletionSource<string>();
            
            userRepository.RegisterUser(model.PhoneNumber).ContinueWith(t =>
                {
                    model.SaveNumber();

                    tcs.SetResult(task.Result);
                });
                
            return tcs.Task;
        }
        
        private Task<string> Login(string PhoneNumber, string PIN)
        {
            var tcs = new TaskCompletionSource<string>();
            
            string deviceId = _systemManager.GetDeviceId();
            
            var task = userRepository.GetUser(PhoneNumber, PIN, deviceId);
            
            task.ContinueWith(t =>
            {
                TaxiApp.Core.Session.Instance.SetUSer(t.Result);
                
                model.SavePIN();
                
                tcs.SetResult("success");
            }, success);
            
            task.ContinueWith(t =>
            {
                model.ClearData();
                
                tcs.SetResult("fail");
                
            },fail);
            
            return tcs.Task;
        }
        
    }
}
