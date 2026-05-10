## ADDED Requirements

### Requirement: SI3.1 Notify user
The `NotificationManager` SHALL expose `NotifyUser(NotifyUserRequest): NotifyUserResponse` for supported notification-triggering events. The manager SHALL resolve the recipient, load templates with `NotificationAccess.NotificationTemplateFilter(NotificationTemplateCriteria)`, persist notification records with `NotificationAccess.Store(Notification)`, send rendered notifications through `CommunicationUtility.NotifyUser(NotifyUserRequest)`, and isolate notification failure from the originating business flow through asynchronous retry and dead-letter handling.

#### Scenario: Preliminary account notification is sent
- **WHEN** `NotifyUser(NotifyUserRequest)` processes `preliminaryUserCreated` with a resolvable recipient and valid activation template payload
- **THEN** the manager stores and sends an email notification of type `activation`, returns `NotifyUserResponse` as successful, and records `User notification sent`

#### Scenario: Equipment registration acceptance notification is sent
- **WHEN** `NotifyUser(NotifyUserRequest)` processes `Equipment registration accepted` with a resolvable recipient and valid equipment-registration payload
- **THEN** the manager stores and sends an email notification of type `accepted_equipment_registration`, returns `NotifyUserResponse` as successful, and records `User notification sent`

#### Scenario: Maintenance plan rejection notification is sent
- **WHEN** `NotifyUser(NotifyUserRequest)` processes `Maintenance plan rejected` with a resolvable recipient and valid rejection payload
- **THEN** the manager stores and sends an email notification of type `rejected_maintenance_plan`, returns `NotifyUserResponse` as successful, and records `User notification sent`

#### Scenario: Maintenance slots proposal notification is sent
- **WHEN** `NotifyUser(NotifyUserRequest)` processes `Maintenance slots proposal confirmed` with a resolvable recipient and a confirmed proposal containing active reserved slots
- **THEN** the manager stores and sends an email notification of type `maintenance_slots_proposal`, returns `NotifyUserResponse` as successful, and records `User notification sent`

#### Scenario: Notification cannot be processed
- **WHEN** `NotifyUser(NotifyUserRequest)` contains an unsupported event, unresolved recipient, missing template, invalid template payload, unavailable channel, or synchronous delivery failure
- **THEN** the manager returns or records `recipient-resolution`, `template-not-found`, `invalid-payload`, `channel-unavailable`, or `delivery-failed` for retry or dead-letter handling without recording a successful notification

### Requirement: Notification channel policy
The `NotificationManager` SHALL use email as the initial user notification channel for supported P2 events.

#### Scenario: Email channel is resolved
- **WHEN** a supported `NotifyUserRequest` has a valid recipient email and template payload
- **THEN** the manager renders the template for email delivery through the communication utility

#### Scenario: Non-email channel is requested before support exists
- **WHEN** a `NotifyUserRequest` requires a notification channel other than email
- **THEN** the manager treats the channel as unavailable unless that channel is explicitly supported by the notification configuration
