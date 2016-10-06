using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{
    /// <summary>
    /// Результат сохранения персональной информации
    /// </summary>
    public class SavePersonalInfoResultMessage
    {
        public bool Edit { get; set; }
        public MessageStatus Status { get; set; }
    }
}
