using FixMe.Access.Customer.Interface;
using CustomerModel = FixMe.Access.Customer.Interface.Customer;

namespace FixMe.Access.Customer.Service;

internal sealed class CustomerResource : ICustomerResource
{
    internal static readonly CustomerResource Shared = new();

    private readonly object _sync = new();
    private readonly List<CustomerModel> _customers = [];

    public Task<CustomerModel?> Find(CustomerLookup lookup)
    {
        ArgumentNullException.ThrowIfNull(lookup);

        lock (_sync)
        {
            CustomerModel? match = _customers.SingleOrDefault(customer => Matches(customer, lookup));
            return Task.FromResult(match is null ? null : Clone(match));
        }
    }

    public Task<CustomerModel> Store(CustomerModel customer)
    {
        ArgumentNullException.ThrowIfNull(customer);

        lock (_sync)
        {
            int existingIndex = FindExistingCustomerIndex(customer);
            if (existingIndex >= 0)
            {
                CustomerModel existing = _customers[existingIndex];
                if (HasUniqueIdentityConflict(existing, customer))
                {
                    throw new InvalidOperationException("Customer conflicts with an existing unique identity.");
                }

                _customers[existingIndex] = Clone(customer);
            }
            else
            {
                if (HasUniqueIdentityConflict(null, customer))
                {
                    throw new InvalidOperationException("Customer conflicts with an existing unique identity.");
                }

                _customers.Add(Clone(customer));
            }

            return Task.FromResult(Clone(customer));
        }
    }

    private static bool Matches(CustomerModel customer, CustomerLookup lookup)
    {
        return lookup.Field switch
        {
            CustomerLookupField.CustomerId => EqualsOrdinal(customer.CustomerId, lookup.Value),
            CustomerLookupField.Email => EqualsOrdinalIgnoreCase(customer.Email, lookup.Value),
            CustomerLookupField.ConfirmationToken => EqualsOrdinal(customer.ConfirmationToken, lookup.Value),
            CustomerLookupField.ProfileReference => EqualsOrdinal(customer.ProfileReference, lookup.Value),
            _ => false,
        };
    }

    private int FindExistingCustomerIndex(CustomerModel customer)
    {
        if (!string.IsNullOrWhiteSpace(customer.CustomerId))
        {
            int idIndex = _customers.FindIndex(existing => EqualsOrdinal(existing.CustomerId, customer.CustomerId));
            if (idIndex >= 0)
            {
                return idIndex;
            }
        }

        if (!string.IsNullOrWhiteSpace(customer.Email))
        {
            return _customers.FindIndex(existing => EqualsOrdinalIgnoreCase(existing.Email, customer.Email));
        }

        return -1;
    }

    private bool HasUniqueIdentityConflict(CustomerModel? replacementTarget, CustomerModel customer)
    {
        return _customers.Any(existing =>
            !ReferenceEquals(existing, replacementTarget)
            && ((!string.IsNullOrWhiteSpace(customer.CustomerId) && EqualsOrdinal(existing.CustomerId, customer.CustomerId))
                || (!string.IsNullOrWhiteSpace(customer.Email) && EqualsOrdinalIgnoreCase(existing.Email, customer.Email))
                || (!string.IsNullOrWhiteSpace(customer.ConfirmationToken) && EqualsOrdinal(existing.ConfirmationToken, customer.ConfirmationToken))
                || (!string.IsNullOrWhiteSpace(customer.ProfileReference) && EqualsOrdinal(existing.ProfileReference, customer.ProfileReference))));
    }

    private static CustomerModel Clone(CustomerModel customer)
    {
        return new CustomerModel
        {
            CustomerId = customer.CustomerId,
            Email = customer.Email,
            IsEmailConfirmed = customer.IsEmailConfirmed,
            ConfirmationToken = customer.ConfirmationToken,
            ConfirmationTokenExpiresAt = customer.ConfirmationTokenExpiresAt,
            ConfirmationTokenUsedAt = customer.ConfirmationTokenUsedAt,
            ProfileReference = customer.ProfileReference,
            PasswordHash = customer.PasswordHash,
            Name = customer.Name,
            PhoneNumber = customer.PhoneNumber,
            Location = customer.Location,
        };
    }

    private static bool EqualsOrdinal(string? left, string? right)
    {
        return string.Equals(left, right, StringComparison.Ordinal);
    }

    private static bool EqualsOrdinalIgnoreCase(string? left, string? right)
    {
        return string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
    }
}
