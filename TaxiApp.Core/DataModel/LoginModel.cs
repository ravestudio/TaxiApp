using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using TaxiApp.Core.Messages;
using TaxiApp.Core.Repository;
using TaxiApp.Core.Managers;

using GalaSoft.MvvmLight.Messaging;

namespace TaxiApp.Core.DataModel
{
    public class LoginModel
    {
        
        private UserRepository _userRepository = null;
        private SystemManager _systemManager = null;
        private SMSManager _SMSManager = null;

        public string PhoneNumber { get; set; }
        public string Pin { get; set; }

        public LoginModel(TaxiApp.Core.Repository.UserRepository userRepository, SystemManager systemManager, SMSManager SMSManager)
        {
            this._userRepository = userRepository;
            this._systemManager = systemManager;
            this._SMSManager = SMSManager;

            this.ReadData();
            
             Messenger.Default.Register<RegisterUserMessage>(this, async (msg) => {
                 UserRegistrationResultMessage result = null;

                 CaughtSMSResultMessage message = new CaughtSMSResultMessage() { Status = MessageStatus.Faulted };

                 string pin = null;

                 using (ISMSStorage storage = await this._SMSManager.GetStorage())
                 {
                     result = await this.RegisterUser(msg.PhoneNumber);
                     var pinTask = this._SMSManager.CatchPIN(storage);
                     Messenger.Default.Send<UserRegistrationResultMessage>(result);

                     pin = await pinTask;
                 }

                 if (!string.IsNullOrEmpty(pin))
                 {
                     message.PIN = pin;
                     message.Status = MessageStatus.Success;
                 }

                 Messenger.Default.Send<CaughtSMSResultMessage>(message);
             });
             
            Messenger.Default.Register<LoginUserMessage>(this, async (msg) => {
                UserAutorizationResultMessage result = await this.Login(msg.PhoneNumber, msg.PIN);
                Messenger.Default.Send<UserAutorizationResultMessage>(result);
            });

        }

        public void SaveNumber(string phoneNumber)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["PhoneNumber"] = phoneNumber;
        }

        public void SavePIN(string PIN)
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values["pin"] = PIN;
        }

        public void ReadData()
        {
            object phone = Windows.Storage.ApplicationData.Current.LocalSettings.Values["PhoneNumber"];
            object pin = Windows.Storage.ApplicationData.Current.LocalSettings.Values["pin"];

            ReadPhoneNumberResultMessage msg = new ReadPhoneNumberResultMessage();

            if (phone != null)
            {
                this.PhoneNumber = Windows.Storage.ApplicationData.Current.LocalSettings.Values["PhoneNumber"].ToString();
                msg.PhoneNumber = this.PhoneNumber;
            }

            if (pin != null)
            {
                this.Pin = Windows.Storage.ApplicationData.Current.LocalSettings.Values["pin"].ToString();
                msg.PIN = this.Pin;
            }

            if (!(string.IsNullOrEmpty(msg.PhoneNumber) || string.IsNullOrEmpty(msg.PIN)))
            {
                Messenger.Default.Send<ReadPhoneNumberResultMessage>(msg);
            }
        }

        public void ClearData()
        {
            Windows.Storage.ApplicationData.Current.LocalSettings.Values.Clear();
        }
        
        private Task<UserRegistrationResultMessage> RegisterUser(string phoneNumber)
        {
            var tcs = new TaskCompletionSource<UserRegistrationResultMessage>();

            var task = this._userRepository.RegisterUser(phoneNumber);

            task.ContinueWith(t =>
            {
                SaveNumber(phoneNumber);

                tcs.SetResult( new UserRegistrationResultMessage() { Status = MessageStatus.Success });
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            task.ContinueWith(t =>
            {

                tcs.SetResult(new UserRegistrationResultMessage() { Status = MessageStatus.Faulted });

            }, TaskContinuationOptions.OnlyOnFaulted);

            return tcs.Task;
        }
        
        public Task<UserAutorizationResultMessage> Login(string PhoneNumber, string PIN)
        {
            var tcs = new TaskCompletionSource<UserAutorizationResultMessage>();
            
            string deviceId = _systemManager.GetDeviceId();
            
            var task = this._userRepository.GetUser(PhoneNumber, PIN, deviceId);
            
            task.ContinueWith(t =>
            {
                TaxiApp.Core.Session.Instance.SetUSer(t.Result);
                
                SavePIN(PIN);
                
                tcs.SetResult(new UserAutorizationResultMessage() { Status = MessageStatus.Success });
            }, TaskContinuationOptions.OnlyOnRanToCompletion);
            
            task.ContinueWith(t =>
            {
                ClearData();
                
                tcs.SetResult(new UserAutorizationResultMessage() { Status = MessageStatus.Faulted });
                
            }, TaskContinuationOptions.OnlyOnFaulted);
            
            return tcs.Task;
        }
        
    }
}
