using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PassCardApp.Models
{
    public class TicketType
    {
        [Key]
        public int TicketTypeId { get; set; }

        [StringLength(50, ErrorMessage = "Ticket type name must be less than 50 characters long")]
        public string TicketTypeName { get; set; }

        public int? ActiveDayCount { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Checkinlimit must be greater than 1")]
        public int? CheckinLimit { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Daily must be greater than 1")]
        public int? DailyCheckInLimit { get; set; }


        [Range(0, 23, ErrorMessage = "Activeness start hour must be between 0-23")]
        public int? StartHour { get; set; }

        [Range(1, 24, ErrorMessage = "Activeness end hour must be between 1-24")]
        public int? EndHour { get; set; }


        public int ActiveDays { get; set; }
        public bool ActiveMonday { get; set; }
        public bool ActiveTuesday { get; set; }
        public bool ActiveWednesday { get; set; }
        public bool ActiveThursday { get; set; }
        public bool ActiveFriday { get; set; }
        public bool ActiveSaturday { get; set; }
        public bool ActiveSunday { get; set; }
    }
}
