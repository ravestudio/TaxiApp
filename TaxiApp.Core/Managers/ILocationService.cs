using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace TaxiApp.Core.Managers
{
    public interface ILocationService
    {
        Task<IRoute> GetRoute(IEnumerable<Geopoint> points);

        Task<ILocation> GetLocation(Geopoint hintPoint);

        Task<IList<ILocation>> GetLocationList(Geopoint hintPoint, string searchQuery);
    }
}
