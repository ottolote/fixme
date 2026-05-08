# SI4.1 Create backoffice task

## Flow

Sources: `BP1.2-register-equipment.bpmn`, `BP1.3-enroll-in-maintenance-plan.bpmn`, `BP1.4-schedule-maintenance-job.bpmn`, and `BP1.5-cancel-maintenance-job-slot.bpmn`.

This interaction formalizes creation of backoffice work across BPMNs. The system receives a create-task command, validates the referenced subject and task type, creates the task, assigns or queues it for a backoffice worker, and returns success.

Known task types from the BPMNs are equipment registration review, maintenance plan review, maintenance slots proposal confirmation, and maintenance provider cancellation notification. For maintenance slots, the backoffice worker confirms with the maintenance provider that the proposed slots are acceptable before the customer is notified.

## Questions

- What assignment rules decide which backoffice worker receives the task?
- Should task type names be standardized as explicit enum values in the system interaction contract?
