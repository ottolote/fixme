# SI3.3 Resolve pending maintenance plan

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn`.

This interaction is used in two different BPMN contexts. After backoffice review rejection, the system resolves the pending maintenance plan as rejected and then calls `SI2.1 Notify user`. After the customer signs the maintenance plan agreement, the system resolves the pending maintenance plan as active and returns the active maintenance plan to the customer.

The system receives a maintenance plan resolution command or event, validates that the pending maintenance plan can still be resolved, loads the pending maintenance plan, and applies the requested outcome. For rejection, it stores a rejection reason when provided and marks the plan as rejected. For activation, the trigger is the customer signing the agreement; this is a single-party signature, so no additional counterparty signature is required. The system attaches the signed agreement or signature reference and marks the plan as active.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `Maintenance plan request rejected or agreement signed` | Receives either a rejection decision or a signed-agreement event. |
| 2 | Decide whether the resolution request is valid | Validates that the pending plan exists and can still be resolved. |
| 3 | Load pending maintenance plan | Loads the pending plan to resolve. |
| 4 | Decide whether the resolution is activation | Routes to activation for signed agreements or rejection for rejected requests. |
| 5 | Attach customer-signed agreement | Stores or links the signed agreement evidence on the plan. |
| 6 | Mark maintenance plan as active | Activates the maintenance plan. |
| 7 | Produce business event: `Maintenance plan activated` | Publishes the activation outcome. |
| 8 | Store rejection reason if provided | Records the reviewer's rejection reason when supplied. |
| 9 | Mark maintenance plan as rejected | Updates the pending plan to rejected. |
| 10 | Produce business event: `Maintenance plan rejected` | Publishes the rejection outcome. |
| 11 | Return success | Returns a successful resolution response. |
| 12 | Return failure | Returns an error when the resolution request is invalid. |

## Business Events

Consumed:
- `Maintenance plan request rejected` from the reviewer rejecting the pending maintenance plan.
- `Maintenance plan agreement signed` from the customer signing the agreement.

Produced:
- `Maintenance plan rejected`, followed by `SI2.1 Notify user` in the rejection path.
- `Maintenance plan activated`, followed by returning the active maintenance plan to the customer.

## Questions / Answers

| Question | Answer |
|---|---|
| Should approval before eSigning be represented as a separate status from active, such as `approved_pending_signature`? | Open. The diagram only resolves rejection or activation after agreement signature. |
| Should activation notify the user explicitly, or is returning the active maintenance plan enough? | Open. The BPMN returns the active maintenance plan but does not show a separate activation notification. |
