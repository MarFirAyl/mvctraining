using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.ViewModels
{
    public class CustomerFormViewModel
    {
        public IEnumerable<MembershipType> MembershipTypes { get; set; } // in de viewmodel zijn er geen functies nodig van de List implemantatie. Kiezen voor IEnumerable draagt dan bij aan de losheid van koppeling
        public Customer Customer  { get; set; }
    }
}