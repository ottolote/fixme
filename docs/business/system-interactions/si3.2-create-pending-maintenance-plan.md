# SI3.2 Create pending maintenance plan

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn`.

After receiving plan options, the user picks a maintenance plan. The system validates the selected option, loads the customer, equipment, and offering, creates a pending maintenance plan, and returns success. The BPMN then creates a backoffice task for maintenance plan review.

## Business Events

Consumed:
- `Maintenance plan selected` from the user picking one of the returned plan options.

Produced:
- `Pending maintenance plan created`, which is followed by `SI4.1 Create backoffice task` for review.

## Questions

- Should pricing be locked when the pending plan is created?
- Can a customer have more than one pending maintenance plan for the same equipment?
