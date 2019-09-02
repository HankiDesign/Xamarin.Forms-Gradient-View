using GradientControl.ViewModel;
using Xamarin.Forms;

namespace GradientControl
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
			BindingContext = new MainPageViewModel();
		}
	}
}
