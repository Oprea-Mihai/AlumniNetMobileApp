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
            { "Informatica",
            "Drept",
            "Magie"
            };

            _specializationNames = new List<string>
            {
                "Facultatea de stiinte economice si administrarea afacerilor",
                "Facultatea de silvicultura",
                "Facultatea de turism"
            };
            SearchedName = "";
            SearchedSpecialization = "";
            _areFacultySugestionsVisible = false;
            _areSpecializationSugestionsVisible = false;
        }

        public AddOrEditSpecializationViewModel(FinishedProgramModel selectedProgram)
        {
            _specializationNames= new List<string>
            { "Informatica",
            "Drept",
            "Magie"
            };

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

            _areSpecializationSugestionsVisible = false;
            _areFacultySugestionsVisible = false;
            SearchedName = SelectedProgram.FacultyName;
            SearchedSpecialization = SelectedProgram.Specialization;
        }

        #endregion

        #region Private fields

        private List<string> _facultyNames;
        private List<string> _specializationNames;

        #endregion


        #region Observables

        [ObservableProperty]
        private FinishedProgramModel _selectedProgram;

        [ObservableProperty]
        private List<string> _typesList;

        [ObservableProperty]
        private string _searchedName;

        [ObservableProperty]
        private string _searchedSpecialization;

        [ObservableProperty]
        private string _selectedFaculty;

        [ObservableProperty]
        private string _selectedSpecialization;

        [ObservableProperty]
        private bool _areFacultySugestionsVisible;

        [ObservableProperty]
        private bool _areSpecializationSugestionsVisible;



        public ObservableRangeCollection<string> _displayedSpecializations;

        public ObservableRangeCollection<string> DisplayedSpecializations
        {
            get { return _displayedSpecializations; }
            set { SetProperty(ref _displayedSpecializations, value); }
        }

        public ObservableRangeCollection<string> _displayedNames;

        public ObservableRangeCollection<string> DisplayedNames
        {
            get { return _displayedNames; }
            set { SetProperty(ref _displayedNames, value); }
        }
        #endregion



        #region Commands

        [RelayCommand]
        public void FacultyTextChanged()
        {
            var names = _facultyNames.Where(x => x.Contains(SearchedName))?.ToList();
            if (names.Count() != 0)
            {
                DisplayedNames = new ObservableRangeCollection<string>(names);
                AreFacultySugestionsVisible = true;
            }
            else
                DisplayedNames = new ObservableRangeCollection<string>();
        }

        [RelayCommand]
        public void SpecializationTextChanged()
        {
            var specializations = _specializationNames.Where(x => x.Contains(SearchedSpecialization))?.ToList();
            if (specializations.Count() != 0)
            {
                DisplayedSpecializations = new ObservableRangeCollection<string>(specializations);
                AreSpecializationSugestionsVisible = true;
            }
            else
                DisplayedSpecializations = new ObservableRangeCollection<string>();
        }

        [RelayCommand]
        public void FacultySelected()
        {
            if (SelectedFaculty == null)
                return;
            SearchedName = SelectedFaculty;
            SelectedFaculty = null;
            AreFacultySugestionsVisible = false;
        }

        [RelayCommand]
        public void SpecializationSelected()
        {
            if (SelectedSpecialization == null)
                return;
            SearchedSpecialization = SelectedSpecialization;
            SelectedSpecialization = null;
            AreSpecializationSugestionsVisible = false;
        }

        #endregion


    }
}
