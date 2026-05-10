namespace FixMe.Access.Notification.Interface
{
    public class NotificationTemplate
    {
        public string? TemplateId { get; set; }

        public string? EventType { get; set; }

        public string? NotificationType { get; set; }

        public string? Channel { get; set; }

        public string? Locale { get; set; }

        public string? Subject { get; set; }

        public string? Body { get; set; }
    }
}
