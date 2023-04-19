namespace AlumniNetMobile.DTOs
{
    public class FinishedStudyDetailedDTO
    {
        public int Year { get; set; }

        public LearningScheduleDTO LearningSchedule { get; set; } = null!;

        public SpecializationDTO Specialization { get; set; }

        public StudyProgramDTO StudyProgram { get; set; } = null!;
    }
}
