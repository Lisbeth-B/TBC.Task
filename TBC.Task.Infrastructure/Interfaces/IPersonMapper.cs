using TBC.Task.Core.PersonEntity;

namespace TBC.Task.Infrastructure.Interfaces
{
    public interface IPersonMapper
    {
        Database.Tables.Person DomainToModel(Person person);
        Task<Person> ModelToDomain(Database.Tables.Person personModel);
    }
}