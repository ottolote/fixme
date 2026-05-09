# SI1.7 Resolve pending registration

## Flow

Source: `BP1.2-register-equipment.bpmn`.

This interaction is used after a backoffice worker decides whether to accept or reject a pending equipment registration. The system receives the resolution request from the backoffice client, validates the decision, loads the referenced registration, checks that it is still pending, and applies the decision.

For acceptance, the system creates a registered equipment record and marks the pending registration as accepted. The BPMN then calls `SI3.1 Notify user` and returns the accepted registration to the customer. For rejection, the system stores the rejection reason when provided and marks the pending registration as rejected. The modeled rejection path ends after resolution and does not notify the customer.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive pending registration resolution request | Receives the accept or reject decision from the backoffice client. |
| 2 | Check whether decision is valid | Validates that the requested decision is acceptance or rejection. |
| 2a | Return invalid-decision failure | Returns an error when the decision is unsupported. |
| 3 | Load pending registration | Loads the registration to resolve. |
| 3a | Return registration-not-found failure | Returns an error when the referenced registration does not exist. |
| 4 | Check whether registration is still pending | Validates that the registration has not already been accepted or rejected. |
| 4a | Return already-resolved failure | Returns an error when the registration is no longer pending. |
| 5 | Decide whether the resolution is acceptance | Routes the interaction to the acceptance or rejection path. |
| 6 | Create registered equipment | Creates a new equipment record from the registration. |
| 7 | Mark registration as accepted | Updates the pending registration to accepted. |
| 8 | Produce business event: `Equipment registration accepted` | Publishes the accepted registration outcome. |
| 9 | Store rejection reason if provided | Records the backoffice rejection reason when supplied. |
| 10 | Mark registration as rejected | Updates the pending registration to rejected. |
| 11 | Produce business event: `Equipment registration rejected` | Publishes the rejected registration outcome. |
| 12 | Return success | Returns a successful resolution response. |

## Business Events

Consumed:
- Pending registration resolution request from the backoffice client when a worker accepts the pending registration.
- Pending registration resolution request from the backoffice client when a worker rejects the pending registration.

Produced:
- `Equipment registration accepted` when equipment is created or activated.
- `Equipment registration rejected` when the pending registration is rejected.

## Questions / Answers

| Question | Answer |
|---|---|
| When accepting, should the system create a new equipment record every time, or can it link the registration to an existing equipment record? | Answered. Create a new equipment record from the registration. Duplicate detection and linking to existing equipment are intentionally out of scope for now. |
| What should happen if the backoffice worker resolves a registration that was already accepted or rejected? | Answered. Return `already-resolved` as a conflict failure, because the steps already require checking that the registration is still pending before applying a decision. |
