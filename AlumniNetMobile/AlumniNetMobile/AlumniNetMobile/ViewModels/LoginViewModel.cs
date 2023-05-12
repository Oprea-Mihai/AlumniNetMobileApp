using AlumniNetMobile.Common;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private bool _invalidDataVisible;
        [ObservableProperty]
        private Color _borderColor;

        public LoginViewModel()
        {
            BorderColor = Color.DarkGray;
        }

        [RelayCommand]
        public async void SignIn()
        {
            try
            {
                BorderColor = Color.DarkGray;
                InvalidDataVisible = false;
                var authService = DependencyService.Resolve<IAuthenticationService>();
                var token = await authService.SignIn(Email, Password);
                await Application.Current.MainPage.Navigation.PushAsync(new Navigation());
            }
            catch (Exception ex)
            {
                BorderColor = Color.Red;
                InvalidDataVisible = true;
                Console.WriteLine(ex.Message);
            }

        }

        [RelayCommand]
        public async void SignUp()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SignUpView());
        }

        [RelayCommand]
        private async void ForgotPassword()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new ForgotPasswordView());
        }
    }
}
