using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.Models;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class SearchViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        #region Constructors
        public SearchViewModel()
        {
            _manageData = new ManageData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();

            _users = new List<SearchUserModel>();
            _recentSearches = new List<SearchUserModel>(_users);
            SearchResults = new ObservableRangeCollection<SearchUserModel>();
            AreRecentSearchesVisible = true;
            SearchResults.ReplaceRange(_users);
        }
        #endregion

        #region Private fields

        private List<SearchUserModel> _users;
        private List<SearchUserModel> _recentSearches;
        private IManageData _manageData;
        private IAuthenticationService _authenticationService;


        #endregion

        #region Methods
        private async Task GetResults(string searchedString)
        {
            if (IsBusy) return;
            IsBusy = true;
            string token = await _authenticationService.GetCurrentTokenAsync();
            _manageData.SetStrategy(new GetData());
            _users = await _manageData.GetDataAndDeserializeIt<List<SearchUserModel>>
                ($"User/GetUserSearchResults?searchedString={searchedString}", "", token);
            foreach (SearchUserModel user in _users)
            {
                if (user.ProfilePicture != string.Empty)
                { GetData getData = new GetData();
                    Stream file = await getData.ManageStreamData($"Files/GetFileByKey?key={user.ProfilePicture}", token);
                    user.ImageSource = ImageSource.FromStream(() => file);
                }
                else user.ImageSource = ImageSource.FromFile("user.png");
            }
            IsBusy = false;
        }
        #endregion

        #region Observables

        private ObservableRangeCollection<SearchUserModel> _searchResults;
        public ObservableRangeCollection<SearchUserModel> SearchResults
        {
            get { return _searchResults; }
            set { SetProperty(ref _searchResults, value); }
        }

        [ObservableProperty]
        private string _searchedName;

        [ObservableProperty]
        private bool _areRecentSearchesVisible;

        [ObservableProperty]
        private SearchUserModel _selectedUser;

        [ObservableProperty]
        private bool _isBusy;

        #endregion

        #region Commands       
        [RelayCommand]
        public async void Search()
        {
            AreRecentSearchesVisible = false;
            await GetResults(SearchedName);
            SearchResults.ReplaceRange(_users);
        }

        [RelayCommand]
        public void SearchBarTextChanged()
        {
            if (SearchedName.Length == 0)
            {
                AreRecentSearchesVisible = true;
                SearchResults.ReplaceRange(_recentSearches);
            }
        }

        [RelayCommand]
        public async void UserSelected()
        {
            if (SelectedUser == null)
            {
                return;
            }
            SearchUserModel selected = SelectedUser;
            SelectedUser = null;
            await Application.Current.MainPage.Navigation.PushAsync(new VisitedProfileView(selected.ProfileId));
        }

        #endregion
    }
}
