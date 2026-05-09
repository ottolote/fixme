# SI3.5 Cancel maintenance job

## Flow

Source: `BP1.5-cancel-maintenance-job-slot.bpmn`.

The customer cancels a selected maintenance job slot. The system receives the cancellation request from the client browser, validates the job and the 24-hour cancellation cutoff in separate checks, marks it as cancelled, creates a provider-pile backoffice task, and returns success. The BPMN then shows a backoffice worker notifying the maintenance provider of the cancellation.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive maintenance job cancellation request | Receives the customer's cancellation request from the client browser. |
| 2 | Load scheduled maintenance job | Loads the scheduled job to cancel. |
| 2a | Return job-not-found failure | Returns an error when the scheduled job cannot be found. |
| 3 | Check whether job is scheduled | Validates that the job is in a cancellable scheduled state. |
| 3a | Return job-not-scheduled failure | Returns an error when the job is not scheduled. |
| 4 | Check whether cancellation is more than 24 hours before start | Validates the customer self-service cancellation cutoff. |
| 4a | Return cancellation-not-allowed failure | Returns an error when the job starts within 24 hours and customer self-service cancellation is not allowed. |
| 5 | Mark job as cancelled | Updates the job status to cancelled. |
| 6 | Create provider-pile backoffice task to notify provider | Creates manual provider-notification work in the relevant maintenance provider's shared pile. |
| 7 | Produce business event: `Maintenance job cancelled` | Publishes that the job was cancelled. |
| 8 | Produce business event: `Maintenance provider cancellation task created` | Publishes that provider-notification work was created. |
| 9 | Return success | Returns a successful cancellation response. |

## Business Events

Consumed:
- Maintenance job cancellation request from the client browser after the customer cancels the selected scheduled slot.

Produced:
- `Maintenance job cancelled` after the scheduled job is marked cancelled.
- `Maintenance provider cancellation task created`, which starts the backoffice provider-notification flow in `BP1.5-cancel-maintenance-job-slot.bpmn`.

## Questions / Answers

| Question | Answer |
|---|---|
| What cancellation rules apply, such as cut-off times or cancellation fees? | Answered. Allow free cancellation until 24 hours before the scheduled start. Within 24 hours, reject customer self-service cancellation and route the customer to support/backoffice handling. No cancellation fees are modeled initially. |
| Should the maintenance provider notification be manual only, or should the system also notify the provider directly? | Answered. Keep provider notification manual initially. Create a backoffice task routed to the relevant maintenance provider's pile, and let backoffice/provider staff handle the notification outside the system. |
