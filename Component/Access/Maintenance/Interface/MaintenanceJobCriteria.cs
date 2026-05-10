namespace FixMe.Access.Maintenance.Interface
{
    public class MaintenanceJobCriteria
    {
        public string? JobId { get; init; }
        public string? MaintenancePlanId { get; init; }
        public string? EquipmentId { get; init; }
        public string? SelectedSlotId { get; init; }
        public string? Status { get; init; }
    }
}
