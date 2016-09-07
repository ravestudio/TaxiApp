using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{
    /// <summary>
    /// Зарегистрировать пользователя
    /// </summary>
    public class RegisterUserMessage
    {
        public string PhoneNumber { get; set; }
    }
}
