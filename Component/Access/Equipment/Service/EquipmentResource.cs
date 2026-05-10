using FixMe.Access.Equipment.Interface;
using EquipmentModel = FixMe.Access.Equipment.Interface.Equipment;

namespace FixMe.Access.Equipment.Service
{
    internal sealed class EquipmentResource
    {
        private readonly object _syncRoot = new();
        private readonly Dictionary<string, EquipmentModel> _equipment = [];
        private readonly Dictionary<string, PendingRegistration> _pendingRegistrations = [];
        private readonly List<EquipmentType> _equipmentTypes =
        [
            new EquipmentType { EquipmentTypeId = "car", Code = "car", Name = "Car" },
            new EquipmentType { EquipmentTypeId = "e-bike", Code = "e-bike", Name = "E-bike" },
            new EquipmentType { EquipmentTypeId = "lawnmower", Code = "lawnmower", Name = "Lawnmower" },
            new EquipmentType { EquipmentTypeId = "boat", Code = "boat", Name = "Boat" },
            new EquipmentType { EquipmentTypeId = "other", Code = "other", Name = "Other" }
        ];

        public EquipmentModel? Filter(EquipmentCriteria criteria)
        {
            lock (_syncRoot)
            {
                return _equipment.Values.FirstOrDefault(e => Matches(e, criteria));
            }
        }

        public EquipmentType? Filter(EquipmentTypeCriteria criteria)
        {
            lock (_syncRoot)
            {
                return _equipmentTypes.FirstOrDefault(t => Matches(t, criteria));
            }
        }

        public PendingRegistration? Filter(PendingRegistrationCriteria criteria)
        {
            lock (_syncRoot)
            {
                return _pendingRegistrations.Values.FirstOrDefault(r => Matches(r, criteria));
            }
        }

        public PendingRegistration Store(PendingRegistration registration)
        {
            lock (_syncRoot)
            {
                _pendingRegistrations[registration.PendingRegistrationId!] = registration;
                return registration;
            }
        }

        public EquipmentModel Store(EquipmentModel equipment)
        {
            lock (_syncRoot)
            {
                _equipment[equipment.EquipmentId!] = equipment;
                return equipment;
            }
        }

        private static bool Matches(EquipmentModel equipment, EquipmentCriteria criteria)
        {
            return Matches(criteria.EquipmentId, equipment.EquipmentId)
                && Matches(criteria.CustomerId, equipment.CustomerId)
                && Matches(criteria.EquipmentTypeId, equipment.EquipmentTypeId)
                && Matches(criteria.RegistrationId, equipment.RegistrationId)
                && Matches(criteria.IsRegistered, equipment.IsRegistered)
                && Matches(criteria.IsEligibleForMaintenancePlan, equipment.IsEligibleForMaintenancePlan);
        }

        private static bool Matches(EquipmentType equipmentType, EquipmentTypeCriteria criteria)
        {
            return equipmentType.IsSupported
                && Matches(criteria.EquipmentTypeId, equipmentType.EquipmentTypeId)
                && Matches(criteria.Code, equipmentType.Code);
        }

        private static bool Matches(PendingRegistration registration, PendingRegistrationCriteria criteria)
        {
            return Matches(criteria.PendingRegistrationId, registration.PendingRegistrationId)
                && Matches(criteria.CustomerId, registration.CustomerId)
                && Matches(criteria.EquipmentTypeId, registration.EquipmentTypeId)
                && Matches(criteria.Status, registration.Status);
        }

        private static bool Matches(string? expected, string? actual)
        {
            return string.IsNullOrWhiteSpace(expected) || string.Equals(expected, actual, StringComparison.OrdinalIgnoreCase);
        }

        private static bool Matches<T>(T? expected, T actual)
            where T : struct
        {
            return expected is null || EqualityComparer<T>.Default.Equals(expected.Value, actual);
        }

        private static bool Matches<T>(T? expected, T? actual)
            where T : struct
        {
            return expected is null || EqualityComparer<T>.Default.Equals(expected.Value, actual.GetValueOrDefault());
        }
    }
}
