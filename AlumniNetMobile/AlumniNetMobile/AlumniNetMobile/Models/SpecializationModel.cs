namespace AlumniNetMobile.Models
{
    public class SpecializationModel
    {
        public string SpecializationName { get; set; }
        public virtual FacultyModel Faculty { get; set; } = null!;
        public int SpecializationId { get; set; }
        public int FacultyId { get; set; }
    }
}
