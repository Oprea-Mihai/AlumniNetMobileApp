using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AlumniNetMobile.DTOs
{
    public class PostDTO
    {
        public int PostId { get; set; }

        public string? Image { get; set; }

        public string? Text { get; set; }

        public string? Title { get; set; }

        public DateTime PostingDate { get; set; }
    }
}
