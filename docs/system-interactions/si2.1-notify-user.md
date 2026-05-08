# SI2.1 Notify user

## Flow

Sources: `BP1.1-onboard.bpmn`, `BP1.2-register-equipment.bpmn`, `BP1.3-enroll-in-maintenance-plan.bpmn`, and `BP1.4-schedule-maintenance-job.bpmn`.

The BPMNs use this as a generic user notification interaction. In onboarding, it sends the activation link after `SI1.1 Register account`. In equipment registration, it notifies the customer after either an accepted or rejected equipment registration. In maintenance-plan enrollment, it notifies the customer after a rejected plan request. In maintenance-job scheduling, it notifies the customer that a maintenance slots proposal is available.

The system validates the notification request, resolves the recipient and channel, renders the message content using the supplied template name or template ID, sends it, and returns success or failure. Email is the primary modeled channel; other channels can be added as delivery details without splitting this into separate system interactions.

## Questions

- Should failed notification delivery fail the registration flow or be retried asynchronously?
- Are all notifications email-only for now, or should the request already support a channel field for future SMS or in-app messages?
