using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net;
using XamarinTemplate.Services;
using XamarinTemplate.ViewModels;

namespace XamarinTemplate.Models
{
    class SampleModel
    {
        BaseViewModel Base = new BaseViewModel();
        public void CallApiSample(MainPageViewModel vm)
        {
            try
            {
                ServiceResponse responses = new ServiceResponse();
                responses = API.CallAPI(vm.Endpoint, null, Method.GET, null);

                JToken response = responses.data;
                if (responses.response == HttpStatusCode.OK)
                {
                    Base.Message = response.ToString();
                }
                else
                {
                    string text = "";
                    foreach (var item in response["errors"])
                    {
                        text += item.ToString() + "\n";
                    }
                    Base.Message = text;
                }
            }
            catch
            {
                Base.Message = "This feature is under maintenance";

            }
        }

    }
}
