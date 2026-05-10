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
        Assert.NotNull(result);
        Assert.Equal(equipment.EquipmentId, result.EquipmentId);
        Assert.Equal(equipment.CustomerId, result.CustomerId);
        Assert.Equal(equipment.EquipmentTypeId, result.EquipmentTypeId);
        Assert.Equal(equipment.RegistrationId, result.RegistrationId);
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
        Assert.NotNull(result);
        Assert.Equal(registration.PendingRegistrationId, result.PendingRegistrationId);
        Assert.Equal(registration.CustomerId, result.CustomerId);
        Assert.Equal(registration.EquipmentTypeId, result.EquipmentTypeId);
        Assert.Equal(registration.Status, result.Status);
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

    [Fact]
    public async Task StoredEquipmentPersistsAcrossAccessInstancesUsingSameDatabase()
    {
        string connectionString = CreateConnectionString();
        EquipmentAccess writer = new(connectionString);
        EquipmentModel equipment = new()
        {
            EquipmentId = "equipment-1",
            CustomerId = "customer-1",
            EquipmentTypeId = "car",
            RegistrationId = "registration-1",
            Attributes = new Dictionary<string, string> { ["vin"] = "abc" }
        };

        await writer.Store(equipment);

        EquipmentAccess reader = new(connectionString);
        EquipmentModel? result = await reader.Filter(new EquipmentCriteria { EquipmentId = "equipment-1" });

        Assert.NotNull(result);
        Assert.Equal("customer-1", result.CustomerId);
        Assert.Equal("abc", result.Attributes["vin"]);
    }

    [Fact]
    public async Task DraftPendingRegistrationStoresWithoutEquipmentType()
    {
        EquipmentAccess access = new();
        PendingRegistration registration = new()
        {
            PendingRegistrationId = "registration-1",
            CustomerId = "customer-1",
            Status = PendingRegistrationStatus.Draft
        };

        await access.Store(registration);

        PendingRegistration? result = await access.Filter(new PendingRegistrationCriteria { PendingRegistrationId = "registration-1" });
        Assert.NotNull(result);
        Assert.Equal("customer-1", result.CustomerId);
        Assert.Null(result.EquipmentTypeId);
        Assert.Equal(PendingRegistrationStatus.Draft, result.Status);
    }

    [Fact]
    public async Task SubmittedPendingRegistrationRejectsMissingEquipmentType()
    {
        EquipmentAccess access = new();
        PendingRegistration registration = new()
        {
            PendingRegistrationId = "registration-1",
            CustomerId = "customer-1",
            Status = PendingRegistrationStatus.Submitted
        };

        await Assert.ThrowsAsync<ArgumentException>(() => access.Store(registration));
        PendingRegistration? result = await access.Filter(new PendingRegistrationCriteria { PendingRegistrationId = "registration-1" });
        Assert.Null(result);
    }

    private static string CreateConnectionString()
    {
        string path = Path.Combine(Path.GetTempPath(), "fixme-equipment-tests", $"{Guid.NewGuid():N}.db");
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        return $"Data Source={path}";
    }
}
