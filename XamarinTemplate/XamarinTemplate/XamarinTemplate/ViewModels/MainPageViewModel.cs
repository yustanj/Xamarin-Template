using Rg.Plugins.Popup.Extensions;
using System;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using XamarinTemplate.Helpers;
using XamarinTemplate.Models;
using XamarinTemplate.Views;
using ZXing.Net.Mobile.Forms;

namespace XamarinTemplate.ViewModels
{
    class MainPageViewModel : BaseViewModel
    {
        Camera Camera = new Camera();
        SampleModel Sample = new SampleModel();

        public string PictureBase64 { get; set; }
        ImageSource picture = new Image { Source = "Icon.png" }.Source;
        public ImageSource Picture
        {
            get
            {
                return picture;
            }
            set
            {
                SetProperty(ref picture, value);
            }
        }

        public string Endpoint { get; set; }

        public ICommand CameraSelector { get; set; }
        public ICommand SaveData { get; set; }
        public ICommand LoadData { get; set; }
        public ICommand CallApi { get; set; }
        public ICommand ShowMessage { get; set; }
        public ICommand ShowMap { get; set; }
        public ICommand Scan { get; set; }

        public MainPageViewModel()
        {
            PageTitle = "Home";

            CameraSelector = new Command(cameraselector);
            SaveData = new Command(savedata);
            LoadData = new Command(loaddata);
            CallApi = new Command(callapi);
            ShowMessage = new Command(showmessage);
            ShowMap = new Command(showmap);
            Scan = new Command(scan);
        }

        private async void cameraselector()
        {
            Button Choose = new Button()
            {
                BackgroundColor = Color.Transparent,
                TextColor = Color.Blue,
                Text = "Choose Picture"
            };
            Button Take = new Button()
            {
                BackgroundColor = Color.Transparent,
                TextColor = Color.Blue,
                Text = "Take Picture"
            };
            Button Show = new Button()
            {
                BackgroundColor = Color.Transparent,
                TextColor = Color.Blue,
                Text = "View Picture"
            };

            Choose.Clicked += async (s, e) =>
            {
                await Navigation.PopPopupAsync();

                PictureBase64 = await Camera.ChoosePicture();

                ChangePicture(PictureBase64);
            };

            //Take pivture is under development
            Take.Clicked += async (s, e) =>
            {
                await Navigation.PopPopupAsync();

                PictureBase64 = await Camera.TakePicture();

                ChangePicture(PictureBase64);
            };
            Show.Clicked += async (s, e) =>
            {
                await Navigation.PopPopupAsync();
                await Navigation.PushAsync(new ContentPage()
                {
                    BackgroundColor = Color.Black,
                    Title = "Picture",
                    Content = Camera.ViewPicture(Picture)
                });
            };

            ContentView content = new ContentView()
            {
                Content = new StackLayout
                {
                    Children = {
                        Show,
                        Choose
                        ,Take
                    }
                }
            };

            await Navigation.PushPopupAsync(new PopUpPage(content, true));
        }

        private void ChangePicture(string Base64Image)
        {
            if (Base64Image != null)
            {
                byte[] byteArray = Convert.FromBase64String(Base64Image);
                Picture = ImageSource.FromStream(() => new MemoryStream(byteArray));
            }
        }

        private void savedata()
        {
            ContentView content = new ContentView();

            StackLayout data = new StackLayout();

            Entry text = new Entry
            {
                Placeholder = "Text to save",
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black
            };
            data.Children.Add(text);

            Button Save = new Button { Text = "Save" };
            Save.Clicked += (s, e) =>
            {
                AppsData.SampleText = text.Text;
                Navigation.PopPopupAsync();
            };
            data.Children.Add(Save);

            content.Content = data;
            Navigation.PushPopupAsync(new PopUpPage(content, true));
        }
        private void loaddata()
        {
            Message = AppsData.SampleText;
        }
        private void callapi()
        {
            ContentView content = new ContentView();

            StackLayout data = new StackLayout();

            Label Baseurl = new Label
            {
                Text = GlobalVar.BaseUrl
            };
            data.Children.Add(Baseurl);

            Entry text = new Entry
            {
                Placeholder = "Endpoint",
                PlaceholderColor = Color.Gray,
                TextColor = Color.Black
            };
            data.Children.Add(text);
            text.TextChanged += (s, e) =>
            {
                Endpoint = text.Text;
            };

            Button Save = new Button { Text = "Call" };
            Save.Clicked += (s, e) =>
            {
                Navigation.PopPopupAsync();
                Sample.CallApiSample(this);
            };
            data.Children.Add(Save);

            content.Content = data;
            Navigation.PushPopupAsync(new PopUpPage(content, true));

        }
        private void showmessage()
        {
            Message = "This is sample message";
        }
        private void showmap()
        {
            ContentView content = new ContentView();

            Map geolocation = new Map() { HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 150, WidthRequest = 150 };
            double lat = double.Parse("-6.174974");
            double longi = double.Parse("106.880487");
            Pin pin = new Pin() { Position = new Position(lat, longi), Label = "I am here!!!" };
            geolocation.Pins.Add(pin);
            geolocation.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(lat, longi), new Distance(300d)));

            content.Content = geolocation;
            Navigation.PushPopupAsync(new PopUpPage(content, true));
        }
        private void scan()
        {
            StackLayout content = new StackLayout();

            Label title = new Label
            {
                Text = "Scan QR or Bar code",
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HorizontalTextAlignment = TextAlignment.Center
            };

            var scanner = new ZXingScannerView()
            {
                IsScanning = true,
                WidthRequest = 200,
                HeightRequest = 200
            };
            scanner.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopPopupAsync(false);
                    Message = result.ToString();
                });
            };


            content.Children.Add(title);
            content.Children.Add(scanner);

            ContentView contents = new ContentView();
            contents.Content = content;
            Navigation.PushPopupAsync(new PopUpPage(contents, true));

            //Navigation.PushAsync(scanner);

        }
    }
}