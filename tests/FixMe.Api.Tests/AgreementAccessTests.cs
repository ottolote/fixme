using FixMe.Access.Agreement.Interface;
using FixMe.Access.Agreement.Service;

namespace FixMe.Api.Tests;

public sealed class AgreementAccessTests
{
    [Fact]
    public async Task StorePersistsAndReturnsAgreement()
    {
        AgreementAccess access = CreateAccess();
        Agreement agreement = CreateAgreement();

        Agreement stored = await access.Store(agreement);

        Assert.Equal(agreement.AgreementId, stored.AgreementId);
        Assert.Equal(agreement.MaintenancePlanId, stored.MaintenancePlanId);
        Assert.Equal(agreement.CustomerId, stored.CustomerId);
        Assert.Equal(agreement.SignatureOrderReference, stored.SignatureOrderReference);
        Assert.Equal(agreement.DocumentReference, stored.DocumentReference);
        Assert.Equal(agreement.SignatureState, stored.SignatureState);
    }

    [Fact]
    public async Task FilterReturnsAgreementMatchingAnySupportedCriterion()
    {
        AgreementAccess access = CreateAccess();
        Agreement stored = await access.Store(CreateAgreement(signatureState: AgreementSignatureState.Signed, signedEvidenceReference: "signed://agreement-1"));

        await AssertMatches(access, new AgreementCriteria { AgreementId = stored.AgreementId }, stored);
        await AssertMatches(access, new AgreementCriteria { MaintenancePlanId = stored.MaintenancePlanId }, stored);
        await AssertMatches(access, new AgreementCriteria { CustomerId = stored.CustomerId }, stored);
        await AssertMatches(access, new AgreementCriteria { SignatureOrderReference = stored.SignatureOrderReference }, stored);
        await AssertMatches(access, new AgreementCriteria { HasSignedEvidence = true }, stored);
    }

    [Fact]
    public async Task FilterReturnsNullWhenAgreementIsNotFound()
    {
        AgreementAccess access = CreateAccess();
        await access.Store(CreateAgreement());

        Agreement? filtered = await access.Filter(new AgreementCriteria { AgreementId = "missing" });

        Assert.Null(filtered);
    }

    [Fact]
    public async Task StoreRejectsIncompleteAgreementAndDoesNotPersistPartialAgreement()
    {
        AgreementAccess access = CreateAccess();
        Agreement incomplete = CreateAgreement();
        incomplete.CustomerId = null;

        await Assert.ThrowsAsync<ArgumentException>(() => access.Store(incomplete));
        Agreement? filtered = await access.Filter(new AgreementCriteria { AgreementId = incomplete.AgreementId });

        Assert.Null(filtered);
    }

    [Fact]
    public async Task StoreRejectsSignedAgreementWithoutSignedEvidence()
    {
        AgreementAccess access = CreateAccess();
        Agreement agreement = CreateAgreement(signatureState: AgreementSignatureState.Signed);

        await Assert.ThrowsAsync<ArgumentException>(() => access.Store(agreement));
        Agreement? filtered = await access.Filter(new AgreementCriteria { AgreementId = agreement.AgreementId });

        Assert.Null(filtered);
    }

    private static AgreementAccess CreateAccess()
    {
        return new AgreementAccess(new AgreementResource());
    }

    private static Agreement CreateAgreement(
        AgreementSignatureState signatureState = AgreementSignatureState.SignatureRequested,
        string? signedEvidenceReference = null)
    {
        string id = Guid.NewGuid().ToString("N");
        return new Agreement
        {
            AgreementId = $"agreement-{id}",
            MaintenancePlanId = $"plan-{id}",
            CustomerId = $"customer-{id}",
            SignatureOrderReference = $"signature-order-{id}",
            DocumentReference = $"document://{id}",
            SignatureState = signatureState,
            SignedEvidenceReference = signedEvidenceReference
        };
    }

    private static async Task AssertMatches(AgreementAccess access, AgreementCriteria criteria, Agreement expected)
    {
        Agreement? filtered = await access.Filter(criteria);

        Assert.NotNull(filtered);
        Assert.Equal(expected.AgreementId, filtered.AgreementId);
    }
}
