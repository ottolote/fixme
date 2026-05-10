namespace FixMe.Access.Maintenance.Interface
{
    public class MaintenanceJobSlotsProposal
    {
        public string? ProposalId { get; init; }
        public string? MaintenancePlanId { get; init; }
        public string? CustomerId { get; init; }
        public string? Status { get; init; }
        public DateTimeOffset ExpiresAt { get; init; }
        public DateTimeOffset? ConfirmedAt { get; init; }
        public string? SelectedSlotId { get; init; }
        public IReadOnlyCollection<string> SlotIds { get; init; } = [];
    }
}
