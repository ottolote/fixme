using FixMe.Access.Equipment.Interface.Common;

namespace FixMe.Access.Equipment.Interface
{
    public class Equipment : StoreRequestBase
    {
        public string? EquipmentId { get; set; }
        public string? CustomerId { get; set; }
        public string? EquipmentTypeId { get; set; }
        public string? RegistrationId { get; set; }
        public bool IsRegistered { get; set; } = true;
        public bool IsEligibleForMaintenancePlan { get; set; } = true;
        public Dictionary<string, string> Attributes { get; set; } = [];
    }
}
