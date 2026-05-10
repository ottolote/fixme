using FixMe.Access.ESigning.Interface;

namespace FixMe.Access.ESigning.Service
{
    public class ESigningAccess : IESigningAccess
    {
        private readonly IExternalESigningResource _externalESigningResource;

        public ESigningAccess()
            : this(new ExternalESigningResource())
        {
        }

        public ESigningAccess(IExternalESigningResource externalESigningResource)
        {
            _externalESigningResource = externalESigningResource;
        }

        public async Task<SignatureRequest> SignatureRequestStore(SignatureRequest request)
        {
            if (!IsValid(request, out string? failureReason))
            {
                return Reject(request, failureReason);
            }

            try
            {
                SignatureRequest storedRequest = await _externalESigningResource.SignatureRequestStore(request).ConfigureAwait(false);

                if (storedRequest.State == SignatureRequestState.Accepted && string.IsNullOrWhiteSpace(storedRequest.ExternalSignatureOrderReference))
                {
                    storedRequest.State = SignatureRequestState.Rejected;
                    storedRequest.ExternalSignatureOrderReference = null;
                    storedRequest.FailureReason = "missing-external-signature-order-reference";
                }
                else if (storedRequest.State != SignatureRequestState.Accepted)
                {
                    storedRequest.State = storedRequest.State == SignatureRequestState.Pending
                        ? SignatureRequestState.Rejected
                        : storedRequest.State;
                    storedRequest.ExternalSignatureOrderReference = null;
                }

                return storedRequest;
            }
            catch (Exception exception) when (exception is TimeoutException or HttpRequestException or TaskCanceledException or OperationCanceledException)
            {
                request.State = SignatureRequestState.Failed;
                request.ExternalSignatureOrderReference = null;
                request.FailureReason = "external-esigning-unavailable";
                return request;
            }
        }

        private static bool IsValid(SignatureRequest request, out string? failureReason)
        {
            failureReason = null;

            if (string.IsNullOrWhiteSpace(request.CustomerSignerName) || string.IsNullOrWhiteSpace(request.CustomerSignerEmail))
            {
                failureReason = "missing-signer-data";
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.AgreementDocumentReference) && string.IsNullOrWhiteSpace(request.AgreementDocumentPayload))
            {
                failureReason = "missing-agreement-document-data";
                return false;
            }

            if (!Uri.TryCreate(request.CallbackUrl, UriKind.Absolute, out _) || string.IsNullOrWhiteSpace(request.CorrelationId))
            {
                failureReason = "invalid-callback-or-correlation-data";
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.MaintenancePlanId))
            {
                failureReason = "missing-maintenance-plan-context";
                return false;
            }

            return true;
        }

        private static SignatureRequest Reject(SignatureRequest request, string? failureReason)
        {
            request.State = SignatureRequestState.Rejected;
            request.ExternalSignatureOrderReference = null;
            request.FailureReason = failureReason;
            return request;
        }
    }
}
