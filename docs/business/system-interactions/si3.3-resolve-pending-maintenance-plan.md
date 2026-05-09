# SI3.3 Resolve pending maintenance plan

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn`.

This interaction is used in two different BPMN contexts. After backoffice review rejection, the system resolves the pending maintenance plan as rejected and then calls `SI2.1 Notify user`. After the customer signs the maintenance plan agreement, the system resolves the pending maintenance plan as active and returns the active maintenance plan to the customer.

The system receives a maintenance plan resolution request from either a backoffice rejection or a signed-agreement event, validates the requested outcome, loads the pending maintenance plan, checks that it can still be resolved, and applies the requested outcome. For rejection, it stores a rejection reason when provided and marks the plan as rejected. For activation, the trigger is the customer signing the agreement; this is a single-party signature, so no additional counterparty signature is required. The system attaches the signed agreement or signature reference and marks the plan as active.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume maintenance plan resolution trigger | Receives either a backoffice rejection request or a signed-agreement event. |
| 2 | Check whether resolution outcome is valid | Validates that the requested outcome is activation or rejection. |
| 2a | Return invalid-outcome failure | Returns an error when the requested outcome is unsupported. |
| 3 | Load pending maintenance plan | Loads the pending plan to resolve. |
| 3a | Return plan-not-found failure | Returns an error when the pending plan cannot be loaded. |
| 4 | Check whether maintenance plan is still pending | Validates that the plan has not already been rejected or activated. |
| 4a | Return already-resolved failure | Returns an error when the plan is no longer pending. |
| 5 | Decide whether the resolution is activation | Routes to activation for signed agreements or rejection for rejected requests. |
| 6 | Check whether signed agreement evidence is present | Validates that activation has signed-agreement evidence. |
| 6a | Return missing-signature-evidence failure | Returns an error when activation lacks signed-agreement evidence. |
| 7 | Attach customer-signed agreement | Stores or links the signed agreement evidence on the plan. |
| 8 | Mark maintenance plan as active | Activates the maintenance plan. |
| 9 | Produce business event: `Maintenance plan activated` | Publishes the activation outcome. |
| 10 | Store rejection reason if provided | Records the reviewer's rejection reason when supplied. |
| 11 | Mark maintenance plan as rejected | Updates the pending plan to rejected. |
| 12 | Produce business event: `Maintenance plan rejected` | Publishes the rejection outcome. |
| 13 | Return success | Returns a successful resolution response. |

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
