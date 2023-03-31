using AlumniNetMobile.Common;
using AlumniNetMobile.Views;
using System;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public class AuthenticationViewModel
    {
        public AuthenticationViewModel()
        {
            CheckUserIsSignedIn();
        }

        private async void CheckUserIsSignedIn()
        {
            try
            {
                var authenticationService = DependencyService.Resolve<IAuthenticationService>();
                if (!authenticationService.IsSignedIn())
                    await Application.Current.MainPage.Navigation.PushAsync(new LoginView());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
