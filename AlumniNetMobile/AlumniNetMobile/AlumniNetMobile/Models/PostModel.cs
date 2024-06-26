﻿using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlumniNetMobile.Models
{
    public class PostModel
    {
        public int PostId { get; set; }

        public ImageSource ImageSource { get; set; }

        public string Image { get; set; }

        public string Text { get; set; }

        public string Title { get; set; }

        public DateTime PostingDate { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
