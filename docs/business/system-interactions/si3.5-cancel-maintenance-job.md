# SI3.5 Cancel maintenance job

## Flow

Source: `BP1.5-cancel-maintenance-job-slot.bpmn`.

The customer cancels a selected maintenance job slot. The system receives the cancellation request, validates that the scheduled maintenance job can be cancelled, marks it as cancelled, creates a backoffice task, and returns success. The BPMN then shows a backoffice worker notifying the maintenance provider of the cancellation.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `Maintenance job cancellation requested` | Receives the customer's cancellation request. |
| 2 | Decide whether scheduled maintenance job can be cancelled | Validates job state and cancellation rules. |
| 3 | Load scheduled maintenance job | Loads the scheduled job to cancel. |
| 4 | Mark job as cancelled | Updates the job status to cancelled. |
| 5 | Create backoffice task to notify provider | Creates manual provider-notification work for backoffice. |
| 6 | Produce business event: `Maintenance job cancelled` | Publishes that the job was cancelled. |
| 7 | Produce business event: `Maintenance provider cancellation task created` | Publishes that provider-notification work was created. |
| 8 | Return success | Returns a successful cancellation response. |
| 9 | Return failure | Returns an error when the job cannot be cancelled. |

## Business Events

Consumed:
- `Maintenance job cancellation requested` from the customer cancelling the selected scheduled slot.

Produced:
- `Maintenance job cancelled` after the scheduled job is marked cancelled.
- `Maintenance provider cancellation task created`, which starts the backoffice provider-notification flow in `BP1.5-cancel-maintenance-job-slot.bpmn`.

## Questions / Answers

| Question | Answer |
|---|---|
| What cancellation rules apply, such as cut-off times or cancellation fees? | Open. The diagram validates cancellability but does not define the business rules. |
| Should the maintenance provider notification be manual only, or should the system also notify the provider directly? | Open. The current flow creates a backoffice task to notify the provider manually. |
