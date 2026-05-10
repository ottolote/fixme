# SI4.1 Create backoffice task

## Flow

Sources: `BP1.2-register-equipment.bpmn`, `BP1.3-enroll-in-maintenance-plan.bpmn`, `BP1.4-schedule-maintenance-job.bpmn`, and `BP1.5-cancel-maintenance-job-slot.bpmn`.

This interaction formalizes creation of backoffice work across BPMNs. The system consumes a business event that requires backoffice work, validates the referenced subject and task type in separate checks, creates the task, and queues it in the relevant provider or backoffice work pile. Successful processing emits a task-created event. Invalid event processing is nacked to the DLQ.

The PlantUML diagram combines validation branches into a single gate; the table keeps the specific failure outcomes.

Known task types from the BPMNs are equipment registration review, maintenance plan review, maintenance slots proposal confirmation, and maintenance provider cancellation notification. Task creation is formalized through this TaskingManager interaction rather than being created directly by the domain interactions. For equipment registrations needing review, the backoffice worker reviews and later resolves the pending registration through `SI1.7 Resolve pending registration`. For maintenance slots, the backoffice worker confirms with the maintenance provider that the proposed slots are acceptable before the customer is notified.

## Steps

| Step | Step detail | Actions performed |
|---|---|---|
| 1 | Consume event: backoffice-work business event | Consumes a business event that requires backoffice work. |
| 2 | Check whether task type is supported | Validates that the requested task type is known. |
| 2a | Nack event to DLQ: unsupported-task-type failure | Nacks the consumed event when the task type is unsupported. |
| 3 | Check whether task subject exists | Validates that the referenced entity exists. |
| 3a | Nack event to DLQ: subject-not-found failure | Nacks the consumed event when the task subject cannot be found. |
| 4 | Check whether task is not already open | Prevents duplicate open tasks for the same subject and task type. |
| 4a | Nack event to DLQ: duplicate-task failure | Nacks the consumed event when an equivalent task is already open. |
| 5 | Create backoffice task | Persists a task for the backoffice team. |
| 6 | Queue task in provider or backoffice work pile | Routes the task to the relevant provider or backoffice shared work pile. |
| 7 | Produce business event: `Backoffice task created` | Publishes that the task was created. |

## Business Events

Consumed:
- `Equipment registration review requested` to create an equipment registration review task.
- `Pending maintenance plan created` to create a maintenance plan review task.
- `Maintenance slots proposal created` to create a maintenance slots proposal confirmation task.
- `Maintenance job cancelled` to create a maintenance provider cancellation notification task.

Produced:
- `Equipment registration review task created`.
- `Maintenance plan review task created`.
- `Maintenance slots proposal confirmation task created`.
- `Maintenance provider cancellation task created`.

## Questions / Answers

| Question | Answer |
|---|---|
| What assignment rules decide which backoffice worker receives the task? | Answered. Do not assign tasks to individual workers initially. Route work to the relevant provider or backoffice shared pile and let provider/backoffice staff pick it up. |
| Should task type names be standardized as explicit enum values in the system interaction contract? | Answered. Yes. Standardize task type enum values in the interaction contract, starting with `equipment_registration_review`, `maintenance_plan_review`, `maintenance_slots_proposal_confirmation`, and `maintenance_provider_cancellation_notification`. |
