using System;
using System.Collections.Generic;
using System.Text;

namespace AlumniNetMobile.DTOs
{
    public class ProfileDTO
    {
        public int ProfileId { get; set; }

        public int UserId { get; set; }

        public string ProfilePicture { get; set; }

        public string Description { get; set; }
    }
}
