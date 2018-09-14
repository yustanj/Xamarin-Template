using Plugin.Connectivity;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using XamarinTemplate.Models;
using XamarinTemplate.Views;

namespace XamarinTemplate.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public string BackgroundImage => "background.png";
        public static string PageTitle { get; set; }
        public string Message
        {
            set
            {
                if (!GlobalVar.NotifShown)
                {
                    GlobalVar.NotifShown = true;
                    Navigation.PushPopupAsync(new PopUpPage(new ContentView { Content = new ScrollView { Content = new Label { Text = value, FontSize = 15 } } }, true)).GetAwaiter();
                }
            }
        }

        public INavigation Navigation => App.Current.MainPage.Navigation;

        public Page currentpage
        {
            set
            {
                Navigation.InsertPageBefore(value, Navigation.NavigationStack[0]);
                Navigation.PopToRootAsync(false).GetAwaiter();
            }
        }
        public bool IsConnected
        {
            get
            {
                return CrossConnectivity.Current.IsConnected;
            }
        }

        public String ConvertImageURLToBase64(String url)
        {
            StringBuilder _sb = new StringBuilder();

            Byte[] _byte = GetImage(url);

            _sb.Append(Convert.ToBase64String(_byte, 0, _byte.Length));

            return _sb.ToString();
        }

        private byte[] GetImage(string url)
        {
            Stream stream = null;
            byte[] buf;

            try
            {
                WebProxy myProxy = new WebProxy();
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                stream = response.GetResponseStream();

                using (BinaryReader br = new BinaryReader(stream))
                {
                    int len = (int)(response.ContentLength);
                    buf = br.ReadBytes(len);
                    br.Close();
                }

                stream.Close();
                response.Close();
            }
            catch (Exception exp)
            {
                buf = null;
            }

            return (buf);
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
               [CallerMemberName]string propertyName = "",
               Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}