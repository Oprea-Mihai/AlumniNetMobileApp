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
                    Connectivity.ConnectivityChanged += OnConnectivityChanged;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                var authenticationService = DependencyService.Resolve<IAuthenticationService>();
                if (!authenticationService.IsSignedIn())
                    MainPage = new NavigationPage(new LoginView());
                else
                    MainPage = new NavigationPage(new Navigation());
            }
            else
            {
                MainPage = new NavigationPage(new NoConnection());
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
