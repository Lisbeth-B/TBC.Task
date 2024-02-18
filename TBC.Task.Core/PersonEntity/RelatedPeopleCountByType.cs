using TBC.Task.Core.Enums;

namespace TBC.Task.Core.PersonEntity
{
    public class RelatedPeopleCountByType
    {
        public int PersonId { get; set; }
        public RelationType RelationType { get; set; }
        public int Count { get; set; }
    }
}
