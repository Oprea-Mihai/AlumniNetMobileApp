using AlumniNetMobile.Models;

namespace AlumniNetMobile.DTOs
{
    public class FinishedStudyDetailedDTO
    {
        public int FinishedStudyId { get; set; }

        public int SpecializationId { get; set; }

        public int LearningScheduleId { get; set; }

        public int StudyProgramId { get; set; }

        public int Year { get; set; }

        public int ProfileId { get; set; }

        public LearningScheduleModel LearningSchedule { get; set; } = null!;

        public SpecializationModel Specialization { get; set; }

        public StudyProgramModel StudyProgram { get; set; } = null!;
    }
}
