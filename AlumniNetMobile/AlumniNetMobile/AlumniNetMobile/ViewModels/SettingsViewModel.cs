using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.Resx;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        #region Constructor
        public SettingsViewModel()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            ResourceManager rm = new ResourceManager("AlumniNetMobile.Resx.AppResource", typeof(AppResource).Assembly);
            ResourceSet resourceSet = rm.GetResourceSet(currentCulture, true, true);

            _manageData = new ManageData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            _languageOptions = new ObservableCollection<string>()
            {
               "English",
               "Romanian"
            };
            SelectedLanguage = resourceSet.GetString("SelectLanguage");

        }
        #endregion

        #region Private fields

        private IManageData _manageData;
        private IAuthenticationService _authenticationService;

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
        private async Task ChangeLanguage()
        {
            if (SelectedLanguage != null)
            {
                try
                {
                    string token = await _authenticationService.GetCurrentTokenAsync();

                    _manageData.SetStrategy(new UpdateData());

                    await _manageData.GetDataAndDeserializeIt<string>($"User/ChangeUserLanguage?language={SelectedLanguage}", "", token);

                    CultureInfo language = new CultureInfo(SelectedLanguage);
                    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(SelectedLanguage);
                    AppResource.Culture = language;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage = new NavigationPage(new Navigation());
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
        #endregion
    }
}
