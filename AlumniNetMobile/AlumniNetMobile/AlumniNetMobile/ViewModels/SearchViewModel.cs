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
            _users = new List<UserModel>
            {
                new UserModel
                {FacultyName="Facultatea de stiinte economice si administrarea afacerilor",
                FirstName="Oprea",
                LastName="Mihai-Lucian"},
                 new UserModel
                 {FacultyName="Facultatea de stiinte economice si administrarea afacerilor",
                FirstName="Cojocaru",
                LastName="Andrei"},
                 new UserModel
                 {FacultyName="Facultatea de stiinte economice si administrarea afacerilor",
                FirstName="Sorojan",
                LastName="Marian"},
                   new UserModel
                   {FacultyName="Facultatea de stiinte economice si administrarea afacerilor",
                FirstName="Antal",
                LastName="Claudiu"}
            };
        }
        #endregion

        #region Private fields

        private List<UserModel> _users;

        #endregion

        #region Observables
        ObservableRangeCollection<UserModel> SearchResults;

        [ObservableProperty]
        private string _searchedName;

        #endregion

        #region Commands       
        [RelayCommand]
        public void Search()
        {

            SearchResults.ReplaceRange(_users.Where(x => x.FirstName.Contains(SearchedName)|| x.LastName.Contains(SearchedName)));
        }
        #endregion
    }
}
