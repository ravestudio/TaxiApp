﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{
    public class LoginUserMessage
    {
        public string PhoneNumber { get; set; }
        public string PIN { get; set; }
    }
}