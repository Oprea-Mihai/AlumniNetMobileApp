using AlumniNetMobile.Common;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class ForgotPasswordViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _email;

        [RelayCommand]
        public async void ResetPassword()
        {
            try
            {
                var authService = DependencyService.Resolve<IAuthenticationService>();
                await authService.ResetPassword(Email);

                await Application.Current.MainPage.DisplayAlert("Password Reset", "Password recovery sent, check your email", "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                await Application.Current.MainPage.DisplayAlert("Password Reset", "An error has occurred. Please verify the email address provided ", "OK");
            }
           
        }

        [RelayCommand]
        public async void SignUp()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SignUpView());
        }
    }
}
