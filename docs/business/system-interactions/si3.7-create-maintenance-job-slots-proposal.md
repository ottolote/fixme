# SI3.7 Create maintenance job slots proposal

## Flow

Source: `BP1.4-schedule-maintenance-job.bpmn`.

The customer requests a maintenance slot for equipment. The system validates that the equipment has an active maintenance plan, loads the equipment and plan, finds available maintenance job slots owned by the system, reserves three proposed slots, creates a maintenance slots proposal, and returns success. The BPMN annotation explicitly says this interaction reserves job slots.

The BPMN then performs a separate system task named "Schedule backoffice task for confirming maintenance slot" before the backoffice worker confirms the proposal with the maintenance provider. This should be formalized as `SI4.1 Create backoffice task`. The proposal is only sent to the customer after backoffice confirmation.

Slot reservations should expire if they are not confirmed and sent to the customer within a reasonable period. Default behavior: hold the reservation for 24 hours, then release the reserved slots and expire the proposal if it has not been confirmed.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `Maintenance slot requested` | Receives the customer's request for maintenance slots for equipment. |
| 2 | Decide whether equipment has active maintenance plan | Validates that the equipment is covered by an active plan. |
| 3 | Load equipment and maintenance plan | Loads the equipment and plan data needed to search slots. |
| 4 | Find system-owned available maintenance job slots | Searches available maintenance slots owned by the system. |
| 5 | Reserve 3 proposed job slots for 24 hours | Temporarily holds three candidate slots for the customer proposal. |
| 6 | Create maintenance slots proposal | Persists the proposal containing the reserved candidate slots. |
| 7 | Produce business event: `Maintenance slots proposal created` | Publishes that a proposal was created and slots are reserved. |
| 8 | Return success | Returns a successful proposal creation response. |
| 9 | Return failure | Returns an error when the equipment lacks an active maintenance plan. |

## Business Events

Consumed:
- `Maintenance slot requested` from the customer requesting a maintenance slot for equipment.

Produced:
- `Maintenance slots proposal created`, with the proposed job slots reserved.

## Questions / Answers

| Question | Answer |
|---|---|
| Should the 24-hour reservation timeout be configurable per equipment type, provider, or market? | Open. The diagram uses a 24-hour reservation period as the documented default. |
