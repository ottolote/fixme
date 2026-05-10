namespace FixMe.Access.ESigning.Interface
{
    public class SignatureRequest
    {
        public string? CustomerSignerName { get; set; }
        public string? CustomerSignerEmail { get; set; }
        public string? AgreementDocumentReference { get; set; }
        public string? AgreementDocumentPayload { get; set; }
        public string? CallbackUrl { get; set; }
        public string? CorrelationId { get; set; }
        public string? MaintenancePlanId { get; set; }
        public string? ExternalSignatureOrderReference { get; set; }
        public SignatureRequestState State { get; set; } = SignatureRequestState.Pending;
        public string? FailureReason { get; set; }

        public bool IsAccepted => State == SignatureRequestState.Accepted && !string.IsNullOrWhiteSpace(ExternalSignatureOrderReference);
    }
}
