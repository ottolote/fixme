# SI3.7 Create maintenance job slots proposal

## Flow

Source: `BP1.4-schedule-maintenance-job.bpmn`.

The customer requests a maintenance slot for equipment. The system receives the maintenance-slot request from the client browser, validates the equipment and active maintenance plan in separate checks, loads the equipment and plan, finds available maintenance job slots owned by the system, reserves three proposed slots, creates a maintenance slots proposal, and returns success. The BPMN annotation explicitly says this interaction reserves job slots.

The BPMN then performs a separate system task named "Schedule backoffice task for confirming maintenance slot" before the backoffice worker confirms the proposal with the maintenance provider. This should be formalized as `SI4.1 Create backoffice task`. The proposal is only sent to the customer after backoffice confirmation.

Slot reservations should expire if they are not confirmed and sent to the customer within a reasonable period. Default behavior: hold the reservation for 24 hours, then release the reserved slots and expire the proposal if it has not been confirmed.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive maintenance-slot request | Receives the customer's request for maintenance slots from the client browser. |
| 2 | Check whether equipment exists | Validates that the target equipment can be found. |
| 2a | Return equipment-not-found failure | Returns an error when the equipment cannot be found. |
| 3 | Check whether equipment has active maintenance plan | Validates that the equipment is covered by an active plan. |
| 3a | Return no-active-plan failure | Returns an error when the equipment lacks an active maintenance plan. |
| 4 | Load equipment and maintenance plan | Loads the equipment and plan data needed to search slots. |
| 5 | Find system-owned available maintenance job slots | Searches available maintenance slots owned by the system. |
| 6 | Check whether at least 3 slots are available | Validates that enough slots exist to create the proposal. |
| 6a | Return insufficient-slots failure | Returns an error when fewer than three slots are available. |
| 7 | Reserve 3 proposed job slots for 24 hours | Temporarily holds three candidate slots for the customer proposal. |
| 8 | Create maintenance slots proposal | Persists the proposal containing the reserved candidate slots. |
| 9 | Produce business event: `Maintenance slots proposal created` | Publishes that a proposal was created and slots are reserved. |
| 10 | Return success | Returns a successful proposal creation response. |

## Business Events

Consumed:
- Maintenance-slot request from the client browser after the customer requests a maintenance slot for equipment.

Produced:
- `Maintenance slots proposal created`, with the proposed job slots reserved.

## Questions / Answers

| Question | Answer |
|---|---|
| Should the 24-hour reservation timeout be configurable per equipment type, provider, or market? | Answered. Keep 24 hours as the global default. Do not add per-equipment, provider, or market configuration until a concrete business rule requires it. |
