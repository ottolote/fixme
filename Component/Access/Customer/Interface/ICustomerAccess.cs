using System.Threading.Tasks;

namespace FixMe.Access.Customer.Interface
{
    public interface ICustomerAccess
    {
        Task<Customer> Filter(CustomerCriteria request);
        Task<Customer> Store(Customer request);
    }
}