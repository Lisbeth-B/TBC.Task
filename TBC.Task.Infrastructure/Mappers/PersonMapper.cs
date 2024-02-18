using TBC.Task.Core.Enums;
using TBC.Task.Core.Interfaces;
using TBC.Task.Core.PersonEntity;
using TBC.Task.Infrastructure.Interfaces;
using PersonModel = TBC.Task.Infrastructure.Database.Tables.Person;
using PhoneNumberModel = TBC.Task.Infrastructure.Database.Tables.PhoneNumber;
using RelationModel = TBC.Task.Infrastructure.Database.Tables.Relation;

namespace TBC.Task.Infrastructure.Mappers
{
    public class PersonMapper(ICityRepository cityRepository) : IPersonMapper
    {
        public PersonModel DomainToModel(Person person)
        {
            PersonModel personModel = new()
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                Sex = (int)person.Sex,
                PersonalNumber = person.PersonalNumber,
                BirthDate = person.DateOfBirth,
                CityId = person.City?.Id,
                Image = person.Image
            };

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

            if (person.RelatedPeople is not null)
            {
                List<RelationModel> relationships = [];

                foreach (RelatedPerson relatedPerson in person.RelatedPeople)
                {
                    relationships.Add(new RelationModel
                    {
                        PersonId = person.Id,
                        RelatedPersonId = relatedPerson.RelatedPersonId,
                        RelationType = (int)relatedPerson.RelationType
                    });
                }
            }

            return personModel;
        }

        public async Task<Person> ModelToDomain(PersonModel personModel)
        {
            Person person = new Person
            {
                Id = personModel.Id,
                FirstName = personModel.FirstName,
                LastName = personModel.LastName,
                Sex = (Sex)personModel.Sex,
                PersonalNumber = personModel.PersonalNumber,
                DateOfBirth = personModel.BirthDate,
                City = personModel.City is null ? null : await cityRepository.GetCity(personModel.City.Id),
                PhoneNumbers = personModel.PhoneNumbers.Select(x => new PhoneNumber { Number = x.Number, Type = (PhoneNumberType)x.Type }).ToList(),
                Image = personModel.Image,
                RelatedPeople = personModel.Relationships.Select(x => new RelatedPerson { RelatedPersonId = x.RelatedPersonId, RelationType = (RelationType)x.RelationType }).ToList(),
            };

            return person;
        }
    }
}
