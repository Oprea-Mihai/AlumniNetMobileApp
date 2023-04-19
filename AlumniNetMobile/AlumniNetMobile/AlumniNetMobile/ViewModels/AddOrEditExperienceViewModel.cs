using AlumniNetMobile.DTOs;
using AlumniNetMobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlumniNetMobile.ViewModels
{
    public partial class AddOrEditExperienceViewModel : ObservableObject
    {
        #region Constructors

        public AddOrEditExperienceViewModel()
        {
            JobTitle = null;
            CompanyName = null;
            StartDate = null;
            IsStillEmployedChecked = false;
        }

        public AddOrEditExperienceViewModel(ExperienceDTO selectedJob)
        {
            _selectedJob = selectedJob;
            StartDate = "2020";
            EndDate = selectedJob.EndDate == 0 ? "" : selectedJob.EndDate.ToString();
            if (selectedJob.EndDate == 0)
                IsStillEmployedChecked = true;
            else
                IsStillEmployedChecked = false;
            JobTitle = selectedJob.JobTitle;
            CompanyName = selectedJob.CompanyName;
        }

        #endregion


        #region Private fields

        private ExperienceDTO _selectedJob;

        #endregion


        #region Observables

        [ObservableProperty]
        private string _startDate;

        [ObservableProperty]
        private string _endDate;

        [ObservableProperty]
        private string _jobTitle;

        [ObservableProperty]
        private string _companyName;

        [ObservableProperty]
        private bool _isStillEmployedChecked;

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
