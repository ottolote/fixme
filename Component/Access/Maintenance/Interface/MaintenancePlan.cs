namespace FixMe.Access.Maintenance.Interface
{
    public class MaintenancePlan
    {
        public string? PlanId { get; init; }
        public string? EquipmentId { get; init; }
        public string? OfferingId { get; init; }
        public string? CustomerId { get; init; }
        public string? Status { get; init; }
        public decimal LockedPrice { get; init; }
        public string? SignatureReference { get; init; }
        public string? SignedEvidence { get; init; }
        public string? RejectionReason { get; init; }
    }
}
