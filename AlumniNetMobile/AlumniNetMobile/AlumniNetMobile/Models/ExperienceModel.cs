using System;
using System.Collections.Generic;
using System.Text;

namespace AlumniNetMobile.Models
{
    public class ExperienceModel
    {
        public int ExperienceId { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public int StartDate { get; set; }
        public int? EndDate { get; set; }
    }
}
