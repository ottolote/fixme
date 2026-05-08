# SI3.5 Cancel maintenance job

## Flow

Source: `BP1.5-cancel-maintenance-job-slot.bpmn`.

The customer cancels a selected maintenance job slot. The system receives the cancellation request, validates that the scheduled maintenance job can be cancelled, marks it as cancelled, creates a backoffice task, and returns success. The BPMN then shows a backoffice worker notifying the maintenance provider of the cancellation.

## Questions

- What cancellation rules apply, such as cut-off times or cancellation fees?
- Should the maintenance provider notification be manual only, or should the system also notify the provider directly?
