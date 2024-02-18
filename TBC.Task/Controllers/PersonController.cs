using Microsoft.AspNetCore.Mvc;
using TBC.Task.Api.ViewModels;
using TBC.Task.Core.Enums;
using TBC.Task.Core.Interfaces;
using TBC.Task.Core.PersonEntity;

namespace TBC.Task.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PersonController(IPersonRepository personRepository, ICityRepository cityRepository) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Person>> AddPerson([FromBody] AddPersonRequest request)
        {
            Person? registeredPerson = await personRepository.GetPerson(request.PersonalNumber);
            if (registeredPerson is not null)
            {
                return BadRequest($"Person with Personal number {request.PersonalNumber} already exists.");
            }

            City? city = null;
            if (request.CityId is not null)
            {
                city = await cityRepository.GetCity(request.CityId.Value);

                if (city is null)
                {
                    return BadRequest($"Count not found city with Id {request.CityId}");
                }
            }

            List<Core.PersonEntity.PhoneNumber> phoneNumbers = [];
            if (request.PhoneNumbers is not null)
            {
                foreach (ViewModels.PhoneNumber phoneNumber in request.PhoneNumbers)
                {
                    if (!Enum.IsDefined(typeof(PhoneNumberType), phoneNumber.Type))
                    {
                        return BadRequest($"PhoneNumberType {phoneNumber.Type} is incorrect. Values can be: Mobile - 0, Office - 1, House - 2.");
                    }

                    phoneNumbers.Add(new Core.PersonEntity.PhoneNumber { Number = phoneNumber.Number, Type = (PhoneNumberType)phoneNumber.Type });
                }
            }

            Person person = new()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Sex = (Sex)request.Sex,
                PersonalNumber = request.PersonalNumber,
                DateOfBirth = request.DateOfBirth,
                City = city,
                PhoneNumbers = phoneNumbers
            };

            await personRepository.AddPerson(person);
            return Ok(person);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePerson([FromBody] UpdatePersonRequest request)
        {
            Person? person = await personRepository.GetPerson(request.PersonId);

            if (person is null)
            {
                return BadRequest($"Person with id {request.PersonId} not found.");
            }

            City? city = null;
            if (request.CityId is not null)
            {
                city = await cityRepository.GetCity(request.CityId.Value);

                if (city is null)
                {
                    return BadRequest($"Count not found city with Id {request.CityId}");
                }
            }

            List<Core.PersonEntity.PhoneNumber> phoneNumbers = [];
            if (request.PhoneNumbers is not null)
            {
                foreach (ViewModels.PhoneNumber phoneNumber in request.PhoneNumbers)
                {
                    if (!Enum.IsDefined(typeof(PhoneNumberType), phoneNumber.Type))
                    {
                        return BadRequest($"PhoneNumberType {phoneNumber.Type} is incorrect. Values can be: Mobile - 0, Office - 1, House - 2.");
                    }

                    phoneNumbers.Add(new Core.PersonEntity.PhoneNumber { Number = phoneNumber.Number, Type = (PhoneNumberType)phoneNumber.Type });
                }
            }

            person.FirstName = request.FirstName;
            person.LastName = request.LastName;
            person.Sex = (Sex)request.Sex;
            person.PersonalNumber = request.PersonalNumber;
            person.DateOfBirth = request.DateOfBirth;
            person.City = city;

            await personRepository.UpdatePerson(person);
            return Ok(person);
        }

        [HttpPut]
        public async Task<ActionResult<Person>> UpdatePersonImage([FromQuery] UpdatePersonImageRequest request)
        {
            Person? person = await personRepository.GetPerson(request.PersonId);
            if (person is null)
            {
                return BadRequest($"Person with id {request.PersonId} not found.");
            }

            if (request.Image is null || request.Image.Length == 0)
            {
                return BadRequest("Image not found");
            }

            await personRepository.UpdatePersonImage(request.PersonId, request.Image);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Person>> AddRelatedPerson([FromQuery] AddRelatedPersonRequest request)
        {
            Person? person = await personRepository.GetPerson(request.PersonId);
            if (person is null)
            {
                return BadRequest($"Person with id {request.PersonId} not found.");
            }

            if (person.RelatedPeople.Any(x => x.RelatedPersonId == request.RelatedPersonId))
            {
                return BadRequest($"This relation already exists");
            }

            Person? relatedPerson = await personRepository.GetPerson(request.RelatedPersonId);
            if (relatedPerson is null)
            {
                return BadRequest($"Related person with id {request.RelatedPersonId} not found.");
            }

            if (!Enum.IsDefined(typeof(RelationType), request.RelationType))
            {
                return BadRequest($"Relation type does not exist.");
            }

            person.RelatedPeople.Add(new RelatedPerson
            {
                RelatedPersonId = relatedPerson.Id,
                RelationType = (RelationType)request.RelationType
            });

            await personRepository.AddRelatedPerson(request.PersonId, request.RelatedPersonId, (RelationType)request.RelationType);

            return Ok(person);
        }

        [HttpDelete]
        public async Task<ActionResult<Person>> DeleteRelatedPerson([FromQuery] DeleteRelatedPersonRequest request)
        {
            Person? person = await personRepository.GetPerson(request.PersonId);
            if (person is null)
            {
                return BadRequest($"Person with id {request.PersonId} not found.");
            }

            Person? relatedPerson = await personRepository.GetPerson(request.RelatedPersonId);
            if (relatedPerson is null)
            {
                return BadRequest($"Related person with id {request.RelatedPersonId} not found.");
            }

            RelatedPerson? relatedPersonToDelete = person.RelatedPeople.FirstOrDefault(x => x.RelatedPersonId == request.RelatedPersonId);

            if (relatedPersonToDelete is null)
            {
                return BadRequest("Relationship does not exist.");
            }

            person.RelatedPeople.Remove(relatedPersonToDelete);

            await personRepository.DeleteRelatedPerson(request.PersonId, request.RelatedPersonId);
            return Ok(person);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            Person? person = await personRepository.GetPerson(id);
            if (person is null)
            {
                return BadRequest($"Person with id {id} not found.");
            }

            await personRepository.DeletePerson(id);
            return Ok();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Person>> GetPerson(int id)
        {
            Person? person = await personRepository.GetPerson(id);

            if (person is null)
            {
                return BadRequest($"Person with id {id} not found");
            }

            return Ok(person);
        }

        [HttpGet]
        public async Task<ActionResult<List<Task<Person>>>> GetAll([FromQuery] string keyword, int itemCount, int pageNumber)
        {
            List<Task<Person>> people = await personRepository.GetAll(keyword, itemCount, pageNumber);
            return Ok(people);
        }

        [HttpGet]
        public async Task<ActionResult<List<RelatedPeopleCountByType>>> GetRelatedPeopleReport()
        {
            List<RelatedPeopleCountByType> relations = await personRepository.GetAllRelations();
            return Ok(relations);
        }
    }
}
