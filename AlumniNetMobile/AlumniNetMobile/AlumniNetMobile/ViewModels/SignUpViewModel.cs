using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.DTOs;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class SignUpViewModel : ObservableObject
    {

        #region Constructors

        public SignUpViewModel()
        {
            ExceptionOccured = false;
            _manageData = new ManageData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
        }

        #endregion

        #region Private fields

        private readonly ManageData _manageData;
        private IAuthenticationService _authenticationService;

        #endregion

        #region Observables

        [ObservableProperty]
        private string _firstName;

        [ObservableProperty]
        private string _lastName;

        [ObservableProperty]
        private string _userEmail;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private bool _exceptionOccured;

        #endregion


        #region Commands
        [RelayCommand]
        private async void OnSignUp()
        {
            try
            {


                if (await _authenticationService.CreateUser(FirstName + " " + LastName, UserEmail, Password))
                {
                    UserDTO newUser = new UserDTO();

                    newUser.FirstName = FirstName;
                    newUser.LastName = LastName;
                    newUser.Email = UserEmail;
                    newUser.IsValid = false;

                    string json = JsonConvert.SerializeObject(newUser);
                    string token = await _authenticationService.GetCurrentTokenAsync();
                    _manageData.SetStrategy(new CreateData());
                    await _manageData.GetDataAndDeserializeIt<UserDTO>("User/AddUser", json, token);


                    await Application.Current.MainPage.Navigation.PushAsync(new Navigation());
                }
                else ExceptionOccured = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

        [RelayCommand]
        public async void SignIn()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new LoginView());
        }
        #endregion

    }
}
