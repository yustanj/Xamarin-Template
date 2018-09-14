using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using XamarinTemplate.Helpers;
using XamarinTemplate.Models;
using XamarinTemplate.ViewModels;

namespace XamarinTemplate.Services
{
    public class API
    {
        public static ServiceResponse CallAPI(string resource, string[,] parameter, Method method, string[,] header)
        {
            BaseViewModel Base = new BaseViewModel();
            ServiceResponse service = new ServiceResponse();

            if (!Base.IsConnected)
            {
                service.response = HttpStatusCode.NoContent;
                string errors = "{\"errors\":[\"Not connected to internet\"]}";
                service.data = (JToken)JsonConvert.DeserializeObject(errors);

                return service;
            }
            else
            {
                var request = new RestRequest(resource, method);

                if (parameter != null)
                {
                    for (int i = 0; i < parameter.Length / 2; i++)
                    {
                        request.AddParameter(parameter[i, 0], parameter[i, 1]); // adds to POST or URL querystring based on Method
                    }
                }

                if (header != null)
                {
                    for (int i = 0; i < header.Length / 2; i++)
                    {
                        request.AddHeader(header[i, 0], header[i, 1]);
                    }
                }

                // execute the request
                IRestResponse response = GlobalVar.Domain.Execute(request);
                service.response = response.StatusCode;
                if (service.response != HttpStatusCode.InternalServerError)
                {
                    service.data = (JToken)JsonConvert.DeserializeObject(response.Content);
                    if (service.response == HttpStatusCode.Unauthorized)
                    {
                        AppsData.isLogin=false;
                    }
                }
                else
                {
                    string errors = "{\"errors\":[\"Unknown error\"]}";
                    service.data = (JToken)JsonConvert.DeserializeObject(errors);
                }
                return service;
            }
        }
    }
    public class ServiceResponse
    {
        public HttpStatusCode response { get; set; }
        public JToken data { get; set; }
    }
}