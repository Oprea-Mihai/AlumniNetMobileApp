using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.DTOs;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        #region Constructor

        public SettingsViewModel(ProfileModel profile)
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

            FacebookURL = profile.Facebook;
            LinkedInURL = profile.LinkedIn;
            InstagramURL = profile.Instagram;

            RemoveFacebookVisible = FacebookURL != null && FacebookURL != string.Empty ? true : false;
            RemoveInstagramVisible = InstagramURL != null && InstagramURL != string.Empty ? true : false;
            RemoveLinkedInVisible = LinkedInURL != null && LinkedInURL != string.Empty ? true : false;

            CanNotUpdateVisible = false;
        }
        #endregion

        #region Private fields

        private IManageData _manageData;
        private IAuthenticationService _authenticationService;

        #endregion

        #region Methods

        private bool ValidateAndSanitizeUrl(string inputUrl, out string sanitizedUrl)
        {
            sanitizedUrl = string.Empty;


            Regex instagramRegex = new Regex(@"^(https?://)?(www\.)?instagram\.com/[a-zA-Z0-9_]+/$");
            Regex facebookRegex = new Regex(@"^(https?://)?(www\.)?facebook\.com/[a-zA-Z0-9.]+$");
            Regex linkedInRegex = new Regex(@"^(https?://)?([a-z]{2,3}\.)?linkedin\.com/(in|pub|company)/[a-zA-Z0-9-_/.]+$");

            if (!instagramRegex.IsMatch(inputUrl) &&
                !facebookRegex.IsMatch(inputUrl) &&
                !linkedInRegex.IsMatch(inputUrl))
            {
                return false;
            }


            if (Uri.TryCreate(inputUrl, UriKind.Absolute, out Uri uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            {
                sanitizedUrl = uri.ToString();
                return true;
            }

            return false;
        }

        #endregion

        #region Observables

        [ObservableProperty]
        private string _selectedLanguage;

        [ObservableProperty]
        private bool _saveChangesVisible;

        [ObservableProperty]
        private bool _canNotUpdateVisible;

        [ObservableProperty]
        private bool _removeFacebookVisible;

        [ObservableProperty]
        private bool _removeInstagramVisible;

        [ObservableProperty]
        private bool _removeLinkedInVisible;

        [ObservableProperty]
        private string _facebookURL;

        [ObservableProperty]
        private string _instagramURL;

        [ObservableProperty]
        private string _linkedInURL;

        [ObservableProperty]
        private bool _languageChanged;

        [ObservableProperty]
        private bool _socialChanged;

        [ObservableProperty]
        private ObservableCollection<string> _languageOptions;

        #endregion

        #region Commands

        [RelayCommand]
        private void FacebookEmpty()
        {
            FacebookURL = null;
        }

        [RelayCommand]
        private void LinkedInEmpty()
        {
            LinkedInURL = null;
        }

        [RelayCommand]
        private void InstagramEmpty()
        {
            InstagramURL = null;
        }

        [RelayCommand]
        private void ChangeLanguage()
        {
            SaveChangesVisible = true;
            LanguageChanged = true;
        }

        [RelayCommand]
        private void FacebookLinkChanged()
        {
            if (FacebookURL != null && FacebookURL != string.Empty)
                RemoveFacebookVisible = true;
            else RemoveFacebookVisible = false;
            SaveChangesVisible = true;
            SocialChanged = true;
        }

        [RelayCommand]
        private void LinkedInLinkChanged()
        {
            if (LinkedInURL != null && LinkedInURL != string.Empty)
                RemoveLinkedInVisible = true;
            else RemoveLinkedInVisible = false;
            SaveChangesVisible = true;
            SocialChanged = true;
        }


        [RelayCommand]
        private void InstagramLinkChanged()
        {
            if (InstagramURL != null && InstagramURL != string.Empty)
                RemoveInstagramVisible = true;
            else RemoveInstagramVisible = false;
            SaveChangesVisible = true;
            SocialChanged = true;
        }

        [RelayCommand]
        public async Task SaveChanges()
        {
            bool canUpdate = true;


            if (SocialChanged)
            {
                //to sanitize url
                string facebookSanitizedUrl = "";
                string instagramSanitizedUrl = "";
                string linkedInSanitizedUrl = "";

                if (LinkedInURL != null && LinkedInURL != string.Empty)
                {
                    canUpdate = ValidateAndSanitizeUrl(LinkedInURL, out linkedInSanitizedUrl);
                }
                if (FacebookURL != null && FacebookURL != string.Empty && canUpdate)
                {
                    canUpdate = ValidateAndSanitizeUrl(FacebookURL, out facebookSanitizedUrl);
                }

                if (InstagramURL != null && InstagramURL != string.Empty && canUpdate)
                {
                    canUpdate = ValidateAndSanitizeUrl(InstagramURL, out instagramSanitizedUrl);
                }

                if (canUpdate)
                {
                    string token = await _authenticationService.GetCurrentTokenAsync();

                    _manageData.SetStrategy(new UpdateData());
                    await _manageData.GetDataAndDeserializeIt<string>($"Profile/UpdateSocialMediaLinksByUserId?" +
                        $"instagram={instagramSanitizedUrl}&facebook={facebookSanitizedUrl}&linkedIn={linkedInSanitizedUrl}", "", token);
                }
                else CanNotUpdateVisible = true;
            }

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


                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            if (canUpdate)
                Device.BeginInvokeOnMainThread(() =>
                           {
                               Application.Current.MainPage = new NavigationPage(new Navigation());
                           });
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
