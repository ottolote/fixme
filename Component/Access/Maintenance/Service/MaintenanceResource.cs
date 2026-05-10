using FixMe.Access.Maintenance.Interface;

namespace FixMe.Access.Maintenance.Service
{
    public sealed class MaintenanceResource
    {
        public static MaintenanceResource Shared { get; } = new();

        private readonly List<MaintenancePlanOffering> _offerings = [];
        private readonly List<MaintenancePlan> _plans = [];
        private readonly List<MaintenanceJobSlot> _slots = [];
        private readonly List<MaintenanceJobSlotsProposal> _proposals = [];
        private readonly List<MaintenanceJob> _jobs = [];

        public MaintenanceResource(IEnumerable<MaintenancePlanOffering>? offerings = null)
        {
            if (offerings is not null)
            {
                _offerings.AddRange(offerings);
            }
        }

        public MaintenancePlanOffering? MaintenancePlanOfferingFilter(MaintenancePlanOfferingCriteria criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);

            return _offerings.FirstOrDefault(offering =>
                Matches(criteria.OfferingId, offering.OfferingId) &&
                Matches(criteria.EquipmentType, offering.EquipmentType) &&
                Matches(criteria.Market, offering.Market) &&
                (!criteria.IsAvailable.HasValue || criteria.IsAvailable.Value == offering.IsAvailable));
        }

        public MaintenancePlan? MaintenancePlanFilter(MaintenancePlanCriteria criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);

