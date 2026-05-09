# SI4.1 Create backoffice task

## Flow

Sources: `BP1.3-enroll-in-maintenance-plan.bpmn` and `BP1.4-schedule-maintenance-job.bpmn`.

This interaction formalizes creation of backoffice work across BPMNs. The system receives a create-task command, validates the referenced subject and task type, creates the task, assigns or queues it for a backoffice worker, and returns success.

Known task types from the BPMNs are maintenance plan review and maintenance slots proposal confirmation. Equipment registration review is started directly by `SI1.5 Create pending registration`, and maintenance provider cancellation notification is created directly by `SI3.5 Cancel maintenance job`. For maintenance slots, the backoffice worker confirms with the maintenance provider that the proposed slots are acceptable before the customer is notified.

## Business Events

Consumed:
- `Pending maintenance plan created` to create a maintenance plan review task.
- `Maintenance slots proposal created` to create a maintenance slots proposal confirmation task.

Produced:
- `Maintenance plan review task created`.
- `Maintenance slots proposal confirmation task created`.

## Questions

- What assignment rules decide which backoffice worker receives the task?
- Should task type names be standardized as explicit enum values in the system interaction contract?
