using System.ComponentModel.DataAnnotations;

namespace TBC.Task.Api.Utilities.Attributes;

class MinimumAgeAttribute : ValidationAttribute
{
  private const int minimumAge = 18;

  protected override ValidationResult IsValid(object value, ValidationContext validationContext)
  {
    if (value is DateOnly dateOfBirth)
    {
      if (dateOfBirth.AddYears(minimumAge) > DateOnly.FromDateTime(DateTime.Now))
      {
        return new ValidationResult("The person must be an adult");
      }

      return ValidationResult.Success;
    }

    return new ValidationResult("Invalid date.");
  }
}
