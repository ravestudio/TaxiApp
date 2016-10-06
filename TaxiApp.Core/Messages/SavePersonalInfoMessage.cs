using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{
    public class SavePersonalInfoMessage
    {
        public bool Edit { get; set; }
        public Entities.IUser PersonalInfo { get; set; }
    }
}
