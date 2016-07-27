using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;

namespace TaxiApp.Core
{
    public class WebApiClient
    {
        public Task<string> GetData(string url, List<KeyValuePair<string, string>> data)
        {
            TaskCompletionSource<string> TCS = new TaskCompletionSource<string>();

            var uri = new Uri(url);
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            System.Net.Http.HttpContent content = new System.Net.Http.FormUrlEncodedContent(data);

            string text = string.Empty;


            Task<System.Net.Http.HttpResponseMessage> response = httpClient.PostAsync(uri, content);

            response.ContinueWith(r =>
            {
                string msg = r.Result.Content.ReadAsStringAsync().Result;
                TCS.SetResult(msg);
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            return TCS.Task;
        }
    }
}
