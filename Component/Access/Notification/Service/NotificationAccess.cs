using FixMe.Access.Notification.Interface;
using NotificationModel = FixMe.Access.Notification.Interface.Notification;

namespace FixMe.Access.Notification.Service
{
    public class NotificationAccess : INotificationAccess
    {
        private readonly NotificationResource _resource;

        public NotificationAccess()
            : this(NotificationResource.Instance)
        {
        }

        public NotificationAccess(NotificationResource resource)
        {
            _resource = resource;
        }

        public Task<NotificationTemplate?> NotificationTemplateFilter(NotificationTemplateCriteria request)
        {
            ArgumentNullException.ThrowIfNull(request);

            return Task.FromResult(_resource.FilterTemplate(request));
        }

        public Task<NotificationModel> Store(NotificationModel request)
        {
            ArgumentNullException.ThrowIfNull(request);
            Validate(request);

            return Task.FromResult(_resource.Store(request));
        }

        private static void Validate(NotificationModel notification)
        {
            if (string.IsNullOrWhiteSpace(notification.Recipient))
            {
                throw new ArgumentException("Notification recipient is required.", nameof(notification));
            }

            if (string.IsNullOrWhiteSpace(notification.TemplateId))
            {
                throw new ArgumentException("Notification template is required.", nameof(notification));
            }

            if (string.IsNullOrWhiteSpace(notification.Channel))
            {
                throw new ArgumentException("Notification channel is required.", nameof(notification));
            }

            if (string.IsNullOrWhiteSpace(notification.RenderedPayload))
            {
                throw new ArgumentException("Notification rendered payload is required.", nameof(notification));
            }

            if (string.IsNullOrWhiteSpace(notification.EventReference))
            {
                throw new ArgumentException("Notification event reference is required.", nameof(notification));
            }

            if (string.IsNullOrWhiteSpace(notification.DeliveryStatus))
            {
                throw new ArgumentException("Notification delivery status is required.", nameof(notification));
            }
        }
    }
}
