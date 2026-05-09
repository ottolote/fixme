# SI3.2 Create pending maintenance plan

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn`.

After receiving plan options, the user picks a maintenance plan. The system receives the selected-plan request from the client browser, validates the selected option in separate checks, loads the customer, equipment, and offering, creates a pending maintenance plan, and returns success. The BPMN then creates a backoffice task for maintenance plan review.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive selected-plan request | Receives the selected maintenance plan offering from the client browser. |
| 2 | Check whether selected offering exists | Validates that the selected offering can be found. |
| 2a | Return offering-not-found failure | Returns an error when the selected offering does not exist. |
| 3 | Check whether selected offering is still available | Validates that the offering can still be selected. |
| 3a | Return offering-unavailable failure | Returns an error when the offering is no longer available. |
| 4 | Check whether offering applies to equipment | Validates that the selected offering applies to the chosen equipment. |
| 4a | Return offering-not-applicable failure | Returns an error when the offering does not apply to the equipment. |
| 5 | Load customer, equipment, and offering | Loads the records required to create the pending plan. |
| 6 | Create pending maintenance plan | Persists a maintenance plan request in pending state. |
| 7 | Produce business event: `Pending maintenance plan created` | Publishes that the plan request is ready for review. |
| 8 | Return success | Returns a successful pending-plan creation response. |

## Business Events

Consumed:
- Selected-plan request from the client browser after the user picks one of the returned plan options.

Produced:
- `Pending maintenance plan created`, which is followed by `SI4.1 Create backoffice task` for review.

## Questions / Answers

| Question | Answer |
|---|---|
| Should pricing be locked when the pending plan is created? | Open. The diagram creates the pending plan but does not define price-locking behavior. |
| Can a customer have more than one pending maintenance plan for the same equipment? | Open. The selection validation step does not specify duplicate pending-plan rules. |
