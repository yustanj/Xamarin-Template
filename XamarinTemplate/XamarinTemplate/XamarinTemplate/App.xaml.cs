using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using XamarinTemplate.ViewModels;
using XamarinTemplate.Models;
using XamarinTemplate.Helpers;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace XamarinTemplate
{
    public partial class App : Application
    {
        BaseViewModel Base = new BaseViewModel();
        public App()
        {
            InitializeComponent();
            
            //initialize notifshown as false so message can appear
            GlobalVar.NotifShown = false;

            if (Base.IsConnected)
            {
                if (AppsData.isLogin)
                {
                    MainPage = new NavigationPage(new Views.MainPage());
                }
                else
                {
                    MainPage = new NavigationPage(new Views.MainPage());
                }
            }
            else
            {
                ContentView content = new ContentView
                {
                    Content = new Label
                    {
                        Text = "Not connected to internet",
                        HorizontalTextAlignment = TextAlignment.Center
                    }
                };
                MainPage = new NavigationPage(new Views.PopUpPage(content, false) { BackgroundImage = "background.png" });

                Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
                {
                    if (Base.IsConnected)
                    {
                        if (AppsData.isLogin)
                        {
                            Base.currentpage = new Views.MainPage();
                        }
                        else
                        {
                            Base.currentpage = new Views.MainPage();
                        }

                        return false;
                    }
                    else
                    {
                        return true;
                    }
                });
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start("android=c322c56f-1196-4967-a012-c352413830b9;" +
                "ios=16b4867b-c6e3-47b3-9189-4867922b7c1b;",
                typeof(Analytics), typeof(Crashes));
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}