# SI1.7 Resolve pending registration

## Flow

Source: `BP1.2-register-equipment.bpmn`.

This interaction is used after a backoffice worker decides whether to accept or reject a pending equipment registration. The system receives the resolution command, validates that the referenced registration is still pending, loads the pending registration, and applies the decision.

For acceptance, the system creates or activates the registered equipment and marks the pending registration as accepted. For rejection, the system stores the rejection reason when provided and marks the pending registration as rejected. Both acceptance and rejection should notify the customer through `SI2.1 Notify user`; the current BPMN only shows the notification path for acceptance and should be corrected or interpreted as incomplete.

## Questions

- When accepting, should the system create a new equipment record every time, or can it link the registration to an existing equipment record?
- What should happen if the backoffice worker resolves a registration that was already accepted or rejected?
