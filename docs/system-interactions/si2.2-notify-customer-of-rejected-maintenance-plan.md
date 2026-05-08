# SI2.2 Notify customer of rejected maintenance plan

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn`.

When a maintenance plan request is rejected, the system notifies the customer. It receives the rejection event, validates it, loads the customer and rejected plan, renders the rejection message, sends the notification, and returns success.

## Questions

- Does the notification include a backoffice rejection reason?
- Which channels are required: email, in-app notification, or both?
