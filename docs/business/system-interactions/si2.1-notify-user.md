# SI2.1 Notify user

## Flow

Sources: `BP1.1-onboard.bpmn`, `BP1.2-register-equipment.bpmn`, and `BP1.3-enroll-in-maintenance-plan.bpmn`.

The BPMNs use this as a generic user notification interaction. In onboarding, it sends the activation link after `SI1.1 Register account`. In equipment registration, it notifies the customer after an accepted equipment registration. In maintenance-plan enrollment, it notifies the customer after a rejected plan request. Maintenance-slot proposal notification is modeled separately as `SI2.3 Notify customer of maintenance slots proposal` in `BP1.4-schedule-maintenance-job.bpmn`.

The system consumes a notification-triggering business event, validates the notification request data in separate checks, resolves the recipient and channel, renders the message content using the supplied template name or template ID, sends it, and returns success or failure. Email is the primary modeled channel; other channels can be added as delivery details without splitting this into separate system interactions.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume notification-triggering business event | Receives a business event that requires user notification. |
| 2 | Check whether recipient can be resolved | Validates that the event identifies a notifiable user. |
| 2a | Return recipient-resolution failure | Returns an error when no recipient can be resolved. |
| 3 | Check whether notification template exists | Validates the supplied template name or template ID. |
| 3a | Return template-not-found failure | Returns an error when the notification template cannot be found. |
| 4 | Check whether payload is valid for template | Validates that the event payload can render the template. |
| 4a | Return invalid-payload failure | Returns an error when required template data is missing or invalid. |
| 5 | Resolve recipient and channel | Determines who should receive the notification and through which channel. |
| 6 | Check whether channel is supported and reachable | Validates that the recipient can be contacted through the selected channel. |
| 6a | Return channel-unavailable failure | Returns an error when the channel is unsupported or unreachable. |
| 7 | Render notification content from template name or ID | Builds the notification body from the selected template and supplied data. |
| 8 | Send notification | Delivers the rendered notification. |
| 8a | Return delivery-failed failure | Returns an error when notification delivery fails synchronously. |
| 9 | Produce business event: `User notification sent` | Publishes that notification delivery was initiated or completed successfully. |
| 10 | Return success | Returns a successful notification response. |

## Business Events

Consumed:
- `preliminaryUserCreated` to send the activation notification.
- `Equipment registration accepted` to send the accepted-registration notification.
- `Maintenance plan rejected` to send the rejected-plan notification.

Produced:
- `Activation notification sent`.
- `Accepted equipment registration notification sent`.
- `Rejected maintenance plan notification sent`.

## Questions / Answers

| Question | Answer |
|---|---|
| Should failed notification delivery fail the registration flow or be retried asynchronously? | Open. The diagram returns failure for invalid requests but does not model delivery retry behavior. |
| Are all notifications email-only for now, or should the request already support a channel field for future SMS or in-app messages? | Open. The diagram resolves a channel but does not define supported channel types. |
