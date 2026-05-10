## ADDED Requirements

### Requirement: Notification template filtering
`NotificationAccess` SHALL expose `NotificationTemplateFilter(NotificationTemplateCriteria): NotificationTemplate` for resolving templates used by `NotifyUser(NotifyUserRequest)`. The access component SHALL delegate reads to `NotificationResource` and SHALL return templates matching the supplied `NotificationTemplateCriteria`.

#### Scenario: Matching notification template is returned
- **WHEN** `NotificationTemplateFilter(NotificationTemplateCriteria)` is called with criteria matching a supported event type, notification type, channel, locale, or template identifier
- **THEN** `NotificationAccess` returns the matching `NotificationTemplate` from `NotificationResource`

#### Scenario: Notification template is not found
- **WHEN** `NotificationTemplateFilter(NotificationTemplateCriteria)` is called with criteria matching no template
- **THEN** `NotificationAccess` returns no `NotificationTemplate` result

### Requirement: Notification storage
`NotificationAccess` SHALL expose `Store(Notification): Notification` for recording rendered or attempted user notifications. The access component SHALL delegate writes to `NotificationResource` and SHALL return the persisted `Notification` state.

#### Scenario: Notification is stored
- **WHEN** `Store(Notification)` is called with a valid recipient, template, channel, rendered payload, event reference, and delivery status
- **THEN** `NotificationAccess` persists the `Notification` through `NotificationResource` and returns the stored `Notification`

#### Scenario: Notification cannot be stored
- **WHEN** `Store(Notification)` is called with missing recipient, template, channel, payload, event reference, or delivery status data
- **THEN** `NotificationAccess` rejects the store operation and does not persist a partial `Notification`
