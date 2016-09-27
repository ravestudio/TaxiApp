﻿using System;
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
        private SMSManager _smsManager = null;

        public LoginModel(TaxiApp.Core.Repository.UserRepository userRepository, SystemManager systemManager, SMSManager smsManager)
        {
            this._userRepository = userRepository;
            this._systemManager = systemManager;
            this._smsManager = smsManager;

            this.ReadData();
            
             Messenger.Default.Register<RegisterUserMessage>(this, async (msg) => {
                 UserRegistrationResultMessage result = await this.RegisterUser(msg.PhoneNumber);

                 Messenger.Default.Send<UserRegistrationResultMessage>(result);
                 
             });
             
            Messenger.Default.Register<LoginUserMessage>(this, async (msg) => {
                 UserAutorizationResultMessage result = await this.Login(msg.PhoneNumber, msg.PIN);
                 
                 Messenger.Default.Send<UserAutorizationResultMessage>(result);

             });

            Messenger.Default.Register<WaitSMSMessage>(this, async (msg) => {
                string smsBody = await this._smsManager.GetMessage();

                Regex rgx = new Regex(@"^TAXI PIN (?<PIN>\d{4})$");

                CaughtSMSResultMessage message = new CaughtSMSResultMessage() { Status = MessageStatus.Faulted };


                if (!string.IsNullOrEmpty(smsBody) && rgx.IsMatch(smsBody))
                {
                    Match match = rgx.Match(smsBody);
                    message.PIN = match.Groups["PIN"].Value;
                    message.Status = MessageStatus.Success;
                }

                Messenger.Default.Send<CaughtSMSResultMessage>(message);
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
                msg.PhoneNumber = Windows.Storage.ApplicationData.Current.LocalSettings.Values["PhoneNumber"].ToString();
            }

            if (pin != null)
            {
                msg.PIN = Windows.Storage.ApplicationData.Current.LocalSettings.Values["pin"].ToString();
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
        
        private Task<UserAutorizationResultMessage> Login(string PhoneNumber, string PIN)
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
