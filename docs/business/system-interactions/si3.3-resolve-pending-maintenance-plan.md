# SI3.3 Resolve pending maintenance plan

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn`.

This interaction is used in two different BPMN contexts. After backoffice review rejection, the system resolves the pending maintenance plan as rejected and then calls `SI2.1 Notify user`. After the customer signs the maintenance plan agreement, the system resolves the pending maintenance plan as active and returns the active maintenance plan to the customer.

The system receives a maintenance plan resolution command or event, validates that the pending maintenance plan can still be resolved, loads the pending maintenance plan, and applies the requested outcome. For rejection, it stores a rejection reason when provided and marks the plan as rejected. For activation, the trigger is the customer signing the agreement; this is a single-party signature, so no additional counterparty signature is required. The system attaches the signed agreement or signature reference and marks the plan as active.

## Business Events

Consumed:
- `Maintenance plan request rejected` from the reviewer rejecting the pending maintenance plan.
- `Maintenance plan agreement signed` from the customer signing the agreement.

Produced:
- `Maintenance plan rejected`, followed by `SI2.1 Notify user` in the rejection path.
- `Maintenance plan activated`, followed by returning the active maintenance plan to the customer.

## Questions

- Should approval before eSigning be represented as a separate status from active, such as `approved_pending_signature`?
- Should activation notify the user explicitly, or is returning the active maintenance plan enough?
