using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TBC.Task.Api.Utilities.Attributes;
public partial class FirstAndLastNameAttribute : ValidationAttribute
{
  protected override ValidationResult IsValid(object value, ValidationContext validationContext)
  {
    ArgumentNullException.ThrowIfNull(value);

    string input = value.ToString() ?? "";

    if (!LatinRegex().IsMatch(input) && !GeorgianRegex().IsMatch(input))
    {
      return new ValidationResult("Only Latin or Georgian alphabets are allowed.");
    }

    return ValidationResult.Success!;
  }

  [GeneratedRegex("^[a-zA-Z]+$")]
  private static partial Regex LatinRegex();

  [GeneratedRegex("^[ა-ჰ]+$")]
  private static partial Regex GeorgianRegex();
}
