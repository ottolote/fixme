# SI1.7 Resolve pending registration

## Flow

Source: `BP1.2-register-equipment.bpmn`.

This interaction is used after a backoffice worker decides whether to accept or reject a pending equipment registration. The system receives the resolution command, validates that the referenced registration is still pending, loads the pending registration, and applies the decision.

For acceptance, the system creates or activates the registered equipment and marks the pending registration as accepted. The BPMN then calls `SI2.1 Notify user` and returns the accepted registration to the customer. For rejection, the system stores the rejection reason when provided and marks the pending registration as rejected. The modeled rejection path ends after resolution and does not notify the customer.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `Pending equipment registration accepted or rejected` | Receives the backoffice resolution decision. |
| 2 | Decide whether the resolution request is valid | Validates that the pending registration can still be resolved and that the decision is allowed. |
| 3 | Load pending registration | Loads the pending registration to resolve. |
| 4 | Decide whether the resolution is acceptance | Routes the interaction to the acceptance or rejection path. |
| 5 | Create or activate registered equipment | Creates a new equipment record or activates the equipment associated with the registration. |
| 6 | Mark registration as accepted | Updates the pending registration to accepted. |
| 7 | Produce business event: `Equipment registration accepted` | Publishes the accepted registration outcome. |
| 8 | Store rejection reason if provided | Records the backoffice rejection reason when supplied. |
| 9 | Mark registration as rejected | Updates the pending registration to rejected. |
| 10 | Produce business event: `Equipment registration rejected` | Publishes the rejected registration outcome. |
| 11 | Return success | Returns a successful resolution response. |
| 12 | Return failure | Returns an error when the resolution request is invalid. |

## Business Events

Consumed:
- `Pending equipment registration accepted` from the backoffice worker accepting the pending registration.
- `Pending equipment registration rejected` from the backoffice worker rejecting the pending registration.

Produced:
- `Equipment registration accepted` when equipment is created or activated.
- `Equipment registration rejected` when the pending registration is rejected.

## Questions / Answers

| Question | Answer |
|---|---|
| When accepting, should the system create a new equipment record every time, or can it link the registration to an existing equipment record? | Open. The diagram intentionally says create or activate registered equipment. |
| What should happen if the backoffice worker resolves a registration that was already accepted or rejected? | Open. The diagram validates the resolution request but does not define idempotency or conflict handling. |
