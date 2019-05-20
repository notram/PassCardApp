using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PassCardApp.Data;

namespace PassCardApp.Models
{
    public class TicketType
    {
        protected enum Day : byte { Mon = 0x01, Tue = 0x02, Wed = 0x04, Thu = 0x08, Fri = 0x10, Sat = 0x20, Sun = 0x40 };
        [Key]
        public int TicketTypeId { get; set; }
        [Display(Name = "Name")]
        [StringLength(50, ErrorMessage = "Ticket type name must be less than 50 characters long")]
        public string TicketTypeName { get; set; }

        [Display(Name = "Active days")]
        public int? ActiveDayCount { get; set; }


        [Display(Name = "Checkins Limit", Description = "Total checkin limit, leave blank if not applyied")]
        [Range(1, int.MaxValue, ErrorMessage = "Checkinlimit must be greater than 1")]
        public int? TotalCheckinLimit { get; set; }


        [Display(Name = "Daily Limit", Description = "Clients checkin allowed a day, leave blank if not applyied")]
        [Range(1, int.MaxValue, ErrorMessage = "Daily must be greater than 1")]
        public int? DailyCheckInLimit { get; set; }


        [Display(Name = "Daily interval from", Description = "Daily interval starts at")]
        [Range(0, 23, ErrorMessage = "Activeness start hour must be between 0-23")]
        public int? StartHour { get; set; }

        [Display(Name = "Daily interval to", Description = "Daily interval ends at")]
        [Range(1, 24, ErrorMessage = "Activeness end hour must be between 1-24")]
        public int? EndHour { get; set; }

        public string ActiveHoursString
        {
            get
            {

                string res = "";
                if (StartHour==null && EndHour == null)
                {
                    return "All Day";
                }

                if (StartHour != null)
                {
                    res += StartHour.ToString();
                }
                else
                {
                    res += "Openning"; 
                }
                res += "-";

                if (EndHour != null) {
                    res += EndHour.ToString();
                }
                else
                {
                    res += "Closing";
                }
                return res;

            }
        }

        public byte ActiveDays { get; set; }

        [NotMapped]
        [Display(Name = "Mon")]
        public bool ActiveMonday
        {
            get { return (ActiveDays & (byte)Day.Mon) > 0 ? true : false; }
            set { if (value) ActiveDays |= (byte)Day.Mon; }
        }
        [NotMapped]
        [Display(Name = "Tue")]
        public bool ActiveTuesday
        {
            get { return (ActiveDays & (byte)Day.Tue) > 0 ? true : false; }
            set { if (value) ActiveDays |= (byte)Day.Tue; }
        }

        [NotMapped]
        [Display(Name = "Wed")]
        public bool ActiveWednesday
        {
            get { return (ActiveDays & (byte)Day.Wed) > 0 ? true : false; }
            set { if (value) ActiveDays |= (byte)Day.Wed; }
        }

        [NotMapped]
        [Display(Name = "Thu")]
        public bool ActiveThursday
        {
            get { return (ActiveDays & (byte)Day.Thu) > 0 ? true : false; }
            set { if (value) ActiveDays |= (byte)Day.Thu; }
        }

        [NotMapped]
        [Display(Name = "Fri")]
        public bool ActiveFriday
        {
            get { return (ActiveDays & (byte)Day.Fri) > 0 ? true : false; }
            set { if (value) ActiveDays |= (byte)Day.Fri; }
        }

        [NotMapped]
        [Display(Name = "Sat")]
        public bool ActiveSaturday
        {
            get { return (ActiveDays & (byte)Day.Sat) > 0 ? true : false; }
            set { if (value) ActiveDays |= (byte)Day.Sat; }
        }

        [NotMapped]
        [Display(Name = "Sun")]
        public bool ActiveSunday
        {
            get { return (ActiveDays & (byte)Day.Sun) > 0 ? true : false; }
            set { if (value) ActiveDays |= ((byte)Day.Sun); }
        }
        [NotMapped]
        [Display(Name = "Active Days")]
        public string ActiveDaysString
        {
            get
            {
                if ((ActiveDays ^ (byte)(Day.Mon | Day.Tue | Day.Wed | Day.Thu | Day.Fri | Day.Sat | Day.Sun)) == 0x00)
                {
                    return "All Days";
                }
                string[] days = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
                string res = "";
                if ((ActiveDays & (byte)Day.Mon) > 0) res += "Mon,";
                if ((ActiveDays & (byte)Day.Tue) > 0) res += "Tue,";
                if ((ActiveDays & (byte)Day.Wed) > 0) res += "Wed,";
                if ((ActiveDays & (byte)Day.Thu) > 0) res += "Thu,";
                if ((ActiveDays & (byte)Day.Fri) > 0) res += "Fri,";
                if ((ActiveDays & (byte)Day.Sat) > 0) res += "Sat,";
                if ((ActiveDays & (byte)Day.Sun) > 0) res += "Sun,";
                res = res.Substring(0,res.Length-1);
                return res;
            }
        }

        public double Price { get; set; }


        public string InsertedBy { get; set; }


        [ForeignKey("InsertedBy")]
        public IdentityUser InsertedByUser { get; set; }

        
        public DateTime InsertedAt { get; set; }

        //public bool Active { get; set; }

        //public DateTime DeActivatedAt { get; set; }
    }
}
