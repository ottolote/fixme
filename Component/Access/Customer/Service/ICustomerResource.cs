using FixMe.Access.Customer.Interface;
using CustomerModel = FixMe.Access.Customer.Interface.Customer;

namespace FixMe.Access.Customer.Service;

internal interface ICustomerResource
{
    Task<CustomerModel?> Find(CustomerLookup lookup);

    Task<CustomerModel> Store(CustomerModel customer);
}
