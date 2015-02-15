using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Entities
{
    public class Driver : Entity<int>
    {
        public string Name { get; set; }
        public string Lastname { get; set; }

        public int CarId { get; set; }
        public string Licenseplate { get; set; }
        public string DriverPhotolink { get; set; }
        public string Carphotolink { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.Name, this.Lastname);
            }
        }

        public override void ReadData(Windows.Data.Json.JsonObject jsonObj)
        {

            this.Name = jsonObj["name"].GetString();
            this.Lastname = jsonObj["lastname"].GetString();

            this.CarId =  this.ReadValue(jsonObj, "idcar");
            this.Licenseplate = jsonObj["licenseplate"].GetString();

            this.DriverPhotolink = jsonObj["driverphotolink"].GetString();
            this.Carphotolink = jsonObj["carphotolink"].GetString();

            base.ReadData(jsonObj);
        }
    }
}