            return _plans.FirstOrDefault(plan =>
                Matches(criteria.PlanId, plan.PlanId) &&
                Matches(criteria.EquipmentId, plan.EquipmentId) &&
                Matches(criteria.OfferingId, plan.OfferingId) &&
                Matches(criteria.CustomerId, plan.CustomerId) &&
                Matches(criteria.Status, plan.Status));
        }

        public MaintenancePlan MaintenancePlanStore(MaintenancePlan plan)
        {
            ValidateMaintenancePlan(plan);
            Upsert(_plans, plan, existing => existing.PlanId == plan.PlanId);
            return plan;
        }

        public MaintenanceJobSlot? MaintenanceJobSlotFilter(MaintenanceJobSlotCriteria criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);

            return _slots.FirstOrDefault(slot =>
                Matches(criteria.SlotId, slot.SlotId) &&
                Matches(criteria.ProviderId, slot.ProviderId) &&
                Matches(criteria.Status, slot.Status) &&
                Matches(criteria.ReservationId, slot.ReservationId) &&
                Matches(criteria.ProposalId, slot.ProposalId) &&
                (!criteria.StartsOnOrAfter.HasValue || slot.ScheduledStart >= criteria.StartsOnOrAfter.Value) &&
                (!criteria.StartsBefore.HasValue || slot.ScheduledStart < criteria.StartsBefore.Value));
        }

        public MaintenanceJobSlot MaintenanceJobSlotStore(MaintenanceJobSlot slot)
        {
            ValidateMaintenanceJobSlot(slot);
            Upsert(_slots, slot, existing => existing.SlotId == slot.SlotId);
            return slot;
        }

        public MaintenanceJobSlotsProposal? MaintenanceJobSlotsProposalFilter(MaintenanceJobSlotsProposalCriteria criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);

            return _proposals.FirstOrDefault(proposal =>
                Matches(criteria.ProposalId, proposal.ProposalId) &&
                Matches(criteria.MaintenancePlanId, proposal.MaintenancePlanId) &&
                Matches(criteria.CustomerId, proposal.CustomerId) &&
                Matches(criteria.Status, proposal.Status) &&
                (criteria.SlotId is null || proposal.SlotIds.Contains(criteria.SlotId, StringComparer.OrdinalIgnoreCase)));
        }

        public MaintenanceJobSlotsProposal MaintenanceJobSlotsProposalStore(MaintenanceJobSlotsProposal proposal)
        {
            ValidateMaintenanceJobSlotsProposal(proposal);
            Upsert(_proposals, proposal, existing => existing.ProposalId == proposal.ProposalId);
            return proposal;
        }

        public MaintenanceJob? MaintenanceJobFilter(MaintenanceJobCriteria criteria)
        {
            ArgumentNullException.ThrowIfNull(criteria);

            return _jobs.FirstOrDefault(job =>
                Matches(criteria.JobId, job.JobId) &&
                Matches(criteria.MaintenancePlanId, job.MaintenancePlanId) &&
                Matches(criteria.EquipmentId, job.EquipmentId) &&
                Matches(criteria.SelectedSlotId, job.SelectedSlotId) &&
                Matches(criteria.Status, job.Status));
        }

        public MaintenanceJob MaintenanceJobStore(MaintenanceJob job)
        {
            ValidateMaintenanceJob(job);
            Upsert(_jobs, job, existing => existing.JobId == job.JobId);
            return job;
        }

        private static bool Matches(string? criteria, string? value) =>
            criteria is null || string.Equals(criteria, value, StringComparison.OrdinalIgnoreCase);

        private static void Upsert<T>(List<T> records, T record, Predicate<T> matches)
        {
            int index = records.FindIndex(matches);
            if (index >= 0)
            {
                records[index] = record;
                return;
            }

            records.Add(record);
        }

        private static void ValidateMaintenancePlan(MaintenancePlan plan)
        {
            ArgumentNullException.ThrowIfNull(plan);
            Require(plan.PlanId, nameof(plan.PlanId));
            Require(plan.EquipmentId, nameof(plan.EquipmentId));
            Require(plan.OfferingId, nameof(plan.OfferingId));
            Require(plan.CustomerId, nameof(plan.CustomerId));
            Require(plan.Status, nameof(plan.Status));

            if (IsStatus(plan.Status, "approved-pending-signature"))
            {
                Require(plan.SignatureReference, nameof(plan.SignatureReference));
            }

            if (IsStatus(plan.Status, "active"))
            {
                Require(plan.SignedEvidence, nameof(plan.SignedEvidence));
            }
        }

        private static void ValidateMaintenanceJobSlot(MaintenanceJobSlot slot)
        {
            ArgumentNullException.ThrowIfNull(slot);
            Require(slot.SlotId, nameof(slot.SlotId));
            Require(slot.ProviderId, nameof(slot.ProviderId));
            Require(slot.Status, nameof(slot.Status));

            if (slot.ScheduledStart == default || slot.ScheduledEnd == default || slot.ScheduledEnd <= slot.ScheduledStart)
            {
                throw new ArgumentException("Maintenance job slot requires a valid schedule.", nameof(slot));
            }

            if (IsStatus(slot.Status, "reserved") || IsStatus(slot.Status, "selected"))
            {
                Require(slot.ReservationId, nameof(slot.ReservationId));
                Require(slot.ProposalId, nameof(slot.ProposalId));
            }
        }

        private static void ValidateMaintenanceJobSlotsProposal(MaintenanceJobSlotsProposal proposal)
        {
            ArgumentNullException.ThrowIfNull(proposal);
            Require(proposal.ProposalId, nameof(proposal.ProposalId));
            Require(proposal.MaintenancePlanId, nameof(proposal.MaintenancePlanId));
            Require(proposal.CustomerId, nameof(proposal.CustomerId));
            Require(proposal.Status, nameof(proposal.Status));

            if (proposal.ExpiresAt == default)
            {
                throw new ArgumentException("Maintenance job slots proposal requires an expiry.", nameof(proposal));
            }

            if (proposal.SlotIds.Count == 0)
            {
                throw new ArgumentException("Maintenance job slots proposal requires at least one slot.", nameof(proposal));
            }

            if (IsStatus(proposal.Status, "confirmed"))
            {
                _ = proposal.ConfirmedAt ?? throw new ArgumentException("Confirmed proposal requires confirmation data.", nameof(proposal));
            }

            if (IsStatus(proposal.Status, "selected"))
            {
                Require(proposal.SelectedSlotId, nameof(proposal.SelectedSlotId));
            }
        }

        private static void ValidateMaintenanceJob(MaintenanceJob job)
        {
            ArgumentNullException.ThrowIfNull(job);
            Require(job.JobId, nameof(job.JobId));
            Require(job.MaintenancePlanId, nameof(job.MaintenancePlanId));
            Require(job.EquipmentId, nameof(job.EquipmentId));
            Require(job.SelectedSlotId, nameof(job.SelectedSlotId));
            Require(job.Status, nameof(job.Status));

            if (job.ScheduledStart == default)
            {
                throw new ArgumentException("Maintenance job requires a schedule.", nameof(job));
            }

            if (IsStatus(job.Status, "cancelled"))
            {
                _ = job.CancelledAt ?? throw new ArgumentException("Cancelled maintenance job requires cancellation data.", nameof(job));
            }
        }

        private static bool IsStatus(string? status, string expected) =>
            string.Equals(status, expected, StringComparison.OrdinalIgnoreCase);

        private static void Require(string? value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{parameterName} is required.", parameterName);
            }
        }
    }
}
