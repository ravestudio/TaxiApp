using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Maps;

namespace TaxiApp.Core.UWP
{
    public class Location : ILocation
    {
        public MapLocation MapLocation { get; set; }
        public Location(MapLocation mapLocation)
        {
            this.MapLocation = mapLocation;
        }

        public string Country { get { return MapLocation.Address.Country; } }

        public string Town { get { return MapLocation.Address.Town; } }

        public string PostCode { get { return MapLocation.Address.PostCode; } }

        public string Region { get { return MapLocation.Address.Region; } }

        public string Street { get { return MapLocation.Address.Street; } }

        public string StreetNumber { get { return MapLocation.Address.StreetNumber; } }

        public double Latitude { get { return MapLocation.Point.Position.Latitude; } }
        public double Longitude { get { return MapLocation.Point.Position.Longitude; } }


    }
}
