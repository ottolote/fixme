namespace FixMe.Access.Maintenance.Interface
{
    public class MaintenanceJob
    {
        public string? JobId { get; init; }
        public string? MaintenancePlanId { get; init; }
        public string? EquipmentId { get; init; }
        public string? SelectedSlotId { get; init; }
        public string? Status { get; init; }
        public DateTimeOffset ScheduledStart { get; init; }
        public DateTimeOffset? CancelledAt { get; init; }
        public string? CancellationReason { get; init; }
    }
}
