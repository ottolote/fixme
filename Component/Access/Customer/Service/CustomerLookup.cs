using FixMe.Access.Customer.Interface;

namespace FixMe.Access.Customer.Service;

internal sealed class CustomerLookup
{
    private CustomerLookup(CustomerLookupField field, string value)
    {
        Field = field;
        Value = value;
    }

    public CustomerLookupField Field { get; }

    public string Value { get; }

    public static CustomerLookup From(CustomerCriteria criteria)
    {
        ArgumentNullException.ThrowIfNull(criteria);

        List<CustomerLookup> lookups = [];
        AddLookup(lookups, CustomerLookupField.CustomerId, criteria.CustomerId);
        AddLookup(lookups, CustomerLookupField.Email, criteria.Email);
        AddLookup(lookups, CustomerLookupField.ConfirmationToken, criteria.ConfirmationToken);
        AddLookup(lookups, CustomerLookupField.ProfileReference, criteria.ProfileReference);

        return lookups.Count switch
        {
            1 => lookups[0],
            0 => throw new ArgumentException("Customer criteria must include exactly one lookup field.", nameof(criteria)),
            _ => throw new ArgumentException("Customer criteria lookup fields are contradictory.", nameof(criteria)),
        };
    }

    private static void AddLookup(List<CustomerLookup> lookups, CustomerLookupField field, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            lookups.Add(new CustomerLookup(field, value));
        }
    }
}

internal enum CustomerLookupField
{
    CustomerId,
    Email,
    ConfirmationToken,
    ProfileReference,
}
