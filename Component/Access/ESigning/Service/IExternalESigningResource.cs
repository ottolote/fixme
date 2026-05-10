using FixMe.Access.ESigning.Interface;

namespace FixMe.Access.ESigning.Service
{
    public interface IExternalESigningResource
    {
        Task<SignatureRequest> SignatureRequestStore(SignatureRequest request);
    }
}
