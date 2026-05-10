namespace FixMe.Manager.Membership.Interface
{
    public class PendingRegistration
    {
        public string? PendingRegistrationId { get; set; }

        public string? CustomerId { get; set; }

        public string? EquipmentTypeId { get; set; }

        public string? Status { get; set; }

        public string? Error { get; set; }

        public string? EventName { get; set; }

        public Dictionary<string, string> EquipmentData { get; set; } = [];
    }
}
