using FixMe.Access.Customer.Interface;
using FixMe.Access.Equipment.Interface;
using FixMe.Manager.Membership.Interface;
using FixMe.Manager.Membership.Service;
using AccessPendingRegistration = FixMe.Access.Equipment.Interface.PendingRegistration;
using ManagerPendingRegistration = FixMe.Manager.Membership.Interface.PendingRegistration;

namespace FixMe.Manager.Membership.Tests;

public sealed class MembershipManagerTests
{
    [Fact]
    public async Task CreatePendingRegistrationStoresDraftWithoutEquipmentType()
    {
        RecordingCustomerAccess customerAccess = new(new Customer { CustomerId = "customer-1", IsEmailConfirmed = true });
        RecordingEquipmentAccess equipmentAccess = new();
        MembershipManager manager = new(customerAccess, equipmentAccess);

        ManagerPendingRegistration result = await manager.CreatePendingRegistration(new CreatePendingRegistrationRequest
        {
            CustomerId = "customer-1",
            EquipmentData = new Dictionary<string, string> { ["nickname"] = "daily driver" }
        });

        Assert.Null(result.Error);
        Assert.False(string.IsNullOrWhiteSpace(result.PendingRegistrationId));
        Assert.Equal("customer-1", result.CustomerId);
        Assert.Null(result.EquipmentTypeId);
        Assert.Equal(PendingRegistrationStatus.Draft.ToString(), result.Status);
        Assert.Equal(MembershipManager.PendingEquipmentRegistrationCreated, result.EventName);
        Assert.Equal("daily driver", result.EquipmentData["nickname"]);
        Assert.Equal("customer-1", customerAccess.LastCriteria?.CustomerId);
        Assert.Equal(0, equipmentAccess.EquipmentTypeFilterCalls);
        Assert.NotNull(equipmentAccess.StoredRegistration);
        Assert.Equal(PendingRegistrationStatus.Draft, equipmentAccess.StoredRegistration.Status);
    }

    [Fact]
    public async Task CreatePendingRegistrationReturnsInvalidCustomerAndDoesNotStoreDraft()
    {
        RecordingCustomerAccess customerAccess = new(null);
        RecordingEquipmentAccess equipmentAccess = new();
        MembershipManager manager = new(customerAccess, equipmentAccess);

        ManagerPendingRegistration result = await manager.CreatePendingRegistration(new CreatePendingRegistrationRequest
        {
            CustomerId = "missing-customer",
            EquipmentTypeId = "car"
        });

        Assert.Equal("invalid-customer", result.Error);
        Assert.Equal("missing-customer", customerAccess.LastCriteria?.CustomerId);
        Assert.Equal(0, equipmentAccess.EquipmentTypeFilterCalls);
        Assert.Equal(0, equipmentAccess.PendingRegistrationStoreCalls);
    }

    [Fact]
    public async Task CreatePendingRegistrationReturnsInvalidCustomerForUnconfirmedCustomer()
    {
        RecordingCustomerAccess customerAccess = new(new Customer { CustomerId = "customer-1", IsEmailConfirmed = false });
        RecordingEquipmentAccess equipmentAccess = new();
        MembershipManager manager = new(customerAccess, equipmentAccess);

        ManagerPendingRegistration result = await manager.CreatePendingRegistration(new CreatePendingRegistrationRequest
        {
            CustomerId = "customer-1"
        });

        Assert.Equal("invalid-customer", result.Error);
        Assert.Equal(0, equipmentAccess.PendingRegistrationStoreCalls);
    }

    [Fact]
    public async Task CreatePendingRegistrationValidatesSuppliedEquipmentTypeBeforeStore()
    {
        RecordingCustomerAccess customerAccess = new(new Customer { CustomerId = "customer-1", IsEmailConfirmed = true });
        RecordingEquipmentAccess equipmentAccess = new();
        MembershipManager manager = new(customerAccess, equipmentAccess);

        ManagerPendingRegistration result = await manager.CreatePendingRegistration(new CreatePendingRegistrationRequest
        {
            CustomerId = "customer-1",
            EquipmentTypeId = "spaceship"
        });

        Assert.Equal("invalid-equipment-type", result.Error);
        Assert.Equal(1, equipmentAccess.EquipmentTypeFilterCalls);
        Assert.Equal("spaceship", equipmentAccess.LastEquipmentTypeCriteria?.EquipmentTypeId);
        Assert.Equal(0, equipmentAccess.PendingRegistrationStoreCalls);
    }

    [Fact]
    public async Task CreatePendingRegistrationDoesNotCreateBackofficeTask()
    {
        RecordingCustomerAccess customerAccess = new(new Customer { CustomerId = "customer-1", IsEmailConfirmed = true });
        RecordingEquipmentAccess equipmentAccess = new(new EquipmentType { EquipmentTypeId = "car", Code = "car" });
        MembershipManager manager = new(customerAccess, equipmentAccess);

        ManagerPendingRegistration result = await manager.CreatePendingRegistration(new CreatePendingRegistrationRequest
        {
            CustomerId = "customer-1",
            EquipmentTypeId = "car"
        });

        Assert.Null(result.Error);
        Assert.Equal(MembershipManager.PendingEquipmentRegistrationCreated, result.EventName);
        Assert.DoesNotContain(typeof(MembershipManager).GetConstructors().Single().GetParameters(), parameter =>
            parameter.ParameterType.FullName?.Contains("Tasking", StringComparison.Ordinal) == true);
    }

    private sealed class RecordingCustomerAccess : ICustomerAccess
    {
        private readonly Customer? _customer;

        public RecordingCustomerAccess(Customer? customer)
        {
            _customer = customer;
        }

        public CustomerCriteria? LastCriteria { get; private set; }

        public Task<Customer?> Filter(CustomerCriteria request)
        {
            LastCriteria = request;
            return Task.FromResult(_customer);
        }

        public Task<Customer> Store(Customer request)
        {
            throw new NotSupportedException();
        }
    }

    private sealed class RecordingEquipmentAccess : IEquipmentAccess
    {
        private readonly EquipmentType? _equipmentType;

        public RecordingEquipmentAccess(EquipmentType? equipmentType = null)
        {
            _equipmentType = equipmentType;
        }

        public int EquipmentTypeFilterCalls { get; private set; }

        public int PendingRegistrationStoreCalls { get; private set; }

        public EquipmentTypeCriteria? LastEquipmentTypeCriteria { get; private set; }

        public AccessPendingRegistration? StoredRegistration { get; private set; }

        public Task<Equipment?> Filter(EquipmentCriteria request)
        {
            throw new NotSupportedException();
        }

        public Task<EquipmentType?> Filter(EquipmentTypeCriteria request)
        {
            EquipmentTypeFilterCalls++;
            LastEquipmentTypeCriteria = request;
            return Task.FromResult(_equipmentType);
        }

        public Task<AccessPendingRegistration?> Filter(PendingRegistrationCriteria request)
        {
            throw new NotSupportedException();
        }

        public Task<AccessPendingRegistration> Store(AccessPendingRegistration request)
        {
            PendingRegistrationStoreCalls++;
            StoredRegistration = request;
            return Task.FromResult(request);
        }

        public Task<Equipment> Store(Equipment request)
        {
            throw new NotSupportedException();
        }
    }
}
