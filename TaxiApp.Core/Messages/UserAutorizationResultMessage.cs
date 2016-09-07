using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{
    /// <summary>
    /// Результат авторизации пользователя
    /// </summary>
    public class UserAutorizationResultMessage
    {
        public MessageStatus Status { get; set; }
    }
}
