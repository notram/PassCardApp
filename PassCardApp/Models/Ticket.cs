using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PassCardApp.Models
{
    public class Ticket
    {
        
        [Key]
        public int TicketId { get; set; }


        // the type of the ticket that was sold 
        public int TicketTypeId { get; set; }

        [ForeignKey("TicketTypeId")]
        public TicketType TicketType {get; set;}

        // who bought it
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Clients Client { get; set; }


        // who sold it
        public string SoldById { get; set; }
        
        [ForeignKey("SoldById")]
        public IdentityUser SoldByUser { get; set; }

        //when 
        public DateTime SoldAt { get; set; }

        //Becomes active after
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime ActiveFrom { get; set; }

        protected bool CheckinsWereIncluded = false;

        protected ICollection<Checkin> checkins;

        [ForeignKey("TicketId")]
        public ICollection<Checkin> Checkins {
            get
            {
                return checkins;
            }
            set
            {
                CheckinsWereIncluded = true;
                checkins = value;
            }
        }



        [NotMapped]
        public DateTime? ActiveUntil { get
            {
                if (TicketType == null)
                {
                    throw new TicketTypeUndefinedException();
                }
                if (TicketType.ActiveDayCount == null)
                {
                    return null;
                }
                return this.ActiveFrom.AddDays((double)TicketType.ActiveDayCount);
            }
        }


        public enum ActiveStaus : int { Active = 1, NotActiveYet, Expired }

        [NotMapped]
        public ActiveStaus Status { get
            {
                if (DateTime.Now < ActiveFrom) return ActiveStaus.NotActiveYet;
                if (DateTime.Now >= ActiveFrom && ( (ActiveUntil == null) || ( (ActiveUntil != null) && DateTime.Now < ActiveUntil ) )) return ActiveStaus.Active;
                return ActiveStaus.Expired;
            }
        }
        [NotMapped]
        public int? RemainingCheckinsCount
        {
            get {
                if (TicketType == null)
                {
                    throw new TicketTypeUndefinedException();
                }
                if (!CheckinsWereIncluded)
                {
                    throw new CheckinsUndefinedException();
                }
                if (TicketType.TotalCheckinLimit == null) return null;
                if (Checkins == null) return TicketType.TotalCheckinLimit;

                return TicketType.TotalCheckinLimit - Checkins.Count;
            }
        }
        //
        public int? RemainingCheckinsTodayCount
        {
            get
            {
                if (TicketType == null)
                {
                    throw new TicketTypeUndefinedException();
                }
                if (!CheckinsWereIncluded)
                {
                    throw new CheckinsUndefinedException();
                }

                if (TicketType.DailyCheckInLimit == null) return null;
                if (Checkins == null) return TicketType.DailyCheckInLimit;
                var todayscheckincount = Checkins.Where(c => c.ChecinDate.Date == DateTime.Now.Date).Count();
                //if (todayscheckincount < TicketType.DailyCheckInLimit) return TicketType.DailyCheckInLimit;

                return TicketType.DailyCheckInLimit - todayscheckincount;
            }
        }
        [NotMapped]
        public bool CanCheckIn{
            get
            {

                if (TicketType == null)
                {
                    throw new TicketTypeUndefinedException();
                }
                if (!CheckinsWereIncluded)
                {
                    throw new CheckinsUndefinedException();
                }

                if ( (ActiveStaus.Active != Status) || //its in active status
                    (!TicketType.IsActive(DateTime.Now.DayOfWeek)) || //today is allowed to checkin //by day of week
                    (! ((RemainingCheckinsCount ?? 1) > 0) ) || //remaining checkincount is ok
                    (! ((RemainingCheckinsTodayCount??1)>0 )) ||
                    (!(DateTime.Now.Hour > (TicketType.StartHour ?? 0) && DateTime.Now.Hour < (TicketType.EndHour ?? 24)))|| //hour interval matches
                    (!(Checkins.Where(c=>c.CheckOutDate==null).Count()==0))//it has no checkins without checkouts
                    )
                {
                    return false;
                }

                return true;
            }
        }
        [NotMapped]
        public bool CanCheckOut
        {
            get
            {
                if (!CheckinsWereIncluded)
                {
                    throw new CheckinsUndefinedException();
                }
                if (Checkins.Where(c => c.CheckOutDate == null).Count() > 0) return true;

                return false;

            }
        }

        private class TicketTypeUndefinedException : Exception
        {
            public TicketTypeUndefinedException()
            {

            }
        }

        private class CheckinsUndefinedException : Exception
        {
            public CheckinsUndefinedException()
            {

            }
        }

    }
}
