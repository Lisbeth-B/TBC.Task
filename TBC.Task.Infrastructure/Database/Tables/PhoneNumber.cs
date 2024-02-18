namespace TBC.Task.Infrastructure.Database.Tables;

public class PhoneNumber
{
    public int Id { get; init; }
    public required int PersonId { get; init; }
    public required int Type { get; init; }
    public required string Number { get; init; }

    public Person? Person { get; init; }
}
