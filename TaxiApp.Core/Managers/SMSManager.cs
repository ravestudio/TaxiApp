using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public Task<string> GetMessage()
        {
            return this._chatService.GetMessage();
        }
    }
}
