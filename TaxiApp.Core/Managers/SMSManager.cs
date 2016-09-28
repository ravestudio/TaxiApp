using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//using Windows.ApplicationModel.Chat;

namespace TaxiApp.Core.Managers
{
    public class SMSManager
    {
        private IChatService _chatService = null;

        public SMSManager(IChatService chatService)
        {
            this._chatService = chatService;
        }

        public Task<ISMSStorage> GetStorage()
        {
            return this._chatService.GetStorage();
        }


        public Task<string> CatchPIN(ISMSStorage storage)
        {
            TaskCompletionSource<string> TCS = new TaskCompletionSource<string>();

            storage.GetMessage().ContinueWith(t =>
            {
                Regex rgx = new Regex(@"^TAXI PIN (?<PIN>\d{4})$");

                string pin = null;

                string smsBody = t.Result;

                if (!string.IsNullOrEmpty(smsBody) && rgx.IsMatch(smsBody))
                {
                    Match match = rgx.Match(smsBody);
                    pin = match.Groups["PIN"].Value;
                }

                TCS.SetResult(pin);
            });

            return TCS.Task;
        }
    }
}
