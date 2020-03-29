using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Vidly.Dtos;

namespace Vidly.Models
{
    public class Min18YeatsIfAMember : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //return base.IsValid(value, validationContext);
            var customer = (Customer)validationContext.ObjectInstance;
            
            if (customer.MembershipTypeId == MembershipType.Unknown || customer.MembershipTypeId == MembershipType.PayAsYouGo)
            {
                return ValidationResult.Success;
            }
            if (customer.Birthdate == null)
            {
                return new ValidationResult("Birthdate is required!");
            }

            var age = DateTime.Today.Year - customer.Birthdate.Value.Year;

            // ternary operator voorbeeld opgenomen in een return statement.
            return (age >= 18) ? ValidationResult.Success : new ValidationResult("Customer must be at least be 18 years old.");
            
        }
    }
}