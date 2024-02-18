using System.ComponentModel.DataAnnotations;
using TBC.Task.Core.Enums;

namespace TBC.Task.Api.Utilities.Attributes
{
    public class SexAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ArgumentNullException.ThrowIfNull(value);

            if (!Enum.IsDefined(typeof(Sex), value))
            {
                return new ValidationResult("Sex should be either 1 - Male or 2 - Female.");
            }

            return ValidationResult.Success!;
        }
    }
}
