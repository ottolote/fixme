using System.Threading.Tasks;

namespace FixMe.Manager.Notification.Interface
{
    public interface INotificationManager
    {
        Task<NotifyUserResponse> NotifyUser(NotifyUserRequest request);
    }
}
