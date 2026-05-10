namespace FixMe.Access.Maintenance.Interface
{
    public class MaintenanceJobSlot
    {
        public string? SlotId { get; init; }
        public string? ProviderId { get; init; }
        public string? Status { get; init; }
        public DateTimeOffset ScheduledStart { get; init; }
        public DateTimeOffset ScheduledEnd { get; init; }
        public string? ReservationId { get; init; }
        public string? ProposalId { get; init; }
    }
}
