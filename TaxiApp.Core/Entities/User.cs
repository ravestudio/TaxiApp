using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Entities
{
    public interface IUser
    {
        int Id { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
        string Lastname { get; set; }
        string Email { get; set; }
        string PhoneNumber { get; set; }

        string token { get; set; }
        string pin { get; set; }
    }

    public class User: Entity<int>, IUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string token { get; set; }
        public string pin { get; set; }

        public override void ReadData(Windows.Data.Json.JsonObject jsonObj)
        {
            this.Id = (int)jsonObj["response"].GetObject()["idpassenger"].GetNumber();
            this.token = jsonObj["response"].GetObject()["token"].GetString();
            //this.pin = jsonObj["response"].GetObject()["pin"].GetString();

            base.ReadData(jsonObj);
        }
    }
}
