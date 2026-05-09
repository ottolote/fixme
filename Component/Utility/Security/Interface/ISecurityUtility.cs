using System.Threading.Tasks;

namespace FixMe.Utility.Security.Interface
{
    public interface ISecurityUtility
    {
        Task<HashResponse> HashPassword(HashPasswordRequest request);
    }
}