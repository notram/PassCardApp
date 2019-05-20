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
        public DateTime ActiveFrom { get; set; }



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


        public enum ActiveStaus { Expired, Active, NotActiveYet  }

        [NotMapped]
        public ActiveStaus Status { get
            {
                if (DateTime.Now < ActiveFrom) return ActiveStaus.NotActiveYet;
                if (DateTime.Now >= ActiveFrom && (ActiveUntil != null) && DateTime.Now < ActiveUntil) return ActiveStaus.Active;
                return ActiveStaus.Expired;
            }
        }
        [NotMapped]
        public bool CanCheckIn{
            get
            {
                return true;
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
