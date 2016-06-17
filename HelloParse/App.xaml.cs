using Xamarin.Forms;

namespace HelloParse
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			HelloParsePage page = new HelloParsePage();
			HelloViewModel viewModel = new HelloViewModel();
			page.BindingContext = viewModel;

			MainPage = page;
		}

		protected override void OnStart()
		{
			// Handle when your app starts
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

