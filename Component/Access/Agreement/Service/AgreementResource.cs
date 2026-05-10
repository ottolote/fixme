using FixMe.Access.Agreement.Interface;
using AgreementModel = FixMe.Access.Agreement.Interface.Agreement;

namespace FixMe.Access.Agreement.Service
{
    public class AgreementResource
    {
        private readonly Dictionary<string, AgreementModel> agreements = [];
        private readonly object gate = new();

        public static AgreementResource Shared { get; } = new();

        public Task<AgreementModel?> Filter(AgreementCriteria criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);

            lock (gate)
            {
                AgreementModel? agreement = agreements.Values.FirstOrDefault(agreement => Matches(agreement, criteria));
                return Task.FromResult(agreement is null ? null : Copy(agreement));
            }
        }

        public Task<AgreementModel> Store(AgreementModel agreement)
        {
            Validate(agreement);

            AgreementModel stored = Copy(agreement);
            stored.AgreementId ??= Guid.NewGuid().ToString("N");

            lock (gate)
            {
                agreements[stored.AgreementId] = Copy(stored);
            }

            return Task.FromResult(stored);
        }

        private static bool Matches(AgreementModel agreement, AgreementCriteria criteria)
        {
            return Matches(criteria.AgreementId, agreement.AgreementId)
                && Matches(criteria.MaintenancePlanId, agreement.MaintenancePlanId)
                && Matches(criteria.CustomerId, agreement.CustomerId)
                && Matches(criteria.SignatureOrderReference, agreement.SignatureOrderReference)
                && (!criteria.HasSignedEvidence.HasValue || criteria.HasSignedEvidence.Value == agreement.HasSignedEvidence);
        }

        private static bool Matches(string? expected, string? actual)
        {
            return string.IsNullOrWhiteSpace(expected) || string.Equals(expected, actual, StringComparison.Ordinal);
        }

        private static void Validate(AgreementModel agreement)
        {
            ArgumentNullException.ThrowIfNull(agreement);

            if (string.IsNullOrWhiteSpace(agreement.MaintenancePlanId))
            {
                throw new ArgumentException("Maintenance plan is required.", nameof(agreement));
            }

            if (string.IsNullOrWhiteSpace(agreement.CustomerId))
            {
                throw new ArgumentException("Customer is required.", nameof(agreement));
            }

            if (string.IsNullOrWhiteSpace(agreement.DocumentReference) && string.IsNullOrWhiteSpace(agreement.DocumentPayload))
            {
                throw new ArgumentException("Document payload or reference is required.", nameof(agreement));
            }

            if (!agreement.SignatureState.HasValue)
            {
                throw new ArgumentException("Signature state is required.", nameof(agreement));
            }

            if (agreement.SignatureState == AgreementSignatureState.Signed && !agreement.HasSignedEvidence)
            {
                throw new ArgumentException("Signed agreement evidence is required for signed agreements.", nameof(agreement));
            }
        }

        private static AgreementModel Copy(AgreementModel agreement)
        {
            return new AgreementModel
            {
                AgreementId = agreement.AgreementId,
                MaintenancePlanId = agreement.MaintenancePlanId,
                CustomerId = agreement.CustomerId,
                SignatureOrderReference = agreement.SignatureOrderReference,
                DocumentReference = agreement.DocumentReference,
                DocumentPayload = agreement.DocumentPayload,
                SignatureState = agreement.SignatureState,
                SignedEvidenceReference = agreement.SignedEvidenceReference,
                SignedEvidencePayload = agreement.SignedEvidencePayload
            };
        }
    }
}
