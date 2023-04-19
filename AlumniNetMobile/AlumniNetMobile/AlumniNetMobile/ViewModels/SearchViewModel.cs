using AlumniNetMobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using Xamarin.CommunityToolkit.ObjectModel;

namespace AlumniNetMobile.ViewModels
{
    public partial class SearchViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        #region Constructors
        public SearchViewModel()
        {
            _users = new List<SearchUserModel>
            {
                new SearchUserModel
                {FacultyName="Facultatea de stiinte economice si administrarea afacerilor",
                FirstName="Oprea",
                LastName="Mihai-Lucian"},
                 new SearchUserModel
                 {FacultyName="Facultatea de stiinte economice si administrarea afacerilor",
                FirstName="Cojocaru",
                LastName="Andrei"},
                 new SearchUserModel
                 {FacultyName="Facultatea de stiinte economice si administrarea afacerilor",
                FirstName="Sorojan",
                LastName="Marian"},
                   new SearchUserModel
                   {FacultyName="Facultatea de stiinte economice si administrarea afacerilor",
                FirstName="Antal",
                LastName="Claudiu"}
            };
            _recentSearches = new List<SearchUserModel>(_users);
            SearchResults = new ObservableRangeCollection<SearchUserModel>();
            AreRecentSearchesVisible = true;
            SearchResults.ReplaceRange(_users);
        }
        #endregion

        #region Private fields

        private List<SearchUserModel> _users;
        private List<SearchUserModel> _recentSearches;


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

        #endregion

        #region Commands       
        [RelayCommand]
        public void Search()
        {
            AreRecentSearchesVisible = false;
            SearchResults.ReplaceRange(_users.Where
                ((System.Func<SearchUserModel, bool>)(x => x.FirstName.ToLower().Contains((string)this.SearchedName.ToLower()) ||
                x.LastName.ToLower().Contains((string)this.SearchedName.ToLower()))).Select(x => new SearchUserModel
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    FacultyName = char.ToUpper((x.FacultyName.Substring(14))[0]) + x.FacultyName.Substring(15)
                })); ;
        }

        [RelayCommand]
        public void SearchBarTextChanged()
        {
            if (SearchedName.Length == 0) { 
            AreRecentSearchesVisible = true;
            SearchResults.ReplaceRange(_recentSearches);
            }
        }

        [RelayCommand]
        public void UserSelected()
        {

        }

        #endregion
    }
}
