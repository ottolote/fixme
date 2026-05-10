using FixMe.Access.Equipment.Interface.Common;

namespace FixMe.Access.Equipment.Interface
{
    public class EquipmentCriteria : FilterRequestBase
    {
        public string? EquipmentId { get; set; }
        public string? CustomerId { get; set; }
        public string? EquipmentTypeId { get; set; }
        public string? RegistrationId { get; set; }
        public bool? IsRegistered { get; set; }
        public bool? IsEligibleForMaintenancePlan { get; set; }
    }
}
