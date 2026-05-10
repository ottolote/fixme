using FixMe.Access.ESigning.Interface;
using FixMe.Access.ESigning.Service;

namespace FixMe.Api.Tests;

public sealed class ESigningAccessTests
{
    [Fact]
    public async Task SignatureRequestStoreReturnsAcceptedProviderState()
    {
        SignatureRequest expected = ValidRequest();
        expected.State = SignatureRequestState.Accepted;
        expected.ExternalSignatureOrderReference = "order-123";
        ESigningAccess access = new(new StubExternalESigningResource(_ => expected));

        SignatureRequest result = await access.SignatureRequestStore(ValidRequest());

        Assert.Equal(SignatureRequestState.Accepted, result.State);
        Assert.True(result.IsAccepted);
        Assert.Equal("order-123", result.ExternalSignatureOrderReference);
    }

    [Fact]
    public async Task SignatureRequestStoreRejectsInvalidRequestWithoutProviderOrderReference()
    {
        ESigningAccess access = new(new StubExternalESigningResource(_ => throw new InvalidOperationException("Provider should not be called.")));
        SignatureRequest request = ValidRequest();
        request.CustomerSignerEmail = null;

        SignatureRequest result = await access.SignatureRequestStore(request);

        Assert.Equal(SignatureRequestState.Rejected, result.State);
        Assert.False(result.IsAccepted);
        Assert.Null(result.ExternalSignatureOrderReference);
        Assert.Equal("missing-signer-data", result.FailureReason);
    }

    [Fact]
    public async Task SignatureRequestStoreReturnsRejectedWhenProviderRejectsRequest()
    {
        ESigningAccess access = new(new StubExternalESigningResource(request =>
        {
            request.State = SignatureRequestState.Rejected;
            request.ExternalSignatureOrderReference = null;
            request.FailureReason = "provider-rejected";
            return request;
        }));

        SignatureRequest result = await access.SignatureRequestStore(ValidRequest());

        Assert.Equal(SignatureRequestState.Rejected, result.State);
        Assert.False(result.IsAccepted);
        Assert.Null(result.ExternalSignatureOrderReference);
        Assert.Equal("provider-rejected", result.FailureReason);
    }

    [Fact]
    public async Task SignatureRequestStoreRejectsAcceptedProviderStateWithoutOrderReference()
    {
        ESigningAccess access = new(new StubExternalESigningResource(request =>
        {
            request.State = SignatureRequestState.Accepted;
            request.ExternalSignatureOrderReference = " ";
            return request;
        }));

        SignatureRequest result = await access.SignatureRequestStore(ValidRequest());

        Assert.Equal(SignatureRequestState.Rejected, result.State);
        Assert.False(result.IsAccepted);
        Assert.Null(result.ExternalSignatureOrderReference);
        Assert.Equal("missing-external-signature-order-reference", result.FailureReason);
    }

    [Fact]
    public async Task SignatureRequestStoreReturnsFailedWhenProviderIsUnavailable()
    {
        ESigningAccess access = new(new UnavailableExternalESigningResource());

        SignatureRequest result = await access.SignatureRequestStore(ValidRequest());

        Assert.Equal(SignatureRequestState.Failed, result.State);
        Assert.False(result.IsAccepted);
        Assert.Null(result.ExternalSignatureOrderReference);
        Assert.Equal("external-esigning-unavailable", result.FailureReason);
    }

    private static SignatureRequest ValidRequest()
    {
        return new SignatureRequest
        {
            CustomerSignerName = "Ada Lovelace",
            CustomerSignerEmail = "ada@example.test",
            AgreementDocumentReference = "agreement-123",
            CallbackUrl = "https://fixme.example.test/esigning/callback",
            CorrelationId = "corr-123",
            MaintenancePlanId = "plan-123"
        };
    }

    private sealed class StubExternalESigningResource(Func<SignatureRequest, SignatureRequest> store) : IExternalESigningResource
    {
        public Task<SignatureRequest> SignatureRequestStore(SignatureRequest request)
        {
            return Task.FromResult(store(request));
        }
    }

    private sealed class UnavailableExternalESigningResource : IExternalESigningResource
    {
        public Task<SignatureRequest> SignatureRequestStore(SignatureRequest request)
        {
            throw new TimeoutException("External eSigning timed out.");
        }
    }
}
