using FixMe.Access.Equipment.Interface;
using EquipmentModel = FixMe.Access.Equipment.Interface.Equipment;

namespace FixMe.Access.Equipment.Service
{
    public class EquipmentAccess : IEquipmentAccess
    {
        private readonly EquipmentResource _resource;

        public EquipmentAccess()
            : this(new EquipmentResource())
        {
        }

        public EquipmentAccess(string resourceConnectionString)
            : this(new EquipmentResource(resourceConnectionString))
        {
        }

        internal EquipmentAccess(EquipmentResource resource)
        {
            _resource = resource;
        }

        public Task<EquipmentModel?> Filter(EquipmentCriteria request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return Task.FromResult(_resource.Filter(request));
        }

        public Task<EquipmentType?> Filter(EquipmentTypeCriteria request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return Task.FromResult(_resource.Filter(request));
        }

        public Task<PendingRegistration?> Filter(PendingRegistrationCriteria request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return Task.FromResult(_resource.Filter(request));
        }

        public Task<PendingRegistration> Store(PendingRegistration request)
        {
            ArgumentNullException.ThrowIfNull(request);
            Validate(request);

            return Task.FromResult(_resource.Store(request));
        }

        public Task<EquipmentModel> Store(EquipmentModel request)
        {
            ArgumentNullException.ThrowIfNull(request);
            Validate(request);

            return Task.FromResult(_resource.Store(request));
        }

        private static void Validate(PendingRegistration registration)
        {
            Require(registration.PendingRegistrationId, nameof(registration.PendingRegistrationId));
            Require(registration.CustomerId, nameof(registration.CustomerId));

            if (registration.Status is null)
            {
                throw new ArgumentException("Pending registration status is required.", nameof(registration));
            }

            if (registration.Status != PendingRegistrationStatus.Draft)
            {
                Require(registration.EquipmentTypeId, nameof(registration.EquipmentTypeId));
            }

            if (registration.Status == PendingRegistrationStatus.Accepted && registration.Decision != RegistrationDecision.Accepted)
            {
                throw new ArgumentException("Accepted registrations require an accepted decision.", nameof(registration));
            }

            if (registration.Status == PendingRegistrationStatus.Rejected && registration.Decision != RegistrationDecision.Rejected)
            {
                throw new ArgumentException("Rejected registrations require a rejected decision.", nameof(registration));
            }
        }

        private static void Validate(EquipmentModel equipment)
        {
            Require(equipment.EquipmentId, nameof(equipment.EquipmentId));
            Require(equipment.CustomerId, nameof(equipment.CustomerId));
            Require(equipment.EquipmentTypeId, nameof(equipment.EquipmentTypeId));
            Require(equipment.RegistrationId, nameof(equipment.RegistrationId));
        }

        private static void Require(string? value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException($"{name} is required.", name);
            }
        }
    }
}
