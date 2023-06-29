using HSE.RP.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;




namespace HSE.RP.API.Models
{
    public record PhoneNumberVerificationModel(string PhoneNumber) : IValidatableModel
    {
        public ValidationSummary Validate()
        {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(PhoneNumber) || !IsPhoneNumber(PhoneNumber))
            {
                errors.Add("You must enter a phone number in the correct format");
            }
            return new ValidationSummary(!errors.Any(), errors.ToArray());
        }


        public static bool IsPhoneNumber(string PhoneNumber)
        {
            Regex validatePhoneNumberRegex = new Regex("^\\+?\\d{1,4}?[-.\\s]?\\(?\\d{1,3}?\\)?[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,4}[-.\\s]?\\d{1,9}$");


            if (PhoneNumber != null) return validatePhoneNumberRegex.IsMatch(PhoneNumber);
            else return false;
        }

    }
}