# SI2.7 Accept maintenance slots proposal

## Flow

Source: `BP1.4-schedule-maintenance-job.bpmn`.

After receiving a maintenance slots proposal, the customer requests one maintenance slot. The system receives the selected-slot request from the client browser, validates the proposal and selected slot in separate checks, loads the proposal, removes reserved slots in the proposal, schedules the accepted maintenance slot, and returns the scheduled slot to the customer.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive selected maintenance slot request | Receives the customer's selected slot from the client browser. |
| 2 | Load maintenance slots proposal | Loads the proposal and its reserved slots. |
| 2a | Return proposal-not-found failure | Returns an error when the proposal cannot be found. |
| 3 | Check whether proposal is selectable | Validates that the proposal is active and can still be accepted. |
| 3a | Return proposal-not-selectable failure | Returns an error when the proposal cannot be accepted. |
| 4 | Check whether selected slot belongs to proposal | Validates that the selected slot is one of the proposal's reserved slots. |
| 4a | Return selected-slot-not-in-proposal failure | Returns an error when the selected slot is not part of the proposal. |
| 5 | Check whether selected slot is still available | Validates that the selected slot has not expired or been taken concurrently. |
| 5a | Return selected-slot-unavailable failure | Returns an error when the selected slot is unavailable. |
| 6 | Remove reserved slots from proposal | Releases or removes the proposal's reserved slots according to acceptance behavior. |
| 7 | Schedule accepted maintenance slot | Creates the scheduled maintenance job for the selected slot. |
| 8 | Produce business event: `Maintenance job scheduled` | Publishes that the selected maintenance slot was scheduled. |
| 9 | Return scheduled maintenance slot | Returns the scheduled slot details to the caller. |

## Business Events

Consumed:
- Selected maintenance slot request from the client browser after the customer chooses one slot from the proposal.

Produced:
- `Maintenance job scheduled` after the accepted slot is scheduled and the other reserved slots are released.

## Questions / Answers

| Question | Answer |
|---|---|
| Does “removes reserved job slots in proposal” mean releasing all non-selected slots or deleting the proposal entirely? | Answered. Keep the proposal record for audit/history, schedule the selected slot, and release all non-selected reserved slots. |
| How should the system handle a slot that expired or was accepted concurrently? | Answered. Treat acceptance as an atomic operation. If the selected slot is expired or no longer available at commit time, return `selected-slot-unavailable` and do not schedule the job. |
