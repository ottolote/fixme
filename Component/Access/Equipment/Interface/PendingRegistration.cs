using FixMe.Access.Equipment.Interface.Common;

namespace FixMe.Access.Equipment.Interface
{
    public class PendingRegistration : StoreRequestBase
    {
        public string? PendingRegistrationId { get; set; }
        public string? CustomerId { get; set; }
        public string? EquipmentTypeId { get; set; }
        public PendingRegistrationStatus? Status { get; set; }
        public RegistrationDecision? Decision { get; set; }
        public string? RejectionReason { get; set; }
        public Dictionary<string, string> EquipmentData { get; set; } = [];
    }
}
