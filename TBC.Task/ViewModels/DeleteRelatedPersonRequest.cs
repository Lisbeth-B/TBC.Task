using System.ComponentModel.DataAnnotations;

namespace TBC.Task.Api.ViewModels
{
    public class DeleteRelatedPersonRequest
    {
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid Person Id")]
        public int PersonId { get; init; }

        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid Related Person Id")]
        public int RelatedPersonId { get; init; }
    }
}
