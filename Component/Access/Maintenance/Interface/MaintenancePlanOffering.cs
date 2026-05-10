namespace FixMe.Access.Maintenance.Interface
{
    public class MaintenancePlanOffering
    {
        public string? OfferingId { get; init; }
        public string? EquipmentType { get; init; }
        public string? Market { get; init; }
        public bool IsAvailable { get; init; }
        public decimal Price { get; init; }
    }
}
