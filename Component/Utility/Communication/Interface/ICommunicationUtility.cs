using System.Threading.Tasks;

namespace FixMe.Utility.Communication.Interface
{
    public interface ICommunicationUtility
    {
        Task<NotifyUserResponse> NotifyUser(NotifyUserRequest request);
    }
}