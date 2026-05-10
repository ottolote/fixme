using FixMe.Access.ESigning.Interface;

namespace FixMe.Access.ESigning.Service
{
    public class ExternalESigningResource : IExternalESigningResource
    {
        public Task<SignatureRequest> SignatureRequestStore(SignatureRequest request)
        {
            if (request.State is SignatureRequestState.Rejected or SignatureRequestState.Failed)
            {
                return Task.FromResult(request);
            }

            request.State = SignatureRequestState.Accepted;
            request.ExternalSignatureOrderReference = string.IsNullOrWhiteSpace(request.ExternalSignatureOrderReference)
                ? $"esign-{Guid.NewGuid():N}"
                : request.ExternalSignatureOrderReference;
            request.FailureReason = null;

            return Task.FromResult(request);
        }
    }
}
