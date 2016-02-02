using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Repository
{
    public class DriverRepository : Repository<Entities.Driver, int>
    {
        public DriverRepository(TaxiApp.Core.WebApiClient apiClient)
            : base(apiClient)
        {

        }

        public override async Task<Entities.Driver> GetById(int id)
        {
            Entities.Driver driver = null;

            var postData = new List<KeyValuePair<string, string>>();

            TaxiApp.Core.Entities.IUser user = TaxiApp.Core.Session.Instance.GetUser();

            postData.Add(new KeyValuePair<string, string>("idpassenger", user.Id.ToString()));
            postData.Add(new KeyValuePair<string, string>("token", user.token));
            postData.Add(new KeyValuePair<string, string>("driverinfo", id.ToString()));

            string url = "http://serv.giddix.ru/api/passenger_getdriverinfo/";

            string data = await this._apiClient.GetData(url, postData);

            var jsonObj = Windows.Data.Json.JsonValue.Parse(data).GetObject();

            var resp = jsonObj["response"].GetObject();

            driver = this.GetObject(resp);

            driver.Id = id;

            return driver;
        }
    }
}
