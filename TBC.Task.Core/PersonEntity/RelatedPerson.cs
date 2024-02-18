using TBC.Task.Core.Enums;

namespace TBC.Task.Core.PersonEntity
{
    public class RelatedPerson
    {
        public required int RelatedPersonId { get; set; }
        public required RelationType RelationType { get; set; }
    }
}
