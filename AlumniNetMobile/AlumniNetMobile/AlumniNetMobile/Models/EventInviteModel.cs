using System;
using Xamarin.Forms;

namespace AlumniNetMobile.Models
{
    public class EventInviteModel
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public DateTime StartDate{ get; set; }
        public string StartDateString { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public int InviteId { get; set; }
        public ImageSource ImageSource { get; set; }
    }
}
