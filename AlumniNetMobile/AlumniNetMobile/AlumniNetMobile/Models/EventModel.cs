using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AlumniNetMobile.Models
{
   public class EventModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string Image { get; set; }
        public ImageSource ImageSource { get; set; }
    }
}
