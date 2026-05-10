# SI3.1 Notify user

## Flow

Sources: `BP1.1-onboard.bpmn`, `BP1.2-register-equipment.bpmn`, `BP1.3-enroll-in-maintenance-plan.bpmn`, and `BP1.4-schedule-maintenance-job.bpmn`.

The BPMNs use this as a generic user notification interaction. In onboarding, it sends the activation link after `SI1.1 Register account`. In equipment registration, it notifies the customer after equipment registration is accepted, either directly by `SI1.6 Submit registration` or through `SI1.7 Resolve pending registration`. In maintenance-plan enrollment, it notifies the customer after a rejected plan request. In maintenance-job scheduling, it notifies the customer after a maintenance slots proposal is confirmed.

The system consumes a notification-triggering business event, validates the notification request data in separate checks, resolves the recipient and channel, renders the message content using the supplied template name or template ID, and sends it. For maintenance slots proposal notifications, the event payload must identify a confirmed proposal with active reserved slots so the template can render selectable options. Successful processing emits a generic notification-sent event with a notification type in the event payload. Invalid or synchronously failed event processing is nacked to the DLQ. Email is the primary modeled channel; other channels can be added as delivery details without splitting this into separate system interactions.

The PlantUML diagram combines validation branches into a single gate; the table keeps the specific failure outcomes.

## Steps

| Step | Step detail | Actions performed |
|---|---|---|
| 1 | Consume event: notification-triggering business event | Consumes a business event that requires user notification. |
| 2 | Check whether recipient can be resolved | Validates that the event identifies a notifiable user. |
| 2a | Nack event to DLQ: recipient-resolution failure | Nacks the consumed event when no recipient can be resolved. |
| 3 | Check whether notification template exists | Validates the supplied template name or template ID. |
| 3a | Nack event to DLQ: template-not-found failure | Nacks the consumed event when the notification template cannot be found. |
| 4 | Check whether payload is valid for template | Validates that the event payload can render the template. |
| 4a | Nack event to DLQ: invalid-payload failure | Nacks the consumed event when required template data is missing or invalid. |
| 5 | Resolve recipient and channel | Determines who should receive the notification and through which channel. |
| 6 | Check whether channel is supported and reachable | Validates that the recipient can be contacted through the selected channel. |
| 6a | Nack event to DLQ: channel-unavailable failure | Nacks the consumed event when the channel is unsupported or unreachable. |
| 7 | Render notification content from template name or ID | Builds the notification body from the selected template and supplied data. |
| 8 | Send notification | Delivers the rendered notification. |
| 8a | Nack event to DLQ: delivery-failed failure | Nacks the consumed event when notification delivery fails synchronously. |
| 9 | Produce business event: `User notification sent` | Publishes that notification delivery was initiated or completed successfully. |

## Business Events

Consumed:
- `preliminaryUserCreated` to send the activation notification.
- `Equipment registration accepted` to send the accepted-registration notification after direct registration or pending-registration acceptance.
- `Maintenance plan rejected` to send the rejected-plan notification.
- `Maintenance slots proposal confirmed` to send the maintenance-slots-proposal notification.

Produced:
- `User notification sent`, with a notification type such as `activation`, `accepted_equipment_registration`, `rejected_maintenance_plan`, or `maintenance_slots_proposal`.

## Questions / Answers

| Question | Answer |
|---|---|
| Should failed notification delivery fail the registration flow or be retried asynchronously? | Answered. Do not fail the originating registration or business flow; notifications are event-driven. Retry delivery asynchronously according to queue policy, then send unrecoverable failures to the DLQ. |
| Are all notifications email-only for now, or should the request already support a channel field for future SMS or in-app messages? | Answered. Email is the only supported channel for now. Keep the interaction's channel resolution step so SMS or in-app delivery can be added later without splitting the interaction. |
| Should maintenance slots proposal notification stay as a dedicated interaction? | Answered. No. It is handled by this generic notification interaction with a `Maintenance slots proposal confirmed` event and a maintenance-slots-proposal template. |
