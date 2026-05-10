namespace FixMe.Access.Maintenance.Interface
{
    public class MaintenanceJobSlotsProposalCriteria
    {
        public string? ProposalId { get; init; }
        public string? MaintenancePlanId { get; init; }
        public string? CustomerId { get; init; }
        public string? Status { get; init; }
        public string? SlotId { get; init; }
    }
}
