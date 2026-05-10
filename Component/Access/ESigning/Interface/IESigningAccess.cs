using System.Threading.Tasks;

namespace FixMe.Access.ESigning.Interface
{
    public interface IESigningAccess
    {
        Task<SignatureRequest> SignatureRequestStore(SignatureRequest request);
    }
}