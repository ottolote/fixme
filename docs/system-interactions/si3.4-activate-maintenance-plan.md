# SI3.4 Activate maintenance plan

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn` and `../references/use-cases/uc2.2-process-new-signature.pu`, `uc2.3-finish-signed-document.pu`.

After the customer signs the maintenance plan agreement, a signed-document event reaches the system. The system validates the signed document and pending plan state, loads the plan, attaches the signed agreement, activates the maintenance plan, and returns the active plan to the customer.

## Questions

- Does activation wait for the eSigning service to finish and store the signed document, or does it happen directly after customer signature?
- Is activation idempotent if the same signing-completed event is delivered more than once?
