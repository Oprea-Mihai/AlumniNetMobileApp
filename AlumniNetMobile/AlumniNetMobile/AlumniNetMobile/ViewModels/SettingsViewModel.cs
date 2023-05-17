using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
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
            ResourceManager rm = new ResourceManager("TSEMobileApp.Resx.AppResource", typeof(AppResource).Assembly);
            ResourceSet resourceSet = rm.GetResourceSet(currentCulture, true, true);

            _manageData = new ManageData();

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

        #endregion

        #region Methods

        private async Task ChangeLanguage(string selectedLanguage)
        {
            try
            {

                _manageData.SetStrategy(new CreateData());
                var json = JsonConvert.SerializeObject(Employee);

                await _manageData.GetDataAndDeserializeIt<Employee>($"Employee/UpdateLanguage?employeeId={Employee.EmployeeId}&language={selectedLanguage}", json);

                CultureInfo language = new CultureInfo(selectedLanguage);
                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(selectedLanguage);
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
