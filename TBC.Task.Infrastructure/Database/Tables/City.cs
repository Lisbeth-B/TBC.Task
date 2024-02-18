namespace TBC.Task.Infrastructure.Database.Tables;

public class City
{
    public int Id { get; init; }
    public required string Name { get; init; }

    public List<Person>? People { get; init; }

}
