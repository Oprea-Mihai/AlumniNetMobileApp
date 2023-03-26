using AlumniNetMobileApp.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class LoginViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [RelayCommand]
        public async void SignIn()
        {
            //await Application.Current.MainPage.Navigation.PushAsync(new )
        }

        [RelayCommand]
        public void ForgotPassword()
        {

        }

        [RelayCommand]
        public async void SignUp()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SignUpView());
        }

    }
}
