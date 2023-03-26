using AlumniNetMobile.Common;
using AlumniNetMobile.Views;
using AlumniNetMobileApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AlumniNetMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            try
            {
                var authenticationService = DependencyService.Resolve<IAuthenticationService>();
                if (!authenticationService.IsSignedIn())
                    MainPage = new NavigationPage(new LoginView());
                else  MainPage = new NavigationPage(new ProfileView()); 

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
