using AlumniNetMobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.ObjectModel;

namespace AlumniNetMobile.ViewModels
{
    public partial class AddOrEditSpecializationViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {

        #region Constructors
        public AddOrEditSpecializationViewModel()
        {
            _facultyNames = new List<string>
            {
                "AAAAAAAAA",
                "Facultatea de silvicultura",
                "Facultatea de turism"
            };
            SearchedName = "";
        }

        public AddOrEditSpecializationViewModel(FinishedProgramModel selectedProgram)
        {
            _facultyNames = new List<string>
            {
                "Facultatea de stiinte economice si administrarea afacerilor",
                "Facultatea de silvicultura",
                "Facultatea de turism"
            };

            SelectedProgram = selectedProgram;
            
            TypesList = new List<string>
            {
                "ZI",
                "IFR"
            };


            SearchedName = SelectedProgram.FacultyName;
        }

        #endregion

        #region Private fields

        private List<string> _facultyNames;

        #endregion


        #region Observables

        [ObservableProperty]
        private FinishedProgramModel _selectedProgram;
       
        [ObservableProperty]
        private List<string> _typesList;

        [ObservableProperty]
        private string _searchedName;

        

        public ObservableRangeCollection<string> _displayedNames;

        public ObservableRangeCollection<string> DisplayedNames
        {
            get { return _displayedNames; }
            set { SetProperty(ref _displayedNames, value); }
        }
        #endregion



        #region Commands

        [RelayCommand]
        public void TextChanged()
        {
            var a = _facultyNames.Where(x => x.Contains(SearchedName));
           DisplayedNames =new ObservableRangeCollection<string>(_facultyNames.Where(x => x.Contains(SearchedName)));
        }

        #endregion


    }
}
