namespace AlumniNetMobile.Models
{
    public class SpecializationModel
    {
        public string SpecializationName { get; set; }
        public virtual FacultyModel Faculty { get; set; } = null!;

    }
}
