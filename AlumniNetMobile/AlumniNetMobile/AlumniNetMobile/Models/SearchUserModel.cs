using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlumniNetMobile.Models
{
    public class SearchUserModel
    {
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string FacultyName { get; set; }
        public int GraduationYear { get; set; }
        public ImageSource ImageSource{ get; set; } 
    }
}
