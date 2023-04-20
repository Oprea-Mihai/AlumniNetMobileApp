using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.Models;
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
            _specializationNames = new List<string>();
            _facultyNames = new List<string>();
            TypesList = new List<string>();

            _manageData = new ManageData();

            SearchedFacultyName = "";
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
            _specializationNames = new List<string>();
            _facultyNames = new List<string>();
            TypesList = new List<string>();

            _selectedProgram = selectedProgram;

            _manageData = new ManageData();

            _areSpecializationSugestionsVisible = false;
            _areFacultySugestionsVisible = false;
            SearchedFacultyName = _selectedProgram.FacultyName;
            SearchedSpecialization = _selectedProgram.Specialization;
            SelectedFaculty = selectedProgram.FacultyName;
            SelectedSchedule = selectedProgram.LearningSchedule;
            SelectedStudyProgram = selectedProgram.Program;
            GraduationYear = selectedProgram.GraduationYear;
            wasFacultyTextChanged = wasSpecializationTextChanged = false;
            WasFacultySelected = true;
        }

        #endregion

        #region Private fields

        private List<string> _facultyNames;
        private List<string> _specializationNames;
        private FinishedProgramModel _selectedProgram;
        private bool wasFacultyTextChanged;
        private bool wasSpecializationTextChanged;
        private ManageData _manageData;

        #endregion


        #region Observables

        [ObservableProperty]
        private List<string> _typesList;

        [ObservableProperty]
        private string _searchedFacultyName;

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

        public ObservableRangeCollection<string> _displayedFacultyNames;

        public ObservableRangeCollection<string> DisplayedFacultyNames
        {
            get { return _displayedFacultyNames; }
            set { SetProperty(ref _displayedFacultyNames, value); }
        }
        #endregion



        #region Commands

        [RelayCommand]
        public void FacultyTextChanged()
        {
            wasFacultyTextChanged = true;
        }

        [RelayCommand]
        public async void SearchFaculty()
        {
            FacultyNotFoundVisible = false;
            if (wasFacultyTextChanged == true || _selectedProgram == null)
            {
                WasFacultySelected = false;
                SelectedFaculty = null;
            }

            if (wasFacultyTextChanged)
            {
                _manageData.SetStrategy(new GetData());
                List<string> names = (await _manageData.GetDataAndDeserializeIt<List<string>>
                    ($"Faculty/GetFacultiesSearchSuggestions?searchedString={SearchedFacultyName}"));
                if (names.Count() != 0)
                {
                    DisplayedFacultyNames = new ObservableRangeCollection<string>(names);
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
            SearchedFacultyName = SelectedFaculty;
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
