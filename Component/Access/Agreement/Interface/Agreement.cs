namespace FixMe.Access.Agreement.Interface
{
    public class Agreement
    {
        public string? AgreementId { get; set; }
        public string? MaintenancePlanId { get; set; }
        public string? CustomerId { get; set; }
        public string? SignatureOrderReference { get; set; }
        public string? DocumentReference { get; set; }
        public string? DocumentPayload { get; set; }
        public AgreementSignatureState? SignatureState { get; set; }
        public string? SignedEvidenceReference { get; set; }
        public string? SignedEvidencePayload { get; set; }
        public bool HasSignedEvidence => !string.IsNullOrWhiteSpace(SignedEvidenceReference)
            || !string.IsNullOrWhiteSpace(SignedEvidencePayload);
    }
}
