using AlumniNetMobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class AddOrEditExperienceViewModel:ObservableObject
    {
        #region Constructors

        public AddOrEditExperienceViewModel()
        {
            JobTitle =null;
            CompanyName =null;
            StartEndDate =null;
        }

        public AddOrEditExperienceViewModel(JobModel selectedJob)
        {
            _selectedJob = selectedJob;
            StartEndDate = selectedJob.StartEndDate;
            JobTitle=selectedJob.JobTitle;
            CompanyName = selectedJob.CompanyName;
        }

        #endregion


        #region Private fields

        private JobModel _selectedJob;

        #endregion


        #region Observables

        [ObservableProperty]
        private string _startEndDate;

        [ObservableProperty]
        private string _endDate;

        [ObservableProperty]
        private string _jobTitle;

        [ObservableProperty]
        private string _companyName;

        #endregion

        #region Commands

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


        [RelayCommand]
        public void StillEmployed()
        {

        }
        #endregion



    }
}
