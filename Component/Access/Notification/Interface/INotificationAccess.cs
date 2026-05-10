using System.Threading.Tasks;

namespace FixMe.Access.Notification.Interface
{
    public interface INotificationAccess
    {
        Task<NotificationTemplate?> NotificationTemplateFilter(NotificationTemplateCriteria request);
        Task<Notification> Store(Notification request);
    }
}
