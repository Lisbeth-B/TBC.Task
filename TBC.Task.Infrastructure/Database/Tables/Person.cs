namespace TBC.Task.Infrastructure.Database.Tables;

public class Person
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required int Sex { get; set; }
    public required string PersonalNumber { get; set; }
    public required DateOnly BirthDate { get; set; }
    public int? CityId { get; set; }
    public string? Image { get; set; }

    public List<PhoneNumber> PhoneNumbers { get; set; } = [];
    public City? City { get; set; }
    public List<Relation> Relationships { get; set; } = [];
}
