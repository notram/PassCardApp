using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PassCardApp.Models
{
    public class Clients
    {

        [Key]
        public int ClientId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }


        [Display(Name = "Passcard Number")]
        public string PasscardNumber { get; set; }

        [Display(Name = "Registration Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime InsertedAt { get; set; }


        public Guid InsertedBy { get; set; }


    }
}
