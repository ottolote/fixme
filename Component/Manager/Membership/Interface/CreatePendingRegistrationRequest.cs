namespace FixMe.Manager.Membership.Interface
{
    public class CreatePendingRegistrationRequest
    {
        public string? CustomerId { get; set; }

        public string? EquipmentTypeId { get; set; }

        public Dictionary<string, string> EquipmentData { get; set; } = [];
    }
}
