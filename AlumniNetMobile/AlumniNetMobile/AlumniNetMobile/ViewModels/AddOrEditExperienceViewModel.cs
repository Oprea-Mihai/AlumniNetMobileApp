using AlumniNetMobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlumniNetMobile.ViewModels
{
    public partial class AddOrEditExperienceViewModel:ObservableObject
    {
        #region Constructors

        public AddOrEditExperienceViewModel()
        {
            
        }

        public AddOrEditExperienceViewModel(JobModel selectedJob)
        {
            SelectedJob = selectedJob;
        }

        #endregion

        #region Observables

        [ObservableProperty]
        private JobModel _selectedJob;

        #endregion

        #region Commands
        #endregion


       
    }
}
