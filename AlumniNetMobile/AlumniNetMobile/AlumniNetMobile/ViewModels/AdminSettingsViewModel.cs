using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.DTOs;
using AlumniNetMobile.Resx;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class AdminSettingsViewModel : ObservableObject
    {
        #region Constructors
        public AdminSettingsViewModel()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;

            ResourceManager rm = new ResourceManager("AlumniNetMobile.Resx.AppResource", typeof(AppResource).Assembly);
            ResourceSet resourceSet = rm.GetResourceSet(currentCulture, true, true);

            _manageData = new ManageData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            _languageOptions = new ObservableCollection<string>()
            {
               resourceSet.GetString("Romanian"),
               resourceSet.GetString("English")
            };
            SelectedLanguage = currentCulture.DisplayName;
            LanguageChanged = false;
            SaveChangesVisible = false;

        }
        #endregion

        #region Private fields
        private ManageData _manageData;
        private IAuthenticationService _authenticationService;
        private UserDTO _user;
        #endregion

        #region Methods

        #endregion

        #region Observables

        [ObservableProperty]
        private string _selectedLanguage;

        [ObservableProperty]
        private bool _saveChangesVisible;
        [ObservableProperty]
        private bool _languageChanged;

        [ObservableProperty]
        private ObservableCollection<string> _languageOptions;

        #endregion

        #region Commands

        [RelayCommand]
        private void ChangeLanguage()
        {
            SaveChangesVisible = true;
            LanguageChanged = true;
        }

        [RelayCommand]
        public async Task SaveChanges()
        {

            if (LanguageChanged)
                if (SelectedLanguage != null)
                {
                    try
                    {
                        string token = await _authenticationService.GetCurrentTokenAsync();

                        _manageData.SetStrategy(new UpdateData());

                        await _manageData.GetDataAndDeserializeIt<string>($"User/ChangeUserLanguage?language={SelectedLanguage}", "", token);

                        string languageString = "";
                        switch (SelectedLanguage)
                        {
                            case "Romanian":
                                languageString = "RO";
                                break;
                            case "English":
                                languageString = "EN";
                                break;
                            default:
                                break;
                        }

                        CultureInfo language = new CultureInfo(languageString);
                        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(languageString);
                        AppResource.Culture = language;

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Application.Current.MainPage = new NavigationPage(new AdminNavigation());
                        });
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

        }

        [RelayCommand]
        public void SignOut()
        {
            var authService = DependencyService.Resolve<IAuthenticationService>();
            authService.SignOut();

            var newNavigationPage = new NavigationPage(new LoginView());
            Application.Current.MainPage = newNavigationPage;
        }

        [RelayCommand]
        private async Task PageAppearing()
        {
            string token = await _authenticationService.GetCurrentTokenAsync();
            _manageData.SetStrategy(new GetData());
            _user = await _manageData.GetDataAndDeserializeIt<UserDTO>("User/GetUserById", "", token);
        }

        #endregion
    }
}
