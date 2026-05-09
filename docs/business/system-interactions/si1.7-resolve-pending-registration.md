# SI1.7 Resolve pending registration

## Flow

Source: `BP1.2-register-equipment.bpmn`.

This interaction is used after a backoffice worker decides whether to accept or reject a pending equipment registration. The system receives the resolution command, validates that the referenced registration is still pending, loads the pending registration, and applies the decision.

For acceptance, the system creates or activates the registered equipment and marks the pending registration as accepted. The BPMN then calls `SI2.1 Notify user` and returns the accepted registration to the customer. For rejection, the system stores the rejection reason when provided and marks the pending registration as rejected. The modeled rejection path ends after resolution and does not notify the customer.

## Business Events

Consumed:
- `Pending equipment registration accepted` from the backoffice worker accepting the pending registration.
- `Pending equipment registration rejected` from the backoffice worker rejecting the pending registration.

Produced:
- `Equipment registration accepted` when equipment is created or activated.
- `Equipment registration rejected` when the pending registration is rejected.

## Questions

- When accepting, should the system create a new equipment record every time, or can it link the registration to an existing equipment record?
- What should happen if the backoffice worker resolves a registration that was already accepted or rejected?
