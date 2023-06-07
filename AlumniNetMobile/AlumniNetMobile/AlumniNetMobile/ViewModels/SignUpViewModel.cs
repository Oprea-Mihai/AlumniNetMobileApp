using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.DTOs;
using AlumniNetMobile.Helpers;
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
            InvalidDataVisible = false;
            _manageData = new ManageData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            BorderColor = Color.DarkGray;
        }

        #endregion

        #region Private fields

        private readonly ManageData _manageData;
        private IAuthenticationService _authenticationService;

        #endregion

        #region Methods

        private void InvalidCredentials(string message)
        {
            BorderColor = Color.Red;
            InvalidDataMessage = message;
            InvalidDataVisible = true;
        }

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

        [ObservableProperty]
        private bool _invalidDataVisible;

        [ObservableProperty]
        private string _invalidDataMessage;

        [ObservableProperty]
        private Color _borderColor;

        #endregion


        #region Commands
        [RelayCommand]
        private async void OnSignUp()
        {
            InvalidDataVisible = false;
            BorderColor = Color.DarkGray;

            try
            {
                if (FirstName == string.Empty
                    || LastName == string.Empty
                    || UserEmail == string.Empty
                    || Password == string.Empty)
                    InvalidCredentials("Fields can not be empty");
                else
                if (await _authenticationService.CreateUser(FirstName + " " + LastName, UserEmail, Password))
                {
                    UserDTO newUser = new UserDTO();

                    newUser.FirstName = FirstName;
                    newUser.LastName = LastName;
                    newUser.Email = UserEmail;
                    newUser.IsValid = false;
                    newUser.DeviceNotificationToken = FCMTokenHelper.Token;
                    string json = JsonConvert.SerializeObject(newUser);
                    string token = await _authenticationService.GetCurrentTokenAsync();

                    _manageData.SetStrategy(new CreateData());
                    await _manageData.GetDataAndDeserializeIt<UserDTO>("User/AddUser", json, token);


                    await Application.Current.MainPage.Navigation.PushAsync(new Navigation());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                InvalidCredentials(ex.Message);
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
