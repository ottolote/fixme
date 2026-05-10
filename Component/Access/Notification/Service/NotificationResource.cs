using FixMe.Access.Notification.Interface;
using NotificationModel = FixMe.Access.Notification.Interface.Notification;

namespace FixMe.Access.Notification.Service
{
    public sealed class NotificationResource
    {
        private readonly List<NotificationModel> _notifications = [];
        private readonly List<NotificationTemplate> _templates;
        private readonly object _syncRoot = new();

        public NotificationResource()
            : this(DefaultTemplates())
        {
        }

        public NotificationResource(IEnumerable<NotificationTemplate> templates)
        {
            _templates = templates.Select(Clone).ToList();
        }

        public static NotificationResource Instance { get; } = new();

        public int StoredNotificationCount
        {
            get
            {
                lock (_syncRoot)
                {
                    return _notifications.Count;
                }
            }
        }

        public NotificationTemplate? FilterTemplate(NotificationTemplateCriteria criteria)
        {
            NotificationTemplate? match = _templates.FirstOrDefault(template => Matches(template, criteria));
            return match is null ? null : Clone(match);
        }

        public NotificationModel Store(NotificationModel notification)
        {
            NotificationModel stored = Clone(notification);
            stored.NotificationId = string.IsNullOrWhiteSpace(stored.NotificationId) ? Guid.NewGuid().ToString("N") : stored.NotificationId;
            stored.StoredAt ??= DateTimeOffset.UtcNow;

            lock (_syncRoot)
            {
                _notifications.Add(Clone(stored));
            }

            return stored;
        }

        private static bool Matches(NotificationTemplate template, NotificationTemplateCriteria criteria)
        {
            return Matches(criteria.TemplateId, template.TemplateId)
                && Matches(criteria.EventType, template.EventType)
                && Matches(criteria.NotificationType, template.NotificationType)
                && Matches(criteria.Channel, template.Channel)
                && Matches(criteria.Locale, template.Locale);
        }

        private static bool Matches(string? criteriaValue, string? templateValue)
        {
            return string.IsNullOrWhiteSpace(criteriaValue)
                || string.Equals(criteriaValue, templateValue, StringComparison.OrdinalIgnoreCase);
        }

        private static NotificationTemplate Clone(NotificationTemplate template)
        {
            return new NotificationTemplate
            {
                TemplateId = template.TemplateId,
                EventType = template.EventType,
                NotificationType = template.NotificationType,
                Channel = template.Channel,
                Locale = template.Locale,
                Subject = template.Subject,
                Body = template.Body
            };
        }

        private static NotificationModel Clone(NotificationModel notification)
        {
            return new NotificationModel
            {
                NotificationId = notification.NotificationId,
                Recipient = notification.Recipient,
                TemplateId = notification.TemplateId,
                Channel = notification.Channel,
                RenderedPayload = notification.RenderedPayload,
                EventReference = notification.EventReference,
                DeliveryStatus = notification.DeliveryStatus,
                StoredAt = notification.StoredAt
            };
        }

        private static NotificationTemplate[] DefaultTemplates()
        {
            return
            [
                new NotificationTemplate
                {
                    TemplateId = "activation-email",
                    EventType = "preliminaryUserCreated",
                    NotificationType = "activation",
                    Channel = "email",
                    Locale = "en-US",
                    Subject = "Activate your FixMe account",
                    Body = "Use the activation link to complete your account setup."
                },
                new NotificationTemplate
                {
                    TemplateId = "accepted-equipment-registration-email",
                    EventType = "Equipment registration accepted",
                    NotificationType = "accepted_equipment_registration",
                    Channel = "email",
                    Locale = "en-US",
                    Subject = "Equipment registration accepted",
                    Body = "Your equipment registration has been accepted."
                },
                new NotificationTemplate
                {
                    TemplateId = "rejected-maintenance-plan-email",
                    EventType = "Maintenance plan rejected",
                    NotificationType = "rejected_maintenance_plan",
                    Channel = "email",
                    Locale = "en-US",
                    Subject = "Maintenance plan request update",
                    Body = "Your maintenance plan request was not accepted."
                },
                new NotificationTemplate
                {
                    TemplateId = "maintenance-slots-proposal-email",
                    EventType = "Maintenance slots proposal confirmed",
                    NotificationType = "maintenance_slots_proposal",
                    Channel = "email",
                    Locale = "en-US",
                    Subject = "Maintenance slots are available",
                    Body = "Choose from the confirmed maintenance slot options."
                }
            ];
        }
    }
}
