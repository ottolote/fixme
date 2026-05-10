namespace FixMe.Access.Notification.Interface
{
    public class Notification
    {
        public string? NotificationId { get; set; }

        public string? Recipient { get; set; }

        public string? TemplateId { get; set; }

        public string? Channel { get; set; }

        public string? RenderedPayload { get; set; }

        public string? EventReference { get; set; }

        public string? DeliveryStatus { get; set; }

        public DateTimeOffset? StoredAt { get; set; }
    }
}
