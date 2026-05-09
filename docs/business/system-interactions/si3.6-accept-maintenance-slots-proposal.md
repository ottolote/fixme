# SI3.6 Accept maintenance slots proposal

## Flow

Source: `BP1.4-schedule-maintenance-job.bpmn`.

After receiving a maintenance slots proposal, the customer requests one maintenance slot. The system validates the proposal and selected slot, loads the proposal, removes reserved slots in the proposal, schedules the accepted maintenance slot, and returns the scheduled slot to the customer.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `Maintenance slot selected` | Receives the customer's selected slot from the proposal. |
| 2 | Decide whether proposal and selected slot are valid | Validates the proposal state and that the selected slot is available and belongs to the proposal. |
| 3 | Load maintenance slots proposal | Loads the proposal and its reserved slots. |
| 4 | Remove reserved slots from proposal | Releases or removes the proposal's reserved slots according to acceptance behavior. |
| 5 | Schedule accepted maintenance slot | Creates the scheduled maintenance job for the selected slot. |
| 6 | Produce business event: `Maintenance job scheduled` | Publishes that the selected maintenance slot was scheduled. |
| 7 | Return scheduled maintenance slot | Returns the scheduled slot details to the caller. |
| 8 | Return failure | Returns an error when the proposal or selected slot is invalid. |

## Business Events

Consumed:
- `Maintenance slot selected` from the customer choosing one slot from the proposal.

Produced:
- `Maintenance job scheduled` after the accepted slot is scheduled and the other reserved slots are released.

## Questions / Answers

| Question | Answer |
|---|---|
| Does “removes reserved job slots in proposal” mean releasing all non-selected slots or deleting the proposal entirely? | Open. The BPMN annotation confirms reserved slots are removed, but not whether the proposal record remains. |
| How should the system handle a slot that expired or was accepted concurrently? | Open. The diagram validates the proposal and selected slot but does not define concurrency behavior. |
