# SI3.7 Create maintenance job slots proposal

## Flow

Source: `BP1.4-schedule-maintenance-job.bpmn`.

The customer requests a maintenance slot for equipment. The system validates that the equipment has an active maintenance plan, loads the equipment and plan, finds available maintenance job slots owned by the system, reserves three proposed slots, creates a maintenance slots proposal, and returns success. The BPMN annotation explicitly says this interaction reserves job slots.

The BPMN then performs a separate system task named "Schedule backoffice task for confirming maintenance slot" before the backoffice worker confirms the proposal with the maintenance provider. This should be formalized as `SI4.1 Create backoffice task`. The proposal is only sent to the customer after backoffice confirmation.

Slot reservations should expire if they are not confirmed and sent to the customer within a reasonable period. Default behavior: hold the reservation for 24 hours, then release the reserved slots and expire the proposal if it has not been confirmed.

## Business Events

Consumed:
- `Maintenance slot requested` from the customer requesting a maintenance slot for equipment.

Produced:
- `Maintenance slots proposal created`, with the proposed job slots reserved.

## Questions

- Should the 24-hour reservation timeout be configurable per equipment type, provider, or market?
