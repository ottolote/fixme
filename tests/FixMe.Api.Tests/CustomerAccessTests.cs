using FixMe.Access.Customer.Interface;
using FixMe.Access.Customer.Service;

namespace FixMe.Api.Tests;

public sealed class CustomerAccessTests
{
    [Fact]
    public async Task FilterReturnsCustomerMatchingEmailWithoutChangingStoredState()
    {
        CustomerResource resource = new();
        CustomerAccess access = new(resource);
        Customer stored = await access.Store(NewCustomer());

        Customer? match = await access.Filter(new CustomerCriteria { Email = "USER@example.com" });

        Assert.NotNull(match);
        Assert.Equal(stored.CustomerId, match.CustomerId);
        Assert.Equal("user@example.com", match.Email);
        Assert.Null(match.ConfirmationTokenUsedAt);
    }

    [Fact]
    public async Task FilterReturnsCustomerMatchingEachSupportedIdentity()
    {
        CustomerResource resource = new();
        CustomerAccess access = new(resource);
        Customer stored = await access.Store(NewCustomer());

        Customer? byId = await access.Filter(new CustomerCriteria { CustomerId = stored.CustomerId });
        Customer? byToken = await access.Filter(new CustomerCriteria { ConfirmationToken = stored.ConfirmationToken });
        Customer? byProfile = await access.Filter(new CustomerCriteria { ProfileReference = stored.ProfileReference });

        Assert.Equal(stored.Email, byId?.Email);
        Assert.Equal(stored.Email, byToken?.Email);
        Assert.Equal(stored.Email, byProfile?.Email);
    }

    [Fact]
    public async Task FilterReturnsNullWhenCustomerIsNotFound()
    {
        CustomerAccess access = new(new CustomerResource());

        Customer? match = await access.Filter(new CustomerCriteria { Email = "missing@example.com" });

        Assert.Null(match);
    }

    [Fact]
    public async Task FilterRejectsMissingOrContradictoryCriteriaWithoutQueryingResource()
    {
        RecordingCustomerResource resource = new();
        CustomerAccess access = new(resource);

        await Assert.ThrowsAsync<ArgumentException>(() => access.Filter(new CustomerCriteria()));
        await Assert.ThrowsAsync<ArgumentException>(() => access.Filter(new CustomerCriteria
        {
            Email = "user@example.com",
            CustomerId = "customer-1",
        }));

        Assert.Equal(0, resource.FindCalls);
    }

    [Fact]
    public async Task StorePersistsNewCustomerAndReturnsStoredState()
    {
        CustomerAccess access = new(new CustomerResource());
        Customer customer = NewCustomer();

        Customer stored = await access.Store(customer);
        Customer? match = await access.Filter(new CustomerCriteria { CustomerId = customer.CustomerId });

        Assert.NotSame(customer, stored);
        Assert.Equal(customer.Email, stored.Email);
        Assert.Equal(customer.Email, match?.Email);
    }

    [Fact]
    public async Task StorePersistsCustomerAcrossSqliteResourceInstances()
    {
        string databasePath = Path.Combine(Path.GetTempPath(), $"fixme-customer-test-{Guid.NewGuid():N}.db");

        try
        {
            CustomerAccess firstAccess = new(new CustomerResource(databasePath));
            Customer stored = await firstAccess.Store(NewCustomer());

            CustomerAccess secondAccess = new(new CustomerResource(databasePath));
            Customer? byId = await secondAccess.Filter(new CustomerCriteria { CustomerId = stored.CustomerId });
            Customer? byEmail = await secondAccess.Filter(new CustomerCriteria { Email = "USER@example.com" });
            Customer? byToken = await secondAccess.Filter(new CustomerCriteria { ConfirmationToken = stored.ConfirmationToken });
            Customer? byProfile = await secondAccess.Filter(new CustomerCriteria { ProfileReference = stored.ProfileReference });

            Assert.Equal(stored.Email, byId?.Email);
            Assert.Equal(stored.CustomerId, byEmail?.CustomerId);
            Assert.Equal(stored.CustomerId, byToken?.CustomerId);
            Assert.Equal(stored.CustomerId, byProfile?.CustomerId);
        }
        finally
        {
            if (File.Exists(databasePath))
            {
                File.Delete(databasePath);
            }
        }
    }

