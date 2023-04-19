using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.DTOs;
using AlumniNetMobile.Models;
using AlumniNetMobile.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class ProfileViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        #region Constructors

        public ProfileViewModel()
        {
            _manageData = new ManageData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            Programs = new ObservableRangeCollection<FinishedProgramModel>();
            FinishedProgramModel programModel = new FinishedProgramModel
            {
                FacultyName = "Facultatea de stiinte economice si administrarea afacerilor",
                Specialization = "Informatica economica",
                LearningSchedule = "Zi",
                GraduationYear = 2020,
                Program = "Licenta"
            };

            FinishedProgramModel programModel2 = new FinishedProgramModel
            {
                FacultyName = "Facultatea de Turism",
                Specialization = "Turism",
                LearningSchedule = "Zi",
                GraduationYear = 2018,
                Program = "Licenta"
            };
            Programs.Add(programModel);
            Programs.Add(programModel2);

            Jobs = new ObservableRangeCollection<ExperienceModel>();

            //JobModel jobModel = new JobModel
            //{
            //    CompanyName = "Talenting Software",
            //    JobTitle = "Software Engineer",
            //    StartEndDate = "2018 - Prezent"
            //};

            //Jobs.Add(jobModel);
            //Jobs.Add(jobModel);


            IsEditing = false;
            IsNotEditing = true;

        }

        #endregion

        #region Private fields

        private readonly ManageData _manageData;
        private IAuthenticationService _authenticationService;

        #endregion

        #region Private methods
        private async Task GetProfileData()
        {

            try
            {
                string token = await _authenticationService.GetCurrentToken();
                _manageData.SetStrategy(new GetData());
                var profile = await _manageData.GetDataAndDeserializeIt<EntireProfileDTO>
                    ($"Profile/GetProfileByUserId", "", token);
                
                FirstName=profile.FirstName;
                LastName=profile.LastName;
                Description = profile.Description;
                Jobs.ReplaceRange(profile.Experiences);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private async Task GetExperiences()
        {

        }

        private async Task GetFinishedStudies()
        {

        }
        private async Task InitializePage()
        {

        }
        #endregion

        #region Observables

        private ObservableRangeCollection<FinishedProgramModel> _programs;
        public ObservableRangeCollection<FinishedProgramModel> Programs
        {
            get { return _programs; }
            set { SetProperty(ref _programs, value); }
        }

        private ObservableRangeCollection<ExperienceModel> _jobs;
        public ObservableRangeCollection<ExperienceModel> Jobs
        {
            get { return _jobs; }
            set { SetProperty(ref _jobs, value); }
        }

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private bool _isEditing;

        [ObservableProperty]
        private string _firstName;

        [ObservableProperty]
        private string _lastName;

        [ObservableProperty]
        private bool _isNotEditing;

        [ObservableProperty]
        private FinishedProgramModel _selectedFinishedProgram;

        [ObservableProperty]
        private ExperienceModel _selectedJobExperience;
        #endregion

        #region Commands

        [RelayCommand]
        public void SignOut()
        {
            var authService = DependencyService.Resolve<IAuthenticationService>();
            authService.SignOut();

            var newNavigationPage = new NavigationPage(new LoginView());
            Application.Current.MainPage = newNavigationPage;
        }

        [RelayCommand]
        public void EditDescription()
        {
            IsEditing = true;
            IsNotEditing = false;
        }

        [RelayCommand]
        public async void SaveDescription()
        {
            IsEditing = false;
            IsNotEditing = true;

            try
            {
                string token = await _authenticationService.GetCurrentToken();
                _manageData.SetStrategy(new UpdateData());
                await _manageData.GetDataAndDeserializeIt<ProfileDTO>($"Profile/UpdateProfileDescriptionByUserId?profileDescription={Description}", "",token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        [RelayCommand]
        public async void PageAppearing()
        {
            await GetProfileData();
            SelectedFinishedProgram = null;
            SelectedJobExperience = null;
        }
        [RelayCommand]
        public async Task AddFinishedProgram()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddOrEditSpecializationView());
        }

        [RelayCommand]
        public async Task AddJobExperience()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddOrEditExperienceView());
        }

        [RelayCommand]
        public async Task EditFinishedProgram(object obj)
        {
            if (SelectedFinishedProgram == null)
            {
                return;
            }
            FinishedProgramModel selected = SelectedFinishedProgram;
            SelectedFinishedProgram = null;
            await Application.Current.MainPage.Navigation.PushAsync(new AddOrEditSpecializationView(selected));
        }

        [RelayCommand]
        public async Task EditWorkExperience(object obj)
        {
            if (SelectedJobExperience == null)
            {
                return;
            }
            ExperienceModel selected = SelectedJobExperience;
            SelectedJobExperience = null;
            await Application.Current.MainPage.Navigation.PushAsync(new AddOrEditExperienceView(selected));
        }



        #endregion
    }
}
