using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.DTOs;
using AlumniNetMobile.Helpers;
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
            _manageData = new ManageData();
        }


        #region Private fields

        private readonly ManageData _manageData;
        private IAuthenticationService _authenticationService;

        #endregion

        private async void CheckUserIsSignedIn()
        {
            try
            {
                var authenticationService = DependencyService.Resolve<IAuthenticationService>();
                if (!authenticationService.IsSignedIn())
                {
                    if (FCMTokenHelper.Token != null && FCMTokenHelper.Token != "")
                    {
                        string token =await authenticationService.GetCurrentTokenAsync();
                        await _manageData.GetDataAndDeserializeIt<UserDTO>
                            ($"User/AddFCMTokenToUser?fcmToken={FCMTokenHelper.Token}", "", token);
                    }
                    await Application.Current.MainPage.Navigation.PushAsync(new LoginView());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
