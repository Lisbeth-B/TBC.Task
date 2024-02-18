using Microsoft.AspNetCore.Http;
using TBC.Task.Core.Enums;
using TBC.Task.Core.PersonEntity;

namespace TBC.Task.Core.Interfaces;

public interface IPersonRepository
{
    System.Threading.Tasks.Task AddPerson(Person person);
    System.Threading.Tasks.Task UpdatePerson(Person person);
    System.Threading.Tasks.Task AddRelatedPerson(int personId, int relatedPersonId, RelationType relationType);
    System.Threading.Tasks.Task DeleteRelatedPerson(int personId, int relatedPersonId);
    System.Threading.Tasks.Task DeletePerson(int id);
    Task<List<Task<Person>>> GetAll(string keyword, int itemCount, int pageNumber);
    Task<Person?> GetPerson(int id);
    Task<Person?> GetPerson(string personalNumber);
    Task<List<RelatedPeopleCountByType>> GetAllRelations();
    System.Threading.Tasks.Task UpdatePersonImage(int personId, IFormFile image);
}
