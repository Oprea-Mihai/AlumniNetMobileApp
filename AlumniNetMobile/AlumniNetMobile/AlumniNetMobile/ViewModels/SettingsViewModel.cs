using AlumniNetMobile.Common;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        #region Constructor
        public SettingsViewModel()
        {
            LanguageOptions = new ObservableCollection<string>()
            {
               "English",
               "Romanian"
            };
        }
        #endregion

        #region Private fields
        #endregion

        #region Methods
        #endregion

        #region Observables

        [ObservableProperty]
        private string _selectedLanguage;

        [ObservableProperty] 
        private ObservableCollection<string> _languageOptions;

        #endregion

        #region Commands
        [RelayCommand]
        private void ChangeLanguage()
        {
            if (_selectedLanguage != null) { }
        }

        [RelayCommand]
        public void SignOut()
        {
            var authService = DependencyService.Resolve<IAuthenticationService>();
            authService.SignOut();

            var newNavigationPage = new NavigationPage(new LoginView());
            Application.Current.MainPage = newNavigationPage;
        }
        #endregion
    }
}
