using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiApp.Core.Socket
{
    public class SocketManager
    {
        private SocketClient client = null;

        public SocketManager(SocketClient client)
        {
            this.client = client;
        }

        public Task Auth()
        {
            //SocketRequest request = new SocketRequest();
            //request.client_type = "passenger";
            //request.clientid = TaxiApp.Core.Session.Instance.GetUser().Id;
            //request.token = TaxiApp.Core.Session.Instance.GetUser().token;
            //request.request="auth";

            Windows.Data.Json.JsonObject json = new Windows.Data.Json.JsonObject();
            json.Add("client_type", Windows.Data.Json.JsonValue.CreateStringValue("passenger"));
            json.Add("clientid", Windows.Data.Json.JsonValue.CreateStringValue(TaxiApp.Core.Session.Instance.GetUser().Id.ToString()));
            json.Add("token", Windows.Data.Json.JsonValue.CreateStringValue(TaxiApp.Core.Session.Instance.GetUser().token));
            json.Add("request", Windows.Data.Json.JsonValue.CreateStringValue("auth"));
            string msg = json.Stringify();

            Task send = client.SendAsync(msg);

            return send;
        }

        public void Start()
        {
            client.StrartListen();
        }

        public Task Read()
        {
            Task read = client.ReceiveDataAsync();

            return read;
        }

        public SocketResponse ProcSocketResp(string msg)
        {

            string auth_exampl = "{\"error\":\"0\", \"request\":\"auth\"}";
            string changeStatus_example = "{\"error\":\"0\", \"request\":\"neworderstatus\", \"params\":{\"idorder\": \"641\", \"status\":\"3\"}}";

            SocketResponse resp = null;

            if (msg.StartsWith("42"))
            {
                resp = new SocketResponse();

                msg = msg.Remove(0, 2);
                var ObjArray = Windows.Data.Json.JsonValue.Parse(msg).GetArray();

                string message = ObjArray[1].GetString();
                //string message = changeStatus_example;

                var RespObj = Windows.Data.Json.JsonValue.Parse(message).GetObject();

                resp.request = RespObj["request"].GetString();

                if (resp.request == "neworderstatus")
                {
                    var paramsObj = RespObj["params"].GetObject();

                    if (paramsObj["idorder"].ValueType == Windows.Data.Json.JsonValueType.Number)
                    {
                        resp.idorder = (int)paramsObj["idorder"].GetNumber();
                    }

                    if (paramsObj["idorder"].ValueType == Windows.Data.Json.JsonValueType.String)
                    {
                        resp.idorder = int.Parse(paramsObj["idorder"].GetString(), System.Globalization.CultureInfo.InvariantCulture);
                    }

                    if (paramsObj["status"].ValueType == Windows.Data.Json.JsonValueType.Number)
                    {
                        resp.orderstatus = (int)paramsObj["status"].GetNumber();
                    }

                    if (paramsObj["status"].ValueType == Windows.Data.Json.JsonValueType.String)
                    {
                        resp.orderstatus = int.Parse(paramsObj["status"].GetString(), System.Globalization.CultureInfo.InvariantCulture);
                    }

                }
            }

            return resp;
        }

    }
}
