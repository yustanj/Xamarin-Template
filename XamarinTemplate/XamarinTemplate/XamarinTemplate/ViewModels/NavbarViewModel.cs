using System.Windows.Input;
using Xamarin.Forms;

namespace XamarinTemplate.ViewModels
{
    public class NavbarViewModel : BaseViewModel
    {
        public string Title => PageTitle;
        public ICommand Back { get; set; }

        public NavbarViewModel()
        {
            Back = new Command(back);
        }

        private void back()
        {
            Navigation.PopAsync().GetAwaiter();
        }
    }
}
