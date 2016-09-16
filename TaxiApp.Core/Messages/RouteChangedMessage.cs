﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Messages
{
    /// <summary>
    /// Маршрут изменен
    /// </summary>
    public class RouteChangedMessage
    {
        public IRoute route { get; set; }
    }
}
