using RestSharp;
using System;
using XamarinTemplate.ViewModels;
using XamarinTemplate.Views;

namespace XamarinTemplate.Models
{
    public static class GlobalVar
    {
        //init template
        public static string BaseUrl = "https://api.loyalto.id/api/v1/";
        public static RestClient Domain => new RestClient(BaseUrl);
        
        public static bool NotifShown { get; set; }
    }
}