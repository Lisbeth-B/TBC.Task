using TBC.Task.Core.Enums;

namespace TBC.Task.Core.PersonEntity
{
    public class PhoneNumber
    {
        public required PhoneNumberType Type { get; set; }
        public required string Number { get; set; }
    }
}
