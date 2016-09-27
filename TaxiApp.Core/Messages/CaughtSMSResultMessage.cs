using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{
    /// <summary>
    /// Результат ожидания СМС
    /// </summary>
    public class CaughtSMSResultMessage
    {
        public MessageStatus Status { get; set; }

        public string PIN { get; set; }
    }
}
