# SI4.1 Create backoffice task

## Flow

Sources: `BP1.3-enroll-in-maintenance-plan.bpmn` and `BP1.4-schedule-maintenance-job.bpmn`.

This interaction formalizes creation of backoffice work across BPMNs. The system consumes a business event that requires backoffice work, validates the referenced subject and task type in separate checks, creates the task, and assigns or queues it for a backoffice worker. Successful processing emits a task-created event. Invalid event processing is nacked to the DLQ.

Known task types from the BPMNs are maintenance plan review and maintenance slots proposal confirmation. Equipment registration review is started directly by `SI1.5 Create pending registration`, and maintenance provider cancellation notification is created directly by `SI3.5 Cancel maintenance job`. For maintenance slots, the backoffice worker confirms with the maintenance provider that the proposed slots are acceptable before the customer is notified.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume event: backoffice-work business event | Consumes a business event that requires backoffice work. |
| 2 | Check whether task type is supported | Validates that the requested task type is known. |
| 2a | Nack event to DLQ: unsupported-task-type failure | Nacks the consumed event when the task type is unsupported. |
| 3 | Check whether task subject exists | Validates that the referenced entity exists. |
| 3a | Nack event to DLQ: subject-not-found failure | Nacks the consumed event when the task subject cannot be found. |
| 4 | Check whether task is not already open | Prevents duplicate open tasks for the same subject and task type. |
| 4a | Nack event to DLQ: duplicate-task failure | Nacks the consumed event when an equivalent task is already open. |
| 5 | Create backoffice task | Persists a task for the backoffice team. |
| 6 | Assign or queue task for backoffice worker | Routes the task to a worker or backoffice queue. |
| 7 | Produce business event: `Backoffice task created` | Publishes that the task was created. |

## Business Events

Consumed:
- `Pending maintenance plan created` to create a maintenance plan review task.
- `Maintenance slots proposal created` to create a maintenance slots proposal confirmation task.

Produced:
- `Maintenance plan review task created`.
- `Maintenance slots proposal confirmation task created`.

## Questions / Answers

| Question | Answer |
|---|---|
| What assignment rules decide which backoffice worker receives the task? | Open. The diagram assigns or queues the task but does not specify routing rules. |
| Should task type names be standardized as explicit enum values in the system interaction contract? | Answered. Yes. Standardize task type enum values in the interaction contract, starting with `maintenance_plan_review` and `maintenance_slots_proposal_confirmation`. |
