using System;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinTemplate.Models;

namespace XamarinTemplate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpPage : PopupPage
    {
        public PopUpPage(ContentView content, bool closebackgroundclick)
        {
            InitializeComponent();
            container.Children.Add(content);
            CloseWhenBackgroundIsClicked = closebackgroundclick;
        }

        protected override void OnDisappearing()
        {
            if (GlobalVar.NotifShown)
            {
                GlobalVar.NotifShown = false;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
    }
}