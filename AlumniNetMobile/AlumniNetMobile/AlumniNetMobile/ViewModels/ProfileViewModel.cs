using AlumniNetMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.CommunityToolkit.ObjectModel;

namespace AlumniNetMobileApp.ViewModels
{
    public class ProfileViewModel : ObservableObject
    {
        private ObservableRangeCollection<FinishedProgramModel> _programs;
        public ObservableRangeCollection<FinishedProgramModel> Programs
        {
            get { return _programs; }
            set { SetProperty(ref _programs, value); }
        }
        private ObservableRangeCollection<FinishedProgramModel> _experience;
        public ObservableRangeCollection<FinishedProgramModel> Experience
         {
            get { return _experience; }
            set { SetProperty(ref _experience, value); }
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

        }
    }
}
