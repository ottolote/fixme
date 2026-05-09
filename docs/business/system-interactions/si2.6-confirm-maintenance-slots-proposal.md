# SI2.6 Confirm maintenance slots proposal

## Flow

Source: `BP1.4-schedule-maintenance-job.bpmn`.

After `SI4.1 Create backoffice task` creates a maintenance slots proposal confirmation task, the backoffice worker confirms with the maintenance provider that the reserved slots are acceptable. The system receives the confirmation request, validates the task, proposal, and confirmed slot set in separate checks, closes the task, marks the proposal as confirmed, and emits the event that triggers `SI3.1 Notify user`.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive provider slot confirmation request | Receives the backoffice worker's confirmation result. |
| 2 | Load confirmation task | Loads the task representing the provider confirmation work. |
| 2a | Return task-not-found failure | Returns an error when the task cannot be loaded. |
| 3 | Check whether task is open | Validates that the task has not already been completed or cancelled. |
| 3a | Return task-not-open failure | Returns an error when the task is no longer open. |
| 4 | Check whether task type is maintenance slots proposal confirmation | Validates that this task is for provider slot confirmation. |
| 4a | Return invalid-task-type failure | Returns an error when the task type does not match this interaction. |
| 5 | Load maintenance slots proposal | Loads the proposal to confirm. |
| 5a | Return proposal-not-found failure | Returns an error when the proposal cannot be loaded. |
| 6 | Check whether proposal can be confirmed | Validates that the proposal still has active reserved slots and is not expired, cancelled, or already confirmed. |
| 6a | Return proposal-not-confirmable failure | Returns an error when the proposal can no longer be confirmed. |
| 7 | Check whether confirmed slots match proposal | Validates that the provider-confirmed slots are the reserved slots in the proposal. |
| 7a | Return slot-mismatch failure | Returns an error when confirmation references different slots. |
| 8 | Record provider slot confirmation | Stores the provider confirmation details. |
| 9 | Close confirmation task | Marks the backoffice task complete. |
| 10 | Mark maintenance slots proposal as confirmed | Persists the proposal confirmation. |
| 11 | Produce business event: `Maintenance slots proposal confirmed` | Publishes that the proposal can be sent to the customer. |
| 12 | Return success | Returns successful confirmation to the backoffice worker. |

## Business Events

Consumed:
- Provider slot confirmation request from the backoffice worker after confirming reserved slots with the provider.

Produced:
- `Maintenance slots proposal confirmed`, followed by `SI3.1 Notify user`.

## Questions / Answers

| Question | Answer |
|---|---|
| Why is provider confirmation handled here instead of a separate TaskingManager interaction? | Answered. The backoffice task is created by `SI4.1 Create backoffice task`, but completion of this specific task directly confirms the maintenance-domain proposal. Keep the confirmation as one MaintenanceManager interaction so there is only one producer of `Maintenance slots proposal confirmed`. |
