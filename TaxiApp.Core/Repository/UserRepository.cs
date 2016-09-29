using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Repository
{
    public class UserRepository: Repository<Entities.User, int>
    {
        public UserRepository(TaxiApp.Core.WebApiClient apiClient) : base(apiClient)
        {
        }

        public Task<string> RegisterUser(string PhoneNumber)
        {
            TaskCompletionSource<string> TCS = new TaskCompletionSource<string>();

            string url = string.Format("{0}{1}", this.ServerURL, "api/passenger_registration/");

            var postData = new List<KeyValuePair<string, string>>();

            postData.Add(new KeyValuePair<string, string>("phone", PhoneNumber));        
            postData.Add(new KeyValuePair<string, string>("idcompany", "1"));
            postData.Add(new KeyValuePair<string, string>("licenseaccepted", "1"));


            //object pin = Windows.Storage.ApplicationData.Current.LocalSettings.Values["pin"];
            object pin = null;
            if (pin != null)
            {
                TCS.SetResult("success");
            }
            else
            {
                this._apiClient.GetData(url, postData).ContinueWith(t =>
                {
                    var resp = Windows.Data.Json.JsonValue.Parse(t.Result);

                    int error = this.GetErrorInfo(resp);

                    if (error != 0)
                    {
                        TCS.SetException(new Exception());
                    }

                    if (error == 0)
                    {

                        TCS.SetResult("success");
                    }
                });
            }

            return TCS.Task;

        }

        public Task GetMyInfo()
        {
            Task resultTask = null;

            string url = string.Format("{0}{1}", this.ServerURL, "api/passenger_getmyinfo/");

            Entities.IUser user = TaxiApp.Core.Session.Instance.GetUser();

            var postData = new List<KeyValuePair<string, string>>();

            postData.Add(new KeyValuePair<string, string>("idpassenger", user.Id.ToString()));
            postData.Add(new KeyValuePair<string, string>("token", user.token.ToString()));

            resultTask = this._apiClient.GetData(url, postData).ContinueWith(t =>
                {
                    string data = t.Result;

                    var jsonObj = Windows.Data.Json.JsonValue.Parse(data).GetObject();

                    user.Name = jsonObj["response"].GetObject()["name"].GetString();
                    user.Surname = jsonObj["response"].GetObject()["surname"].GetString();
                    user.Lastname = jsonObj["response"].GetObject()["lastname"].GetString();
                    user.Email = jsonObj["response"].GetObject()["email"].GetString();
                });

            return resultTask;
        }

        public Task<string> SaveMyInfo()
        {
            System.Threading.Tasks.TaskCompletionSource<string> TCS = new TaskCompletionSource<string>();

            string url = string.Format("{0}{1}", this.ServerURL, "api/passenger_setsettings/");

            Entities.IUser user = TaxiApp.Core.Session.Instance.GetUser();

            var postData = new List<KeyValuePair<string, string>>();

            postData.Add(new KeyValuePair<string, string>("idpassenger", user.Id.ToString()));
            postData.Add(new KeyValuePair<string, string>("token", user.token.ToString()));
            postData.Add(new KeyValuePair<string, string>("name", user.Name.ToString()));
            postData.Add(new KeyValuePair<string, string>("surname", user.Surname.ToString()));
            postData.Add(new KeyValuePair<string, string>("lastname", user.Lastname.ToString()));
            postData.Add(new KeyValuePair<string, string>("email", user.Email.ToString()));

            this._apiClient.GetData(url, postData).ContinueWith(t =>
                {

                    var value = Windows.Data.Json.JsonValue.Parse(t.Result);

                    int error = this.GetErrorInfo(value);

                    if (error != 0)
                    {
                        TCS.SetException(new Exception());
                    }

                    if (error == 0)
                    {

                        TCS.SetResult("success");
                    }

                });

            return TCS.Task;
        }

        public Task<Entities.User> GetUser(string PhoneNumber, string PIN, string protector)
        {
            TaskCompletionSource<Entities.User> TCS = new TaskCompletionSource<Entities.User>();

            string url = string.Format("{0}{1}", this.ServerURL, "api/passenger_auth/");

            var postData = new List<KeyValuePair<string, string>>();

            postData.Add(new KeyValuePair<string, string>("phone", PhoneNumber));
            postData.Add(new KeyValuePair<string, string>("pin", PIN));
            postData.Add(new KeyValuePair<string, string>("protector", protector));
            postData.Add(new KeyValuePair<string, string>("idcompany", "1"));
            postData.Add(new KeyValuePair<string, string>("type", "4"));
            postData.Add(new KeyValuePair<string, string>("appversion", "80"));

            //string data = string.Format("phone={0}&pin={1}&idcompany={2}", model.PhoneNumber, model.PIN, 1);

            int thread = Environment.CurrentManagedThreadId;

            this._apiClient.GetData(url, postData).ContinueWith(t =>
            {
                thread = Environment.CurrentManagedThreadId;

                var userValue = Windows.Data.Json.JsonValue.Parse(t.Result);

                int error = this.GetErrorInfo(userValue);

                if (error != 0)
                {
                    TCS.SetException(new Exception());
                }

                if (error == 0)
                {
                    TCS.SetResult(this.GetObject(userValue));
                }
            });

            return TCS.Task;
        }
    }
}
