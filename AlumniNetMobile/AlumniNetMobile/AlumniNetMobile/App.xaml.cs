using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.DTOs;
using AlumniNetMobile.Views;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlumniNetMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InitializeAppAsync();
        }
        private async void InitializeAppAsync()
        {
            try
            {
                if (Connectivity.NetworkAccess <= NetworkAccess.ConstrainedInternet)
                {
                    MainPage = new NavigationPage(new NoConnection());
                }
                else
                {
                    var authenticationService = DependencyService.Resolve<IAuthenticationService>();
                    if (!authenticationService.IsSignedIn())
                    {
                        MainPage = new NavigationPage(new LoginView());
                    }
                    else
                    {
                        MainPage = new NavigationPage(new Navigation());
                    }
                }
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
