# SI2.4 Initiate eSigning for maintenance plan

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn` and `../references/use-cases/uc2.1-request-signatures.pu`.

After a maintenance plan request is approved, the system initiates eSigning. It consumes the approval business event, validates that the pending maintenance plan can be signed in separate checks, loads the plan, marks it approved pending signature, generates the maintenance plan agreement, requests a single customer signature through the eSigning capability, and stores the returned signature order reference. Successful processing emits a signature-requested event. Invalid or synchronously failed event processing is nacked to the DLQ.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume event: `Maintenance plan request approved` | Consumes approval of the pending maintenance plan request. |
| 2 | Load pending maintenance plan | Loads the plan data needed to create the agreement. |
| 2a | Nack event to DLQ: plan-not-found failure | Nacks the consumed event when the pending plan cannot be loaded. |
| 3 | Check whether plan is approved | Validates that the pending plan is approved for signing. |
| 3a | Nack event to DLQ: plan-not-approved failure | Nacks the consumed event when the plan is not approved. |
| 4 | Check whether customer is signable | Validates that the customer can receive and complete an eSigning request. |
| 4a | Nack event to DLQ: customer-not-signable failure | Nacks the consumed event when signature collection cannot be initiated for the customer. |
| 5 | Mark plan approved pending signature | Records that the plan is approved but not active until the customer signs. |
| 6 | Generate maintenance plan agreement | Creates the document or agreement payload for signature. |
| 7 | Request customer signature | Sends a command to the eSigning capability to request the customer's signature. |
| 7a | Nack event to DLQ: esigning-request-failed failure | Nacks the consumed event when the eSigning capability rejects the request. |
| 8 | Store signature order reference | Persists the eSigning provider/order reference for later tracking. |
| 9 | Produce business event: `Maintenance plan signature requested` | Publishes that signature collection has been initiated. |

## Business Events

Consumed:
- `Maintenance plan request approved` from the reviewer approving the pending maintenance plan.

Produced:
- `Maintenance plan signature requested`, which leads to the customer receiving the signature request.

## Questions / Answers

| Question | Answer |
|---|---|
| Does the eSigning reference model map directly to `../references/use-cases/uc2.1-request-signatures.pu`? | Answered. Yes. This interaction is the maintenance-plan-specific use of that generic eSigning capability: generate agreement, request the customer signature, and store the signature order reference. |
| Is the signed agreement document stored by this interaction, or only later by `SI2.3 Resolve pending maintenance plan` after the customer signs? | Answered. Store only the signature order reference here. Store or link the signed agreement later in `SI2.3 Resolve pending maintenance plan` when signed evidence is received. |
