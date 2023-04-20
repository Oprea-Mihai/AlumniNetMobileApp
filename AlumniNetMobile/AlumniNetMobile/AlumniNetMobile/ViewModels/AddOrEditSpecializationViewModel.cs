using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class AddOrEditSpecializationViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {

        #region Constructors
        public AddOrEditSpecializationViewModel()
        {
            CommonInitialization();

            _programToUpdate = new FinishedProgramModel();
            SearchedFacultyName = "";
            SearchedSpecialization = "";
            SelectedSchedule = null;
            SelectedStudyProgram = null;
            GraduationYear = null;
            WasFacultySelected = false;
            IsDeleteButtonVisible = false;
        }

        public AddOrEditSpecializationViewModel(FinishedProgramModel selectedProgram)
        {
            CommonInitialization();

            _programToUpdate = selectedProgram;
            SearchedFacultyName = selectedProgram.FacultyName;
            SearchedSpecialization = selectedProgram.Specialization;
            SelectedSchedule = selectedProgram.LearningSchedule;
            SelectedStudyProgram = selectedProgram.Program;
            GraduationYear = selectedProgram.GraduationYear;
            WasFacultySelected = true;
            IsDeleteButtonVisible = true;
        }

        #endregion

        #region Private fields

        private List<string> _facultyNames;
        private List<string> _specializationNames;
        private FinishedProgramModel _programToUpdate;
        private bool wasFacultyTextChanged;
        private bool wasSpecializationTextChanged;
        private ManageData _manageData;
        private IAuthenticationService _authenticationService;
        private List<StudyProgramModel> _studyPrograms;
        private List<LearningScheduleModel> _learningSchedules;

        #endregion


        #region Observables


        [ObservableProperty]
        private bool _isDeleteButtonVisible;

        [ObservableProperty]
        private string _searchedFacultyName;

        [ObservableProperty]
        private string _searchedSpecialization;

        [ObservableProperty]
        private FacultyModel _selectedFaculty;

        [ObservableProperty]
        private SpecializationModel _selectedSpecialization;

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

        public ObservableRangeCollection<string> _schedulesToDisplay;

        public ObservableRangeCollection<string> SchedulesToDisplay
        {
            get { return _schedulesToDisplay; }
            set
            {
                SetProperty(ref _schedulesToDisplay, value);
            }
        }

        public ObservableRangeCollection<string> _studyProgramsToDisplay;

        public ObservableRangeCollection<string> StudyProgramsToDisplay
        {
            get { return _studyProgramsToDisplay; }
            set { SetProperty(ref _studyProgramsToDisplay, value); }
        }

        public ObservableRangeCollection<SpecializationModel> _displayedSpecializations;

        public ObservableRangeCollection<SpecializationModel> DisplayedSpecializations
        {
            get { return _displayedSpecializations; }
            set { SetProperty(ref _displayedSpecializations, value); }
        }

        public ObservableRangeCollection<FacultyModel> _displayedFacultyNames;

        public ObservableRangeCollection<FacultyModel> DisplayedFacultyNames
        {
            get { return _displayedFacultyNames; }
            set { SetProperty(ref _displayedFacultyNames, value); }
        }
        #endregion

        #region Private methods
        private async void CommonInitialization()
        {
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            _manageData = new ManageData();
            _manageData.SetStrategy(new GetData());
            SchedulesToDisplay = new ObservableRangeCollection<string>();
            StudyProgramsToDisplay = new ObservableRangeCollection<string>();
            _learningSchedules = new List<LearningScheduleModel>();
            _studyPrograms = new List<StudyProgramModel>();
            string token = await _authenticationService.GetCurrentTokenAsync();

            _learningSchedules = await _manageData.GetDataAndDeserializeIt<List<LearningScheduleModel>>
                ($"LearningSchedule/GetAllLearningSchedules", "", token);
            SchedulesToDisplay.ReplaceRange(_learningSchedules.Select(x=>x.ScheduleName));

            _studyPrograms= await _manageData.GetDataAndDeserializeIt<List<StudyProgramModel>>
                ($"StudyProgram/GetAllStudyPrograms", "", token);
            StudyProgramsToDisplay.ReplaceRange(_studyPrograms.Select(x => x.ProgramName));

            _specializationNames = new List<string>();
            _facultyNames = new List<string>();
            _areFacultySugestionsVisible = false;
            _areSpecializationSugestionsVisible = false;
            wasFacultyTextChanged = wasSpecializationTextChanged = false;
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
            if (wasFacultyTextChanged == true || _programToUpdate == null)
            {
                WasFacultySelected = false;
                SelectedFaculty = null;
            }

            if (wasFacultyTextChanged)
            {
                _manageData.SetStrategy(new GetData());
                List<FacultyModel> names = (await _manageData.GetDataAndDeserializeIt<List<FacultyModel>>
                    ($"Faculty/GetFacultiesSearchSuggestions?searchedString={SearchedFacultyName}"));
                if (names.Count() != 0)
                {
                    DisplayedFacultyNames = new ObservableRangeCollection<FacultyModel>(names);
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
        public async void FacultySelected()
        {
            if (SelectedFaculty == null)
                return;
            SearchedFacultyName = SelectedFaculty.FacultyName;
            _programToUpdate.FacultyName = SelectedFaculty.FacultyName;
            _programToUpdate.FacultyId = SelectedFaculty.FacultyId;
            SelectedFaculty = null;
            AreFacultySugestionsVisible = false;
            wasFacultyTextChanged = false;
            WasFacultySelected = true;
        }

        [RelayCommand]
        public void SpecializationTextChanged()
        {
            wasSpecializationTextChanged = true;
        }

        [RelayCommand]
        public async void SearchSpecialization()
        {
            SpecializationNotFoundVisible = false;

            if (wasSpecializationTextChanged)
            {
                string token = await _authenticationService.GetCurrentTokenAsync();
                _manageData.SetStrategy(new GetData());

                List<SpecializationModel> specializations = (await _manageData.GetDataAndDeserializeIt<List<SpecializationModel>>
                    ($"Specialization/GetSpecializationsByFacultyAndSearchString" +
                    $"?facultyId={_programToUpdate.FacultyId}" +
                    $"&searchedString={SearchedSpecialization}", "", token));

                if (specializations.Count() != 0)
                {
                    DisplayedSpecializations = new ObservableRangeCollection<SpecializationModel>(specializations);
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
        public void SpecializationSelected()
        {
            if (SelectedSpecialization == null)
                return;
            SearchedSpecialization = SelectedSpecialization.SpecializationName;
            _programToUpdate.SpecializationId = SelectedSpecialization.SpecializationId;
            _programToUpdate.Specialization = SelectedSpecialization.SpecializationName;
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
        public async void Save()
        {
            string token = await _authenticationService.GetCurrentTokenAsync();
            _manageData.SetStrategy(new UpdateData());
            string json = JsonConvert.SerializeObject(_programToUpdate);
            await _manageData.GetDataAndDeserializeIt<FinishedProgramModel>($"FinishedStudy/UpdateFinishedStudy", json, token);
            await Application.Current.MainPage.Navigation.PopAsync();
            if (SelectedSchedule != null)
            {
                _programToUpdate.LearningSchedule = SelectedSchedule;
                _programToUpdate.LearningScheduleId = _learningSchedules
                    .First(x=>x.ScheduleName==SelectedSchedule).LearningScheduleId;
            }
            if(SelectedStudyProgram!=null)
            {   _programToUpdate.Program= SelectedStudyProgram;
                _programToUpdate.StudyProgramId = _studyPrograms
                    .First(x => x.ProgramName == SelectedStudyProgram).StudyProgramId;
            }
            _programToUpdate.GraduationYear = (int)GraduationYear;
        }
        #endregion

       
    }
}
