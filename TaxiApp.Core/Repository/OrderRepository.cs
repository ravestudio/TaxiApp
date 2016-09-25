using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Repository
{
    public class OrderRepository : Repository<Entities.Order, int>
    {
        public OrderRepository(TaxiApp.Core.WebApiClient apiClient): base(apiClient)
        {

        }

        public override void Create(Entities.Order order)
        {
            base.Create(order);
        }

        public Task<string> CreateOrder(TaxiApp.Core.Entities.Order order)
        {
            string url = string.Format("{0}{1}", this.ServerURL, "api/passenger_setorder/");

            TaxiApp.Core.Entities.IUser user = TaxiApp.Core.Session.Instance.GetUser();

            var postData = order.ConverToKeyValue();

            //var postData = new List<KeyValuePair<string, string>>();

            postData.Add(new KeyValuePair<string, string>("idpassenger", user.Id.ToString()));
            postData.Add(new KeyValuePair<string, string>("token", user.token));
            postData.Add(new KeyValuePair<string, string>("idcompany", "1"));

            TaxiApp.Core.WebApiClient client = new TaxiApp.Core.WebApiClient();

            return client.GetData(url, postData);
        }

        public Task<IList<Entities.Order>> GetUserOrders(Entities.IUser user)
        {
            TaskCompletionSource<IList<Entities.Order>> TCS = new TaskCompletionSource<IList<Entities.Order>>();

            IList<Entities.Order> orderList = new List<Entities.Order>();

            var postData = new List<KeyValuePair<string, string>>();

            postData.Add(new KeyValuePair<string, string>("idpassenger", user.Id.ToString()));
            postData.Add(new KeyValuePair<string, string>("token", user.token));
            postData.Add(new KeyValuePair<string, string>("idcompany", "1"));

            string url = string.Format("{0}{1}", this.ServerURL, "api/passenger_getmyorders/");

            this._apiClient.GetData(url, postData).ContinueWith(t =>
            {
                string data = t.Result;

                var jsonObj = Windows.Data.Json.JsonValue.Parse(data).GetObject();

                var resp = jsonObj["response"].GetObject();
                var orders = resp["orders"];


                if (orders.ValueType == Windows.Data.Json.JsonValueType.Array)
                {
                    var orderArray = orders.GetArray();

                    for (int i = 0; i < orderArray.Count; i++)
                    {
                        var ordderValue = orderArray[i];
                        Entities.Order order = this.GetObject(ordderValue);
                        orderList.Add(order);
                    }
                }

                TCS.SetResult(orderList);
            });

            return TCS.Task;
        }

        public Task<bool> DeleteOrder(int OrderId)
        {

            TaskCompletionSource<bool> TCS = new TaskCompletionSource<bool>();

            TaxiApp.Core.Entities.IUser user = TaxiApp.Core.Session.Instance.GetUser();

            string url = string.Format("{0}{1}", this.ServerURL, "api/passenger_cancelorder/");

            var postData = new List<KeyValuePair<string, string>>();

            postData.Add(new KeyValuePair<string, string>("idpassenger", user.Id.ToString()));
            postData.Add(new KeyValuePair<string, string>("token", user.token));
            postData.Add(new KeyValuePair<string, string>("idorder", OrderId.ToString()));

            this._apiClient.GetData(url, postData).ContinueWith(t =>
            {
                TCS.SetResult(t.Result == "{\"error\":0}");
            });

            return TCS.Task;
        }
    }
}
