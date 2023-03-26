﻿using AlumniNetMobile.Models;
using Xamarin.CommunityToolkit.ObjectModel;

namespace AlumniNetMobileApp.ViewModels
{
    public class ProfileViewModel :ObservableObject
    {
        private ObservableRangeCollection<FinishedProgramModel> _programs;
        public ObservableRangeCollection<FinishedProgramModel> Programs
        {
            get { return _programs; }
            set { SetProperty(ref _programs, value); }
        }
        private ObservableRangeCollection<JobModel> _jobs;
        public ObservableRangeCollection<JobModel> Jobs
        {
            get { return _jobs; }
            set { SetProperty(ref _jobs, value); }
        }
        public ProfileViewModel()
        {
            Programs = new ObservableRangeCollection<FinishedProgramModel>();
            FinishedProgramModel programModel = new FinishedProgramModel
            {
                FacultyName = "Facultatea de stiinte economice si administrarea afacerilor",
                Specialization = "Informatica economica",
                LearningSchedule = "Zi",
                GraduationYear = 2020,
                Program = "Licenta"
            };
            Programs.Add(programModel);
            Programs.Add(programModel);

            Jobs = new ObservableRangeCollection<JobModel>();

            JobModel jobModel = new JobModel
            {
                CompanyName = "Talenting Software",
                JobTitle = "Software Engineer",
                StartEndDate = "2018 - Prezent"
            };

            Jobs.Add(jobModel);
            Jobs.Add(jobModel);
        }
    }
}
