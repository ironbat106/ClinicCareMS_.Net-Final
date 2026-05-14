using System;
using System.ComponentModel.DataAnnotations;

namespace BLL.Validations
{
    public class FutureOrTodayAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            DateTime date = Convert.ToDateTime(value);
            if (date < DateTime.Today)
            {
                return new ValidationResult("Appointment date cannot be in the past.");
            }

            return ValidationResult.Success;
        }
    }
}
