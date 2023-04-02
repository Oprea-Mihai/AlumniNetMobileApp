﻿using AlumniNetMobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class AddOrEditSpecializationViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {

        #region Constructors
        public AddOrEditSpecializationViewModel()
        {

            _specializationNames = new List<string>
            { "Informatica",
            "Drept",
                "Magie"
            };

            TypesList = new List<string>
            {
                "ZI",
                "IFR"
            };

            _facultyNames = new List<string>
            {
                "Facultatea de stiinte economice si administrarea afacerilor",
                "Facultatea de silvicultura",
                "Facultatea de turism"
            };
            SearchedName = "";
            SearchedSpecialization = "";
            _areFacultySugestionsVisible = false;
            _areSpecializationSugestionsVisible = false;
            SelectedSchedule = null;
            SelectedStudyProgram = null;
            GraduationYear = null;
            wasFacultyTextChanged = wasSpecializationTextChanged = false;
            WasFacultySelected = false;
        }

        public AddOrEditSpecializationViewModel(FinishedProgramModel selectedProgram)
        {
            _specializationNames = new List<string>
            { "Informatica",
            "Informatica economica",
            "Drept",
            "Magie"
            };

            _facultyNames = new List<string>
            {
                "Facultatea de stiinte economice si administrarea afacerilor",
                "Facultatea de silvicultura",
                "Facultatea de turism"
            };

            _selectedProgram = selectedProgram;

            TypesList = new List<string>
            {
                "ZI",
                "IFR"
            };

            _areSpecializationSugestionsVisible = false;
            _areFacultySugestionsVisible = false;
            SearchedName = _selectedProgram.FacultyName;
            SearchedSpecialization = _selectedProgram.Specialization;
            SelectedSchedule = selectedProgram.LearningSchedule;
            SelectedStudyProgram = selectedProgram.Program;
            GraduationYear = selectedProgram.GraduationYear;
            wasFacultyTextChanged = wasSpecializationTextChanged = false;
            WasFacultySelected = false;
        }

        #endregion

        #region Private fields

        private List<string> _facultyNames;
        private List<string> _specializationNames;
        private FinishedProgramModel _selectedProgram;
        private bool wasFacultyTextChanged;
        private bool wasSpecializationTextChanged;

        #endregion


        #region Observables

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
        private string _selectedSchedule;

        [ObservableProperty]
        private string _selectedStudyProgram;

        [ObservableProperty]
        private bool _areFacultySugestionsVisible;

        [ObservableProperty]
        private bool _areSpecializationSugestionsVisible;

        [ObservableProperty]
        private int? _graduationYear;

        [ObservableProperty]
        private bool _specializationNotFoundVisible;

        [ObservableProperty]
        private bool _facultyNotFoundVisible;

        [ObservableProperty]
        private bool _wasFacultySelected;



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
            wasFacultyTextChanged = true;
        }

        [RelayCommand]
        public void SearchFaculty()
        {
            FacultyNotFoundVisible = false;
            WasFacultySelected = false;

            if (wasFacultyTextChanged)
            {
                var names = _facultyNames.Where(x => x.ToLower().Contains(SearchedName.ToLower()))?.ToList();
                if (names.Count() != 0)
                {
                    DisplayedNames = new ObservableRangeCollection<string>(names);
                    AreFacultySugestionsVisible = true;
                }
                else
                {
                    AreFacultySugestionsVisible = false;
                    FacultyNotFoundVisible = true;
                }
            }
        }

        [RelayCommand]
        public void SpecializationTextChanged()
        {
            wasSpecializationTextChanged = true;
        }

        [RelayCommand]
        public void SearchSpecialization()
        {
            SpecializationNotFoundVisible = false;

            if (wasSpecializationTextChanged)
            {
                var specializations = _specializationNames.Where(x => x.ToLower().Contains(SearchedSpecialization.ToLower()))?.ToList();
                if (specializations.Count() != 0)
                {
                    DisplayedSpecializations = new ObservableRangeCollection<string>(specializations);
                    AreSpecializationSugestionsVisible = true;
                }
                else
                {
                    AreSpecializationSugestionsVisible = false;
                    SpecializationNotFoundVisible = true;
                }
            }
        }

        [RelayCommand]
        public void FacultySelected()
        {
            if (SelectedFaculty == null)
                return;
            SearchedName = SelectedFaculty;
            SelectedFaculty = null;
            AreFacultySugestionsVisible = false;
            wasFacultyTextChanged = false;
            WasFacultySelected = true;
        }

        [RelayCommand]
        public void SpecializationSelected()
        {
            if (SelectedSpecialization == null)
                return;
            SearchedSpecialization = SelectedSpecialization;
            SelectedSpecialization = null;
            AreSpecializationSugestionsVisible = false;
            wasSpecializationTextChanged = false;

        }

        [RelayCommand]
        public void Cancel()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }

        [RelayCommand]
        public void Save()
        {
            Application.Current.MainPage.Navigation.PopAsync();
        }
        #endregion


    }
}
