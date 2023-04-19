namespace AlumniNetMobile.DTOs
{
    public class SpecializationDTO
    {
        public string SpecializationName { get; set; }
        public virtual FacultyDTO Faculty { get; set; } = null!;

    }
}
