using FixMe.Access.Customer.Interface;
using CustomerModel = FixMe.Access.Customer.Interface.Customer;

namespace FixMe.Access.Customer.Service;

public sealed class CustomerAccess : ICustomerAccess
{
    private readonly ICustomerResource _customerResource;

    public CustomerAccess()
        : this(CustomerResource.Shared)
    {
    }

    internal CustomerAccess(ICustomerResource customerResource)
    {
        _customerResource = customerResource;
    }

    public Task<CustomerModel?> Filter(CustomerCriteria request)
    {
        CustomerLookup lookup = CustomerLookup.From(request);
        return _customerResource.Find(lookup);
    }

    public Task<CustomerModel> Store(CustomerModel request)
    {
        ValidateCustomer(request);
        return _customerResource.Store(request);
    }

    private static void ValidateCustomer(CustomerModel request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.CustomerId) && string.IsNullOrWhiteSpace(request.Email))
        {
            throw new ArgumentException("Customer must include a customer identifier or email.", nameof(request));
        }
    }
}
