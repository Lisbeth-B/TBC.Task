using TBC.Task.Core.Enums;

namespace TBC.Task.Core.PersonEntity;

public class Person
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required Sex Sex { get; set; }
    public required string PersonalNumber { get; set; }
    public required DateOnly DateOfBirth { get; set; }
    public City? City { get; set; }
    public List<PhoneNumber>? PhoneNumbers { get; set; } = [];
    public string? Image { get; set; }
    public List<RelatedPerson> RelatedPeople { get; set; } = [];
}
