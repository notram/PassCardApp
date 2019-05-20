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

        

    }
}
