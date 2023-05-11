using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.DTOs;
using AlumniNetMobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    partial class VisitedProfileViewModel : CommunityToolkit.Mvvm.ComponentModel.ObservableObject
    {
        #region Constructors
        public VisitedProfileViewModel(int profileId)
        {
            _profileId = profileId;
            _manageData = new ManageData();
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            Programs = new ObservableRangeCollection<FinishedProgramModel>();
            Jobs = new ObservableRangeCollection<ExperienceModel>();
            IsExtended = false;
            IsNotExtended = true;
        }
        #endregion

        #region Private fields

        private readonly ManageData _manageData;
        private IAuthenticationService _authenticationService;
        private int _profileId;
        private string profilePicKey = "";

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
        private string _firstName;

        [ObservableProperty]
        private string _lastName;

        [ObservableProperty]
        private bool _isExtended;

        [ObservableProperty]
        private bool _isNotExtended;

        [ObservableProperty]
        ImageSource _profilePicture;
        #endregion

        #region Methods

        private async Task GetProfileData()
        {

            try
            {
                string token = await _authenticationService.GetCurrentTokenAsync();
                _manageData.SetStrategy(new GetData());
                var profile = await _manageData.GetDataAndDeserializeIt<EntireProfileDTO>
                    ($"Profile/GetProfileById?profileId={_profileId}", "", token);

                FirstName = profile.FirstName;
                LastName = profile.LastName;
                Description = profile.Description;
                Jobs.ReplaceRange(profile.Experiences);

                Programs.ReplaceRange(profile.FinishedStudiesDetailed.Select(x => new FinishedProgramModel
                {
                    FinishedStudyId = x.FinishedStudyId,
                    FacultyName = x.Specialization.Faculty.FacultyName,
                    Specialization = x.Specialization.SpecializationName,
                    Program = x.StudyProgram.ProgramName,
                    LearningSchedule = x.LearningSchedule.ScheduleName,
                    Year = x.Year,
                    SpecializationId = x.SpecializationId,
                    StudyProgramId = x.StudyProgramId,
                    LearningScheduleId = x.LearningScheduleId,
                    FacultyId = x.Specialization.FacultyId,
                    ProfileId = x.ProfileId
                }));
                profilePicKey = profile.ProfilePicture;
                if (profilePicKey != null && profilePicKey != "")
                {
                    GetData getData = new GetData();
                    Stream file = await getData.ManageStreamData($"Files/GetFileByKey?key={profilePicKey}", token);
                    ProfilePicture = ImageSource.FromStream(() => file);
                }
                else ProfilePicture = ImageSource.FromFile("user.png");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion

        #region Commands
        [RelayCommand]
        public async void PageAppearing()
        {
            await GetProfileData();
        }

        [RelayCommand]
        public void ShowMore()
        {
            IsExtended = !IsExtended;
            IsNotExtended = !IsNotExtended;
        }
        #endregion
    }

}
