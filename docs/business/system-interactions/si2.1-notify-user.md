# SI2.1 Notify user

## Flow

Sources: `BP1.1-onboard.bpmn`, `BP1.2-register-equipment.bpmn`, and `BP1.3-enroll-in-maintenance-plan.bpmn`.

The BPMNs use this as a generic user notification interaction. In onboarding, it sends the activation link after `SI1.1 Register account`. In equipment registration, it notifies the customer after an accepted equipment registration. In maintenance-plan enrollment, it notifies the customer after a rejected plan request. Maintenance-slot proposal notification is modeled separately as `SI2.3 Notify customer of maintenance slots proposal` in `BP1.4-schedule-maintenance-job.bpmn`.

The system validates the notification request, resolves the recipient and channel, renders the message content using the supplied template name or template ID, sends it, and returns success or failure. Email is the primary modeled channel; other channels can be added as delivery details without splitting this into separate system interactions.

## Business Events

Consumed:
- `User account registered pending email confirmation` to send the activation notification.
- `Equipment registration accepted` to send the accepted-registration notification.
- `Maintenance plan rejected` to send the rejected-plan notification.

Produced:
- `Activation notification sent`.
- `Accepted equipment registration notification sent`.
- `Rejected maintenance plan notification sent`.

## Questions

- Should failed notification delivery fail the registration flow or be retried asynchronously?
- Are all notifications email-only for now, or should the request already support a channel field for future SMS or in-app messages?
