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
using Xamarin.Essentials;
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
        private string facebook;
        private string instagram;
        private string linkedIn;

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
        private bool _isInstagramVisible;

        [ObservableProperty]
        private bool _isLinkedInVisible;

        [ObservableProperty]
        private bool _isFacebookVisible;

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
                var profile = await _manageData.GetDataAndDeserializeIt<ProfileModel>
                    ($"Profile/GetProfileById?profileId={_profileId}", "", token);

                FirstName = profile.FirstName;
                LastName = profile.LastName;
                Description = profile.Description;
                Jobs.ReplaceRange(profile.Experiences);

                if (profile.Facebook != null && profile.Facebook != "")
                {
                    facebook = profile.Facebook;
                    IsFacebookVisible = true;
                }

                if (profile.Instagram != null && profile.Instagram != "")
                {
                    instagram = profile.Instagram;
                    IsInstagramVisible = true;
                }

                if (profile.LinkedIn != null && profile.LinkedIn != "")
                {
                    linkedIn = profile.LinkedIn;
                    IsLinkedInVisible = true;
                }

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
        public void OpenFacebook()
        {
            Launcher.OpenAsync(new Uri(facebook));
        }

        [RelayCommand]
        public void OpenLinkedIn()
        {
            Launcher.OpenAsync(new Uri(linkedIn));
        }

        [RelayCommand]
        public void OpenInstagram()
        {
            Launcher.OpenAsync(new Uri(instagram));
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
