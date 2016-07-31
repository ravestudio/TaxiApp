using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Maps;

namespace TaxiApp.Core.UWP
{
    public class Route : IRoute
    {
        public MapRoute MapRoute { get; set; }
        public Route(MapRoute route)
        {
            this.MapRoute = route;
        }

        public double LengthInMeters
        {
            get { return MapRoute.LengthInMeters; }
        }

        public int EstimatedDuration
        {
            get { return MapRoute.EstimatedDuration.Minutes; }
        }
    }
}
