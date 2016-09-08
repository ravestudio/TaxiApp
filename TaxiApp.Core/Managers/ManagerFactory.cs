using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Managers
{
    public class ManagerFactory : TaxiApp.Core.Singleton<ManagerFactory>
    {
        private LocationManager locationMG = null;
        private MapPainter mapPainter = null;
        private SystemManager systemMG = null;

        //public LocationManager GetLocationManager()
        //{
        //    if (this.locationMG == null)
        //    {
        //        this.locationMG = new LocationManager();
        //    }

        //    return this.locationMG;
        //}

        //public MapPainter GetMapPainter(MapControl mapControl)
        //{
        //    if (this.mapPainter == null)
        //    {
        //        this.mapPainter = new MapPainter(new Map(mapControl));
        //    }

        //    return this.mapPainter;
        //}

        public SystemManager GetSystemManager()
        {
            if (this.systemMG == null)
            {
                this.systemMG = new SystemManager();
            }

            return this.systemMG;
        }

    }
}
