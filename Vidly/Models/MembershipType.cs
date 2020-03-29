using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class MembershipType
    {
        public byte Id { get; set; }
        public short SignUpFee { get; set; }
        public byte DurationInMonths { get; set; }
        public byte DiscountRate { get; set; }
        [Required]
        public string Name { get; set; }

        // Om  magic numbers te voorkomen kun je static properties gebruiken. Dit komt de onderhoudbaarheid van decode ten goede,er kan hiervoor eventueel ook een neum gebruikt worden dit heeft als nadeel dat er extra gecast moet worden voor een vergelijking.
        public static readonly byte Unknown = 0;
        public static readonly byte PayAsYouGo = 1;
    }
}