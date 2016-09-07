using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{
    /// <summary>
    /// номер телефон и PIN считан из хранилища
    /// </summary>
    public class ReadPhoneNumberResultMessage
    {
        public string PhoneNumber { get; set; }
        public string PIN { get; set; }
    }
}
