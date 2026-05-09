# SI2.1 Notify user

## Flow

Sources: `BP1.1-onboard.bpmn`, `BP1.2-register-equipment.bpmn`, and `BP1.3-enroll-in-maintenance-plan.bpmn`.

The BPMNs use this as a generic user notification interaction. In onboarding, it sends the activation link after `SI1.1 Register account`. In equipment registration, it notifies the customer after an accepted equipment registration. In maintenance-plan enrollment, it notifies the customer after a rejected plan request. Maintenance-slot proposal notification is modeled separately as `SI2.3 Notify customer of maintenance slots proposal` in `BP1.4-schedule-maintenance-job.bpmn`.

The system validates the notification request, resolves the recipient and channel, renders the message content using the supplied template name or template ID, sends it, and returns success or failure. Email is the primary modeled channel; other channels can be added as delivery details without splitting this into separate system interactions.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `User notification requested` | Receives a request to notify a user. |
| 2 | Decide whether the notification request is valid | Validates recipient, template, payload, and delivery requirements. |
| 3 | Resolve recipient and channel | Determines who should receive the notification and through which channel. |
| 4 | Render notification content from template name or ID | Builds the notification body from the selected template and supplied data. |
| 5 | Send notification | Delivers the rendered notification. |
| 6 | Produce business event: `User notification sent` | Publishes that notification delivery was initiated or completed successfully. |
| 7 | Return success | Returns a successful notification response. |
| 8 | Return failure | Returns an error when the notification request is invalid. |

## Business Events

Consumed:
- `User account registered pending email confirmation` to send the activation notification.
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
