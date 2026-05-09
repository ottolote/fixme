# SI3.6 Accept maintenance slots proposal

## Flow

Source: `BP1.4-schedule-maintenance-job.bpmn`.

After receiving a maintenance slots proposal, the customer requests one maintenance slot. The system validates the proposal and selected slot, loads the proposal, removes reserved slots in the proposal, schedules the accepted maintenance slot, and returns the scheduled slot to the customer.

## Business Events

Consumed:
- `Maintenance slot selected` from the customer choosing one slot from the proposal.

Produced:
- `Maintenance job scheduled` after the accepted slot is scheduled and the other reserved slots are released.

## Questions

- Does “removes reserved job slots in proposal” mean releasing all non-selected slots or deleting the proposal entirely?
- How should the system handle a slot that expired or was accepted concurrently?
