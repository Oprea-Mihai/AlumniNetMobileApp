using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.DTOs;
using AlumniNetMobile.Helpers;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {

        #region Constructors
        public LoginViewModel()
        {
            BorderColor = Color.DarkGray;
            _manageData = new ManageData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
        }

        #endregion

        #region Private fields

        private readonly ManageData _manageData;
        private IAuthenticationService _authenticationService;

        #endregion

        #region Methods
        #endregion

        #region Observables

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private bool _invalidDataVisible;
        [ObservableProperty]
        private Color _borderColor;

        #endregion

        #region Commands

        [RelayCommand]
        public async void SignIn()
        {
            try
            {
                BorderColor = Color.DarkGray;
                InvalidDataVisible = false;
                var authService = DependencyService.Resolve<IAuthenticationService>();
                var token = await authService.SignIn(Email, Password);
                
                if (FCMTokenHelper.Token!=null&&FCMTokenHelper.Token!="")
                    await _manageData.GetDataAndDeserializeIt<UserDTO>
                        ($"User/AddFCMTokenToUser?fcmToken={FCMTokenHelper.Token}", "", token);

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

        #endregion


    }
}
