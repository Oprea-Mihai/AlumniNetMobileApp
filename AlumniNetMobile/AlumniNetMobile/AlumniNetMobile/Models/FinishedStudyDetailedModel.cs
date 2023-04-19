namespace AlumniNetMobile.Models
{
    public class FinishedStudyDetailedModel
    {
        public int Year { get; set; }

        public LearningScheduleModel LearningSchedule { get; set; } = null!;

        public SpecializationModel Specialization { get; set; }

        public StudyProgramModel StudyProgram { get; set; } = null!;
    }
}
