# SI3.2 Create pending maintenance plan

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn`.

After receiving plan options, the user picks a maintenance plan. The system validates the selected option, loads the customer, equipment, and offering, creates a pending maintenance plan, and returns success. The BPMN then creates a backoffice task for maintenance plan review.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `Maintenance plan selected` | Receives the selected maintenance plan offering. |
| 2 | Decide whether selection is valid | Validates that the selected offering is applicable and still available. |
| 3 | Load customer, equipment, and offering | Loads the records required to create the pending plan. |
| 4 | Create pending maintenance plan | Persists a maintenance plan request in pending state. |
| 5 | Produce business event: `Pending maintenance plan created` | Publishes that the plan request is ready for review. |
| 6 | Return success | Returns a successful pending-plan creation response. |
| 7 | Return failure | Returns an error when the selection is invalid. |

## Business Events

Consumed:
- `Maintenance plan selected` from the user picking one of the returned plan options.

Produced:
- `Pending maintenance plan created`, which is followed by `SI4.1 Create backoffice task` for review.

## Questions / Answers

| Question | Answer |
|---|---|
| Should pricing be locked when the pending plan is created? | Open. The diagram creates the pending plan but does not define price-locking behavior. |
| Can a customer have more than one pending maintenance plan for the same equipment? | Open. The selection validation step does not specify duplicate pending-plan rules. |
