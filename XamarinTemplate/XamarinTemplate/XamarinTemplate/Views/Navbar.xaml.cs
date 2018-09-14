using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XamarinTemplate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Navbar : ContentView
	{
		public Navbar ()
		{
			InitializeComponent ();
            if (Navigation.NavigationStack.Count == 0)
            {
                Back.IsVisible = false;
            }
        }
	}
}