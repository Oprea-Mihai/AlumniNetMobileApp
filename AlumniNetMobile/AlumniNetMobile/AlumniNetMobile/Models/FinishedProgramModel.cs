using System;
using System.Collections.Generic;
using System.Text;

namespace AlumniNetMobile.Models
{
    public class FinishedProgramModel
    {
        public int FinishedStudyId { get; set; }
        public string FacultyName { get; set; }
        public string Specialization { get; set; }
        public string Program { get; set; }
        public string LearningSchedule { get; set; }
        public int Year { get; set; }
        public int SpecializationId { get; set; }
        public int LearningScheduleId { get; set; }
        public int StudyProgramId { get; set; }
        public int FacultyId { get; set; }
    }
}
