# SI5.1 Initiate eSigning for maintenance plan

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn` and `../references/use-cases/uc2.1-request-signatures.pu`.

After a maintenance plan request is approved, the system initiates eSigning. It validates that the pending maintenance plan can be signed, loads the plan, generates the maintenance plan agreement, requests a single customer signature through the eSigning capability, stores the returned signature order reference, and returns the signature request. The BPMN then sends a signature request to the customer.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `Maintenance plan request approved` | Receives approval of the pending maintenance plan request. |
| 2 | Decide whether maintenance plan can be signed | Validates that the pending plan is approved and eligible for signature. |
| 3 | Load pending maintenance plan | Loads the plan data needed to create the agreement. |
| 4 | Generate maintenance plan agreement | Creates the document or agreement payload for signature. |
| 5 | Request customer signature | Sends a command to the eSigning capability to request the customer's signature. |
| 6 | Store signature order reference | Persists the eSigning provider/order reference for later tracking. |
| 7 | Produce business event: `Maintenance plan signature requested` | Publishes that signature collection has been initiated. |
| 8 | Return signature request | Returns the signature request information to the caller. |
| 9 | Return failure | Returns an error when the plan cannot be signed. |

## Business Events

Consumed:
- `Maintenance plan request approved` from the reviewer approving the pending maintenance plan.

Produced:
- `Maintenance plan signature requested`, which leads to the customer receiving the signature request.

## Questions / Answers

| Question | Answer |
|---|---|
| Does the eSigning reference model map directly to `../references/use-cases/uc2.1-request-signatures.pu`? | Open. The system interaction references the eSigning capability, but the exact mapping is not confirmed in this diagram. |
| Is the signed agreement document stored by this interaction, or only later by `SI3.3 Resolve pending maintenance plan` after the customer signs? | Open. This interaction stores the signature order reference; `SI3.3 Resolve pending maintenance plan` attaches the customer-signed agreement. |
