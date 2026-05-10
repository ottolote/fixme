namespace FixMe.Access.Maintenance.Interface
{
    public class MaintenanceJobSlotCriteria
    {
        public string? SlotId { get; init; }
        public string? ProviderId { get; init; }
        public string? Status { get; init; }
        public string? ReservationId { get; init; }
        public string? ProposalId { get; init; }
        public DateTimeOffset? StartsOnOrAfter { get; init; }
        public DateTimeOffset? StartsBefore { get; init; }
    }
}
