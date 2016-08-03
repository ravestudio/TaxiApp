﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{
    public class LocationChangedMessage
    {
        public bool Ready { get; set; }
        public ILocation Location { get; set; }
    }
}