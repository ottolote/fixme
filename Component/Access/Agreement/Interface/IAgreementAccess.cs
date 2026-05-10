using System.Threading.Tasks;

namespace FixMe.Access.Agreement.Interface
{
    public interface IAgreementAccess
    {
        Task<Agreement?> Filter(AgreementCriteria request);
        Task<Agreement> Store(Agreement request);
    }
}
