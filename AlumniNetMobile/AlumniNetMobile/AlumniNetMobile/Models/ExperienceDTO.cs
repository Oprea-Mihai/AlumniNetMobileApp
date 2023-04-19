using System;
using System.Collections.Generic;
using System.Text;

namespace AlumniNetMobile.DTOs
{
    public class ExperienceDTO
    {
        public int ExperienceId { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public int StartDate { get; set; }
        public int EndDate { get; set; }
    }
}
