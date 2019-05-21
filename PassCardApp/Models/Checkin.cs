using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PassCardApp.Models
{
    [Authorize(Roles = "Admin")]
    public class Checkin
    {
        [Key]
        public int CheckinId { get; set; }

        //checkin with ticket

        public int TicketId { get; set; }

        [ForeignKey("TicketId")]
        public Ticket Ticket { get; set; }


        // Client
        //public int ClientId { get; set; }

        //[ForeignKey("ClientId")]
        //public Clients Client {get;set;}


        // Employee who and when checked in
        public string CheckedInById { get; set; }

        [ForeignKey("CheckedInById")]
        public IdentityUser CheckedInBy { get;set; }

        public DateTime ChecinDate { get; set; }



        // Employee who and when checked out
        public string CheckedOutById { get; set; }

        [ForeignKey("CheckedOutById")]
        public IdentityUser CheckedOutBy { get; set; }

        public DateTime? CheckOutDate { get; set; }

    }
}
