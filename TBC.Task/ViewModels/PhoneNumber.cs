using System.ComponentModel.DataAnnotations;

namespace TBC.Task.Api.ViewModels;

public class PhoneNumber
{
    public required int Type { get; set; }

    [Length(4, 50)]
    public required string Number { get; set; }
}
