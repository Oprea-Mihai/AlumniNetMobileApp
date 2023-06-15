using System;
using Xamarin.Forms;

namespace AlumniNetMobile.Models
{
    public class EventModel
    {
        public int EventId { get; set; }

        public string Initiator { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public string Description { get; set; } = null!;

        public string Image { get; set; }

        public string EventName { get; set; } = null!;
        public ImageSource ImageSource { get; set; }

        public int? Accepted { get; set; }
        public int? Rejected { get; set; }
        public int? Pending { get; set; }
    }
}
