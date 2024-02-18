using System.ComponentModel.DataAnnotations;

namespace TBC.Task.Api.ViewModels
{
    public class UpdatePersonImageRequest
    {
        [Range(0, int.MaxValue, ErrorMessage = "Please enter valid Person Id")]
        public required int PersonId { get; set; }
        public required IFormFile Image { get; set; }
    }
}