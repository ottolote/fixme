namespace FixMe.Access.Agreement.Interface
{
    public class AgreementCriteria
    {
        public string? AgreementId { get; set; }
        public string? MaintenancePlanId { get; set; }
        public string? CustomerId { get; set; }
        public string? SignatureOrderReference { get; set; }
        public bool? HasSignedEvidence { get; set; }
    }
}
