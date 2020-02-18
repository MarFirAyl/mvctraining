using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        
        public string Name { get; set; }
        
        public bool IsSubscribedToNewsletter { get; set; }
        
        public MembershipType MembershipType { get; set; }
        
        [Display(Name = "Membership Type")]
        public byte MembershipTypeId { get; set; }
        
        [Display(Name = "Date of Birth")]  // naam instellen die getoond wordt in een view. Dit kan eventueel ook door een <label for="id_textbox"> te gebruiken.
        public DateTime? Birthdate { get; set; }
    }
}