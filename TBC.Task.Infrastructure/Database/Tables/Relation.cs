namespace TBC.Task.Infrastructure.Database.Tables;

public class Relation
{
    public int Id { get; set; }
    public required int PersonId { get; init; }
    public required int RelatedPersonId { get; init; }
    public required int RelationType { get; init; }

    public Person? Person { get; init; }
}
