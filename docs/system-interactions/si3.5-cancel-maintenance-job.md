# SI3.5 Cancel maintenance job

## Flow

Source: `BP1.5-cancel-maintenance-job-slot.bpmn`.

The customer cancels a selected maintenance job slot. The system receives the cancellation request, validates that the scheduled maintenance job can be cancelled, marks it as cancelled, creates a backoffice task, and returns success. The BPMN then shows a backoffice worker notifying the maintenance provider of the cancellation.

## Business Events

Consumed:
- `Maintenance job cancellation requested` from the customer cancelling the selected scheduled slot.

Produced:
- `Maintenance job cancelled` after the scheduled job is marked cancelled.
- `Maintenance provider cancellation task created`, which starts the backoffice provider-notification flow in `BP1.5-cancel-maintenance-job-slot.bpmn`.

## Questions

- What cancellation rules apply, such as cut-off times or cancellation fees?
- Should the maintenance provider notification be manual only, or should the system also notify the provider directly?
