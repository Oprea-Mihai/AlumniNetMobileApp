using AlumniNetMobile.Common;
using AlumniNetMobile.DataHandlingStrategy;
using AlumniNetMobile.DTOs;
using AlumniNetMobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class AddOrEditExperienceViewModel : ObservableObject
    {
        #region Constructors

        public AddOrEditExperienceViewModel()
        {
            CommonInitialization();
            IsDeleteButtonVisible = false;
            JobTitle = null;
            CompanyName = null;
            StartDate = EndDate = "";
            IsStillEmployedChecked = false;
            _jobToUpdate = null;
            _saveButtonEnabled = false;
            _isNew = true;
            _jobToUpdate = new ExperienceModel();
        }

        public AddOrEditExperienceViewModel(ExperienceModel selectedJob)
        {
            CommonInitialization();
            IsDeleteButtonVisible = true;
            _jobToUpdate = selectedJob;
            StartDate = selectedJob.StartDate.ToString();
            EndDate = selectedJob.EndDate == null ? "" : selectedJob.EndDate.ToString();
            if (selectedJob.EndDate == null)
                IsStillEmployedChecked = true;
            else
                IsStillEmployedChecked = false;
            JobTitle = selectedJob.JobTitle;
            CompanyName = selectedJob.CompanyName;
            _saveButtonEnabled = true;
            _isNew = false;
        }

        #endregion


        #region Private fields

        private ExperienceModel _jobToUpdate;
        private ManageData _manageData;
        private IAuthenticationService _authenticationService;
        private bool _isNew;

        #endregion

        #region Private methods

        private void IsValidForSaving()
        {
            SaveButtonEnabled = true;
            if (IsStillEmployedChecked == false && EndDate.Length < 4)
                SaveButtonEnabled = false;
            else if (StartDate.Length < 4)
                SaveButtonEnabled = false;
            else if (JobTitle == "" || CompanyName == "")
                SaveButtonEnabled = false;

        }


        private void CommonInitialization()
        {
            _authenticationService = DependencyService.Resolve<IAuthenticationService>();
            _manageData = new ManageData();
        }

        #endregion

        #region Observables

        [ObservableProperty]
        private string _startDate;

        [ObservableProperty]
        private bool _saveButtonEnabled;

        [ObservableProperty]
        private string _endDate;

        [ObservableProperty]
        private string _jobTitle;

        [ObservableProperty]
        private string _companyName;

        [ObservableProperty]
        private bool _isStillEmployedChecked;

        [ObservableProperty]
        private bool _isDeleteButtonVisible;

        #endregion

        #region Commands

        [RelayCommand]
        public async void Cancel()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        [RelayCommand]
        public async void Save()
        {
            _jobToUpdate.JobTitle = JobTitle;
            _jobToUpdate.StartDate = int.Parse(StartDate);
            if (IsStillEmployedChecked)
                _jobToUpdate.EndDate = null;
            else
                _jobToUpdate.EndDate = int.Parse(EndDate);
            _jobToUpdate.CompanyName = CompanyName;

            string token = await _authenticationService.GetCurrentTokenAsync();
            string json = JsonConvert.SerializeObject(_jobToUpdate);

            if (_isNew)
            {
                _manageData.SetStrategy(new CreateData());
                await _manageData.GetDataAndDeserializeIt<ExperienceModel>($"Experience/AddNewExperienceForUser", json, token);
            }
            else
            {
                _manageData.SetStrategy(new UpdateData());
                await _manageData.GetDataAndDeserializeIt<ExperienceModel>($"Experience/UpdateExperience", json, token);
            }
            _manageData.SetStrategy(new GetData());
            await Application.Current.MainPage.Navigation.PopAsync();
        }


        [RelayCommand]
        public void StillEmployed()
        {
            if (IsStillEmployedChecked == true)
                EndDate = "";
            IsValidForSaving();
        }

        [RelayCommand]
        public void CompanyNameTextChanged()
        {
            IsValidForSaving();
        }

        [RelayCommand]
        public void JobTitleTextChanged()
        {
            IsValidForSaving();
        }

        [RelayCommand]
        public void YearTextChanged()
        {
            IsValidForSaving();
        }

        [RelayCommand]
        public async void DeleteExperience()
        {
            string token = await _authenticationService.GetCurrentTokenAsync();
            _manageData.SetStrategy(new DeleteData());
            await _manageData.GetDataAndDeserializeIt<bool>($"Experience/DeleteExperience?experienceId={_jobToUpdate.ExperienceId}", "", token);
            await Application.Current.MainPage.Navigation.PopAsync();

        }
        #endregion



    }
}
