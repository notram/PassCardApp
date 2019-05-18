using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using PassCardApp.Data;
using Sy

namespace PassCardApp.Models
{
    public class TicketType
    {
        protected enum Day : byte { Mon = 0x01, Tue = 0x02, Wed = 0x04, Thu = 0x08, Fri = 0x10, Sat = 0x20, Sun = 0x40 };
        [Key]
        public int TicketTypeId { get; set; }

        [StringLength(50, ErrorMessage = "Ticket type name must be less than 50 characters long")]
        public string TicketTypeName { get; set; }

        public int? ActiveDayCount { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Checkinlimit must be greater than 1")]
        public int? TotalCheckinLimit { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Daily must be greater than 1")]
        public int? DailyCheckInLimit { get; set; }


        [Range(0, 23, ErrorMessage = "Activeness start hour must be between 0-23")]
        public int? StartHour { get; set; }

        [Range(1, 24, ErrorMessage = "Activeness end hour must be between 1-24")]
        public int? EndHour { get; set; }


        public byte ActiveDays { get; set; }

        [NotMapped]
        public bool ActiveMonday
        {
            get { return (ActiveDays & (byte)Day.Mon) > 0 ? true : false; }
            set { if (value) ActiveDays |= (byte)Day.Mon; }
        }
        [NotMapped]
        public bool ActiveTuesday
        {
            get { return (ActiveDays & (byte)Day.Tue) > 0 ? true : false; }
            set { if (value) ActiveDays |= (byte)Day.Tue; }
        }
        [NotMapped]
        public bool ActiveWednesday
        {
            get { return (ActiveDays & (byte)Day.Wed) > 0 ? true : false; }
            set { if (value) ActiveDays |= (byte)Day.Wed; }
        }
        [NotMapped]
        public bool ActiveThursday
        {
            get { return (ActiveDays & (byte)Day.Thu) > 0 ? true : false; }
            set { if (value) ActiveDays |= (byte)Day.Thu; }
        }
        [NotMapped]
        public bool ActiveFriday
        {
            get { return (ActiveDays & (byte)Day.Fri) > 0 ? true : false; }
            set { if (value) ActiveDays |= (byte)Day.Fri; }
        }
        [NotMapped]
        public bool ActiveSaturday
        {
            get { return (ActiveDays & (byte)Day.Sat) > 0 ? true : false; }
            set { if (value) ActiveDays |= (byte)Day.Sat; }
        }
        [NotMapped]
        public bool ActiveSunday
        {
            get { return (ActiveDays & (byte)Day.Sun) > 0 ? true : false; }
            set { if (value) ActiveDays |= ((byte)Day.Sun); }
        }

        public double Price { get; set; }


        public Guid? InsertedBy { get; set; }


        public DateTime InsertedAt { get; set; }
    }
}
