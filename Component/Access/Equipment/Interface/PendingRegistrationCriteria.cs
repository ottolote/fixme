using FixMe.Access.Equipment.Interface.Common;

namespace FixMe.Access.Equipment.Interface
{
    public class PendingRegistrationCriteria : FilterRequestBase
    {
        public string? PendingRegistrationId { get; set; }
        public string? CustomerId { get; set; }
        public string? EquipmentTypeId { get; set; }
        public PendingRegistrationStatus? Status { get; set; }
    }
}
