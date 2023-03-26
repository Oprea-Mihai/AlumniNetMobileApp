using AlumniNetMobile.Common;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class SignUpViewModel:ObservableObject
    {
        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private bool _exceptionOccured;

        public SignUpViewModel()
        {
            ExceptionOccured = false;
        }

        [RelayCommand]
        private async void OnSignUp()
        {
            var authService=DependencyService.Resolve<IAuthenticationService>();
            
            if(await authService.CreateUser(Username,Email,Password))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new ProfileView());
            }
            else ExceptionOccured = true;
                
        }
    }
}
