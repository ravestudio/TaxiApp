using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{
    public class SavePersonalInfoMessage
    {
        public Entities.IUser PersonalInfo { get; set; }
    }
}
