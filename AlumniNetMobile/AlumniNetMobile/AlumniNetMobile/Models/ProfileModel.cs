using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AlumniNetMobile.Models;

namespace AlumniNetMobile.DTOs
{
    public class ProfileModel
    {
        //profile
        public string ProfilePicture { get; set; }
        public string Description { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? LinkedIn { get; set; }

        //user
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsValid { get; set; }



        //experiences
        public List<ExperienceModel> Experiences { get; set; }

        //finished studies
        public List<FinishedStudyDetailedDTO> FinishedStudiesDetailed { get; set; }
    }
}
