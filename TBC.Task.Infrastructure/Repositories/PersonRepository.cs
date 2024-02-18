using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TBC.Task.Core.Enums;
using TBC.Task.Core.Interfaces;
using TBC.Task.Core.PersonEntity;
using TBC.Task.Infrastructure.Database;
using TBC.Task.Infrastructure.Interfaces;
using PersonModel = TBC.Task.Infrastructure.Database.Tables.Person;
using PhoneNumberModel = TBC.Task.Infrastructure.Database.Tables.PhoneNumber;
using RelationModel = TBC.Task.Infrastructure.Database.Tables.Relation;

namespace TBC.Task.Infrastructure.Repositories
{
    public class PersonRepository(DBContext dbContext, IPersonMapper personMapper, IConfiguration configuration) : IPersonRepository
    {
        public async System.Threading.Tasks.Task AddPerson(Person request)
        {
            PersonModel personModel = personMapper.DomainToModel(request);
            dbContext.Persons.Add(personModel);
            await dbContext.SaveChangesAsync();

            request.Id = personModel.Id;
        }

        public async Task<Person?> GetPerson(int id)
        {
            PersonModel? personModel = await dbContext.Persons
                .Include("PhoneNumbers")
                .Include("Relationships")
                .FirstOrDefaultAsync(p => p.Id == id);

            if (personModel == null)
            {
                return null;
            }

            Person result = await personMapper.ModelToDomain(personModel);
            return result;
        }

        public async System.Threading.Tasks.Task UpdatePerson(Person person)
        {
            PersonModel personModel = await dbContext.Persons.FirstAsync(p => p.Id == person.Id);

            personModel.FirstName = person.FirstName;
            personModel.LastName = person.LastName;
            personModel.Sex = (int)person.Sex;
            personModel.PersonalNumber = person.PersonalNumber;
            personModel.BirthDate = person.DateOfBirth;
            personModel.CityId = person.City?.Id;

            if (person.PhoneNumbers is not null)
            {
                List<PhoneNumberModel> phoneNumbers = [];
                foreach (PhoneNumber phoneNumber in person.PhoneNumbers)
                {
                    phoneNumbers.Add(new PhoneNumberModel
                    {
                        PersonId = person.Id,
                        Type = (int)phoneNumber.Type,
                        Number = phoneNumber.Number
                    });
                }

                personModel.PhoneNumbers.AddRange(phoneNumbers);
            }

            await dbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task AddRelatedPerson(int personId, int relatedPersonId, RelationType relationType)
        {
            RelationModel relation = new RelationModel
            {
                PersonId = personId,
                RelatedPersonId = relatedPersonId,
                RelationType = (int)relationType
            };

            dbContext.Relationships.Add(relation);
            await dbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeleteRelatedPerson(int personId, int relatedPersonId)
        {
            RelationModel relationship = dbContext.Relationships.First(r => r.PersonId == personId && r.RelatedPersonId == relatedPersonId);

            dbContext.Relationships.Remove(relationship);
            await dbContext.SaveChangesAsync();
        }

        public async System.Threading.Tasks.Task DeletePerson(int id)
        {
            PersonModel personModel = await dbContext.Persons.FirstAsync(p => p.Id == id);

            dbContext.Persons.Remove(personModel);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<Task<Person>>> GetAll(string keyword, int itemCount, int pageNumber)
        {
            List<PersonModel> people = await dbContext.Persons
                        .Include(x => x.City)
                        .Include(x => x.PhoneNumbers)
                        .Include(x => x.Relationships)
                        .Where(x => (x.FirstName + x.LastName + x.PersonalNumber).Contains(keyword))
                        .Skip(itemCount * pageNumber)
                        .Take(itemCount)
                        .ToListAsync();

            List<Task<Person>> result = people.Select(x => personMapper.ModelToDomain(x)).ToList();
            return result;
        }

        public async Task<Person?> GetPerson(string personalNumber)
        {
            PersonModel? personModel = await dbContext.Persons.FirstOrDefaultAsync(p => p.PersonalNumber == personalNumber);

            if (personModel == null)
            {
                return null;
            }

            Person result = await personMapper.ModelToDomain(personModel);
            return result;
        }

        public Task<List<RelatedPeopleCountByType>> GetAllRelations()
        {
            Task<List<RelatedPeopleCountByType>> result = dbContext.Relationships
                .GroupBy(x => new { x.PersonId, x.RelationType })
                .Select(g => new RelatedPeopleCountByType
                {
                    PersonId = g.Key.PersonId,
                    RelationType = (RelationType)g.Key.RelationType,
                    Count = g.Count()
                })
                .ToListAsync();

            return result;
        }

        public async System.Threading.Tasks.Task UpdatePersonImage(int personId, IFormFile image)
        {
            string? imageStoragePath = configuration["ImageStorage:Path"];
            Directory.CreateDirectory(imageStoragePath);

            string imagePath = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string path = Path.Combine(imageStoragePath, imagePath);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
                stream.Close();
            }

            PersonModel personModel = await dbContext.Persons.FirstAsync(p => p.Id == personId);
            personModel.Image = imagePath;
            await dbContext.SaveChangesAsync();
        }
    }
}
