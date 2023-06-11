using System;
using System.Collections.Generic;
using System.Text;

namespace AlumniNetMobile.DTOs
{
    public class EventDTO
    {
        public int EventId { get; set; }

        public string Initiator { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public string Description { get; set; } = null!;

        public string? Image { get; set; }

        public string EventName { get; set; } = null!;
    }
}
