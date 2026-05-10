using FixMe.Access.Equipment.Interface;
using FixMe.Access.Equipment.Service;
using EquipmentModel = FixMe.Access.Equipment.Interface.Equipment;

namespace FixMe.Access.Equipment.Tests;

public sealed class EquipmentAccessTests
{
    [Fact]
    public async Task FilterEquipmentReturnsStoredMatch()
    {
        EquipmentAccess access = new();
        EquipmentModel equipment = new()
        {
            EquipmentId = "equipment-1",
            CustomerId = "customer-1",
            EquipmentTypeId = "car",
            RegistrationId = "registration-1"
        };

        EquipmentModel stored = await access.Store(equipment);
        EquipmentModel? result = await access.Filter(new EquipmentCriteria { EquipmentId = "equipment-1", CustomerId = "customer-1" });

        Assert.Same(equipment, stored);
        Assert.Same(equipment, result);
    }

    [Fact]
    public async Task FilterEquipmentReturnsNullWhenNoEquipmentMatches()
    {
        EquipmentAccess access = new();

        EquipmentModel? result = await access.Filter(new EquipmentCriteria { EquipmentId = "missing" });

        Assert.Null(result);
    }

    [Fact]
    public async Task FilterEquipmentTypeReturnsSupportedMatch()
    {
        EquipmentAccess access = new();

        EquipmentType? result = await access.Filter(new EquipmentTypeCriteria { Code = "car" });

        Assert.NotNull(result);
        Assert.Equal("car", result.Code);
        Assert.True(result.IsSupported);
    }

    [Fact]
    public async Task FilterEquipmentTypeReturnsNullWhenUnsupported()
    {
        EquipmentAccess access = new();

        EquipmentType? result = await access.Filter(new EquipmentTypeCriteria { Code = "spaceship" });

        Assert.Null(result);
    }

    [Fact]
    public async Task FilterPendingRegistrationReturnsStoredMatch()
    {
        EquipmentAccess access = new();
        PendingRegistration registration = new()
        {
            PendingRegistrationId = "registration-1",
            CustomerId = "customer-1",
            EquipmentTypeId = "boat",
            Status = PendingRegistrationStatus.PendingReview
        };

        PendingRegistration stored = await access.Store(registration);
        PendingRegistration? result = await access.Filter(new PendingRegistrationCriteria
        {
            PendingRegistrationId = "registration-1",
            Status = PendingRegistrationStatus.PendingReview
        });

        Assert.Same(registration, stored);
        Assert.Same(registration, result);
    }

    [Fact]
    public async Task FilterPendingRegistrationReturnsNullWhenNoRegistrationMatches()
    {
        EquipmentAccess access = new();

        PendingRegistration? result = await access.Filter(new PendingRegistrationCriteria { PendingRegistrationId = "missing" });

        Assert.Null(result);
    }

    [Fact]
    public async Task StorePendingRegistrationRejectsMissingRequiredState()
    {
        EquipmentAccess access = new();
        PendingRegistration registration = new()
        {
            PendingRegistrationId = "registration-1",
            CustomerId = "customer-1",
            EquipmentTypeId = "car",
            Status = PendingRegistrationStatus.Accepted
        };

        await Assert.ThrowsAsync<ArgumentException>(() => access.Store(registration));
        PendingRegistration? result = await access.Filter(new PendingRegistrationCriteria { PendingRegistrationId = "registration-1" });
        Assert.Null(result);
    }

    [Fact]
    public async Task StoreEquipmentRejectsMissingRequiredIdentity()
    {
        EquipmentAccess access = new();
        EquipmentModel equipment = new()
        {
            EquipmentId = "equipment-1",
            CustomerId = "customer-1",
            EquipmentTypeId = "car"
        };

        await Assert.ThrowsAsync<ArgumentException>(() => access.Store(equipment));
        EquipmentModel? result = await access.Filter(new EquipmentCriteria { EquipmentId = "equipment-1" });
        Assert.Null(result);
    }
}
