using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{
    /// <summary>
    /// Результат регистрации пользователя
    /// </summary>
    public class UserRegistrationResultMessage
    {
        public MessageStatus Status { get; set; }
    }
}
