using System.ComponentModel.DataAnnotations;
using TBC.Task.Api.Utilities.Attributes;

namespace TBC.Task.Api.ViewModels
{
    public class UpdatePersonRequest
    {
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid Id")]
        public required int PersonId { get; set; }

        [Length(2, 50)]
        [FirstAndLastName]
        public required string FirstName { get; set; }

        [Length(2, 50)]
        [FirstAndLastName]
        public required string LastName { get; set; }

        [SexAttribute]
        public required int Sex { get; set; }

        [Length(11, 11)]
        public required string PersonalNumber { get; set; }

        [MinimumAge]
        public required DateOnly DateOfBirth { get; set; }

        public int? CityId { get; set; }

        public List<PhoneNumber>? PhoneNumbers { get; set; }
    }
}
