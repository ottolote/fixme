# SI2.2 Create pending maintenance plan

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn`.

After receiving plan options, the user picks a maintenance plan. The system receives the selected-plan request from the client browser, validates the selected option in separate checks, rejects duplicate pending or active plans for the same equipment, loads the customer, equipment, and offering, locks the selected offering price, creates a pending maintenance plan, and returns success. The BPMN then creates a backoffice task for maintenance plan review.

The PlantUML diagram combines validation branches into a single gate; the table keeps the specific failure outcomes.

## Steps

| Step | Step detail | Actions performed |
|---|---|---|
| 1 | Receive selected-plan request | Receives the selected maintenance plan offering from the client browser. |
| 2 | Check whether selected offering exists | Validates that the selected offering can be found. |
| 2a | Return offering-not-found failure | Returns an error when the selected offering does not exist. |
| 3 | Check whether selected offering is still available | Validates that the offering can still be selected. |
| 3a | Return offering-unavailable failure | Returns an error when the offering is no longer available. |
| 4 | Check whether offering applies to equipment | Validates that the selected offering applies to the chosen equipment. |
| 4a | Return offering-not-applicable failure | Returns an error when the offering does not apply to the equipment. |
| 5 | Check whether equipment has no pending or active plan | Prevents duplicate maintenance-plan requests for the same equipment. |
| 5a | Return duplicate-plan failure | Returns an error when the equipment already has a pending or active maintenance plan. |
| 6 | Load customer, equipment, and offering | Loads the records required to create the pending plan. |
| 7 | Lock selected offering price | Stores the selected commercial terms on the pending plan. |
| 8 | Create pending maintenance plan | Persists a maintenance plan request in pending state. |
| 9 | Produce business event: `Pending maintenance plan created` | Publishes that the plan request is ready for review. |
| 10 | Return success | Returns a successful pending-plan creation response. |

## Business Events

Consumed:
- Selected-plan request from the client browser after the user picks one of the returned plan options.

Produced:
- `Pending maintenance plan created`, followed by `SI4.1 Create backoffice task` for review.

## Questions / Answers

| Question | Answer |
|---|---|
| Should pricing be locked when the pending plan is created? | Answered. Lock the selected offering price when the pending plan is created so backoffice review, agreement generation, and eSigning use the same commercial terms the customer selected. |
| Can a customer have more than one pending maintenance plan for the same equipment? | Answered. No. Reject a second pending or active maintenance plan for the same equipment to avoid duplicate review tasks, duplicate agreements, and ambiguous coverage. |