    [Fact]
    public async Task StoreUpdatesExistingCustomerWithoutChangingUnrelatedFields()
    {
        CustomerAccess access = new(new CustomerResource());
        Customer original = await access.Store(NewCustomer());
        original.IsEmailConfirmed = true;
        original.ConfirmationTokenUsedAt = DateTimeOffset.UtcNow;
        original.PasswordHash = "hash-v2";

        Customer stored = await access.Store(original);
        Customer? match = await access.Filter(new CustomerCriteria { Email = original.Email });

        Assert.True(stored.IsEmailConfirmed);
        Assert.Equal("hash-v2", stored.PasswordHash);
        Assert.Equal(original.Name, match?.Name);
        Assert.Equal(original.PhoneNumber, match?.PhoneNumber);
        Assert.Equal(original.Location, match?.Location);
    }

    [Fact]
    public async Task StoreRejectsMissingRequiredIdentityFields()
    {
        CustomerAccess access = new(new CustomerResource());

        await Assert.ThrowsAsync<ArgumentException>(() => access.Store(new Customer()));
    }

    [Fact]
    public async Task StoreRejectsConflictingUniqueCustomerIdentity()
    {
        CustomerAccess access = new(new CustomerResource());
        await access.Store(NewCustomer(customerId: "customer-1", email: "one@example.com", token: "token-1", profile: "profile-1"));
        Customer conflicting = NewCustomer(customerId: "customer-2", email: "two@example.com", token: "token-1", profile: "profile-2");

        await Assert.ThrowsAsync<InvalidOperationException>(() => access.Store(conflicting));

        Customer? existing = await access.Filter(new CustomerCriteria { CustomerId = "customer-1" });
        Assert.Equal("one@example.com", existing?.Email);
    }

    [Fact]
    public async Task StoreRejectsSameEmailWithDifferentCustomerId()
    {
        CustomerAccess access = new(new CustomerResource());
        await access.Store(NewCustomer(customerId: "customer-1", email: "one@example.com"));
        Customer conflicting = NewCustomer(customerId: "customer-2", email: "ONE@example.com", token: "token-2", profile: "profile-2");

        await Assert.ThrowsAsync<InvalidOperationException>(() => access.Store(conflicting));

        Customer? existing = await access.Filter(new CustomerCriteria { Email = "one@example.com" });
        Assert.Equal("customer-1", existing?.CustomerId);
    }

    [Fact]
    public async Task StoreRejectsSameCustomerIdWithDifferentEmail()
    {
        CustomerAccess access = new(new CustomerResource());
        await access.Store(NewCustomer(customerId: "customer-1", email: "one@example.com"));
        Customer conflicting = NewCustomer(customerId: "customer-1", email: "two@example.com", token: "token-2", profile: "profile-2");

        await Assert.ThrowsAsync<InvalidOperationException>(() => access.Store(conflicting));

        Customer? existing = await access.Filter(new CustomerCriteria { CustomerId = "customer-1" });
        Assert.Equal("one@example.com", existing?.Email);
    }

    [Fact]
    public async Task ConfirmedCustomerMayRegisterEquipment()
    {
        CustomerAccess access = new(new CustomerResource());
        Customer customer = NewCustomer();
        customer.IsEmailConfirmed = true;
        await access.Store(customer);

        Customer? match = await access.Filter(new CustomerCriteria { CustomerId = customer.CustomerId });

        Assert.True(match?.MayRegisterEquipment);
    }

    [Fact]
    public async Task MissingOrUnconfirmedCustomerMayNotRegisterEquipment()
    {
        CustomerAccess access = new(new CustomerResource());
        Customer customer = await access.Store(NewCustomer());

        Customer? unconfirmed = await access.Filter(new CustomerCriteria { CustomerId = customer.CustomerId });
        Customer? missing = await access.Filter(new CustomerCriteria { CustomerId = "missing-customer" });

        Assert.False(unconfirmed?.MayRegisterEquipment);
        Assert.Null(missing);
    }

    private static Customer NewCustomer(
        string customerId = "customer-1",
        string email = "user@example.com",
        string token = "confirm-token",
        string profile = "profile-1")
    {
        return new Customer
        {
            CustomerId = customerId,
            Email = email,
            ConfirmationToken = token,
            ConfirmationTokenExpiresAt = DateTimeOffset.UtcNow.AddHours(24),
            ProfileReference = profile,
            Name = "Ada Lovelace",
            PhoneNumber = "+4512345678",
            Location = "Copenhagen",
        };
    }

    private sealed class RecordingCustomerResource : ICustomerResource
    {
        public int FindCalls { get; private set; }

        public Task<Customer?> Find(CustomerLookup lookup)
        {
            FindCalls++;
            return Task.FromResult<Customer?>(null);
        }

        public Task<Customer> Store(Customer customer)
        {
            return Task.FromResult(customer);
        }
    }
}
