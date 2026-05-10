using FixMe.Access.Customer.Interface;
using FixMe.Access.Equipment.Interface;
using FixMe.Manager.Membership.Interface;
using AccessPendingRegistration = FixMe.Access.Equipment.Interface.PendingRegistration;
using ManagerPendingRegistration = FixMe.Manager.Membership.Interface.PendingRegistration;

namespace FixMe.Manager.Membership.Service;

public sealed class MembershipManager : IMembershipManager
{
    public const string PendingEquipmentRegistrationCreated = "Pending equipment registration created";

    private readonly ICustomerAccess _customerAccess;
    private readonly IEquipmentAccess _equipmentAccess;

    public MembershipManager(ICustomerAccess customerAccess, IEquipmentAccess equipmentAccess)
    {
        _customerAccess = customerAccess;
        _equipmentAccess = equipmentAccess;
    }

    public Task<RegisterAccountResponse> RegisterAccount(RegisterAccountRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ConfirmUserEmailResponse> ConfirmUserEmail(ConfirmUserEmailRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<UpdateUserPasswordResponse> UpdateUserPassword(UpdateUserPasswordRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<SetUserPreferencesResponse> SetUserPreferences(SetUserPreferencesRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ManagerPendingRegistration> CreatePendingRegistration(CreatePendingRegistrationRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.CustomerId))
        {
            return Failure("invalid-customer");
        }

        Customer? customer = await _customerAccess.Filter(new CustomerCriteria { CustomerId = request.CustomerId });
        if (customer is null || !customer.MayRegisterEquipment)
        {
            return Failure("invalid-customer");
        }

        string? equipmentTypeId = Normalize(request.EquipmentTypeId);
        if (equipmentTypeId is not null)
        {
            EquipmentType? equipmentType = await _equipmentAccess.Filter(new EquipmentTypeCriteria { EquipmentTypeId = equipmentTypeId });
            if (equipmentType is null)
            {
                return Failure("invalid-equipment-type");
            }
        }

        AccessPendingRegistration stored = await _equipmentAccess.Store(new AccessPendingRegistration
        {
            PendingRegistrationId = Guid.NewGuid().ToString("N"),
            CustomerId = request.CustomerId,
            EquipmentTypeId = equipmentTypeId,
            Status = PendingRegistrationStatus.Draft,
            EquipmentData = new Dictionary<string, string>(request.EquipmentData)
        });

        return ToManagerRegistration(stored, PendingEquipmentRegistrationCreated);
    }

    public Task<ResolvePendingRegistrationResponse> ResolvePendingRegistration(ResolvePendingRegistrationRequest request)
    {
        throw new NotImplementedException();
    }

    private static ManagerPendingRegistration Failure(string error)
    {
        return new ManagerPendingRegistration { Error = error };
    }

    private static ManagerPendingRegistration ToManagerRegistration(AccessPendingRegistration registration, string eventName)
    {
        return new ManagerPendingRegistration
        {
            PendingRegistrationId = registration.PendingRegistrationId,
            CustomerId = registration.CustomerId,
            EquipmentTypeId = registration.EquipmentTypeId,
            Status = registration.Status?.ToString(),
            EventName = eventName,
            EquipmentData = new Dictionary<string, string>(registration.EquipmentData)
        };
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value;
    }
}
