using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace TaxiApp.Core
{
    public interface ILocation
    {
        string PostCode { get; }
        string Country { get; }
        string Town { get; }
        string Region { get; }
        string Street { get; }
        string StreetNumber { get; }

        double Latitude { get; }
        double Longitude { get; }

        Geopoint GetGeopoint();
    }
}
