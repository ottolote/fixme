using FixMe.Access.Notification.Interface;
using FixMe.Access.Notification.Service;

namespace FixMe.Api.Tests;

public sealed class NotificationAccessTests
{
    [Fact]
    public async Task NotificationTemplateFilterReturnsMatchingTemplate()
    {
        NotificationAccess access = new(new NotificationResource(
        [
            new NotificationTemplate
            {
                TemplateId = "activation-email",
                EventType = "preliminaryUserCreated",
                NotificationType = "activation",
                Channel = "email",
                Locale = "en-US",
                Subject = "Activate your account",
                Body = "Activate now"
            }
        ]));

        NotificationTemplate? template = await access.NotificationTemplateFilter(new NotificationTemplateCriteria
        {
            EventType = "preliminaryUserCreated",
            NotificationType = "activation",
            Channel = "email",
            Locale = "en-US"
        });

        Assert.NotNull(template);
        Assert.Equal("activation-email", template.TemplateId);
    }

    [Fact]
    public async Task NotificationTemplateFilterCanMatchByTemplateIdOnly()
    {
        NotificationAccess access = new(new NotificationResource(
        [
            new NotificationTemplate
            {
                TemplateId = "maintenance-slots-proposal-email",
                EventType = "Maintenance slots proposal confirmed",
                NotificationType = "maintenance_slots_proposal",
                Channel = "email",
                Locale = "en-US"
            }
        ]));

        NotificationTemplate? template = await access.NotificationTemplateFilter(new NotificationTemplateCriteria
        {
            TemplateId = "maintenance-slots-proposal-email"
        });

        Assert.NotNull(template);
        Assert.Equal("maintenance_slots_proposal", template.NotificationType);
    }

    [Fact]
    public async Task NotificationTemplateFilterReturnsNullWhenTemplateIsNotFound()
    {
        NotificationAccess access = new(new NotificationResource(
        [
            new NotificationTemplate
            {
                TemplateId = "activation-email",
                EventType = "preliminaryUserCreated",
                NotificationType = "activation",
                Channel = "email",
                Locale = "en-US"
            }
        ]));

        NotificationTemplate? template = await access.NotificationTemplateFilter(new NotificationTemplateCriteria
        {
            EventType = "Maintenance slots proposal confirmed",
            NotificationType = "maintenance_slots_proposal",
            Channel = "email",
            Locale = "en-US"
        });

        Assert.Null(template);
    }

    [Fact]
    public async Task StorePersistsAndReturnsStoredNotificationState()
    {
        NotificationResource resource = new([]);
        NotificationAccess access = new(resource);

        Notification stored = await access.Store(new Notification
        {
            Recipient = "user@example.test",
            TemplateId = "activation-email",
            Channel = "email",
            RenderedPayload = "Activate now",
            EventReference = "preliminaryUserCreated:123",
            DeliveryStatus = "sent"
        });

        Assert.Equal(1, resource.StoredNotificationCount);
        Assert.Equal("user@example.test", stored.Recipient);
        Assert.False(string.IsNullOrWhiteSpace(stored.NotificationId));
        Assert.NotNull(stored.StoredAt);
    }

    [Theory]
    [InlineData("recipient")]
    [InlineData("template")]
    [InlineData("channel")]
    [InlineData("payload")]
    [InlineData("event")]
    [InlineData("status")]
    public async Task StoreRejectsInvalidNotificationWithoutPersisting(string missingField)
    {
        NotificationResource resource = new([]);
        NotificationAccess access = new(resource);
        Notification notification = ValidNotification();

        switch (missingField)
        {
            case "recipient":
                notification.Recipient = null;
                break;
            case "template":
                notification.TemplateId = null;
                break;
            case "channel":
                notification.Channel = null;
                break;
            case "payload":
                notification.RenderedPayload = null;
                break;
            case "event":
                notification.EventReference = null;
                break;
            case "status":
                notification.DeliveryStatus = null;
                break;
        }

        await Assert.ThrowsAsync<ArgumentException>(() => access.Store(notification));
        Assert.Equal(0, resource.StoredNotificationCount);
    }

    private static Notification ValidNotification()
    {
        return new Notification
        {
            Recipient = "user@example.test",
            TemplateId = "activation-email",
            Channel = "email",
            RenderedPayload = "Activate now",
            EventReference = "preliminaryUserCreated:123",
            DeliveryStatus = "sent"
        };
    }
}
