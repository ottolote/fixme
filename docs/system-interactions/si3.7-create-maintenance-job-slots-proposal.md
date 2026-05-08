# SI3.7 Create maintenance job slots proposal

## Flow

Source: `BP1.4-schedule-maintenance-job.bpmn`.

The customer requests a maintenance slot for equipment. The system validates that the equipment has an active maintenance plan, loads the equipment and plan, finds available maintenance job slots, reserves the proposed slots, creates a maintenance slots proposal, and creates a backoffice confirmation task. The BPMN annotation explicitly says this interaction reserves job slots.

## Questions

- Which service owns available maintenance job slots and reservations?
- How many slots should be proposed and for how long are they reserved?
- Is creating the backoffice confirmation task part of this interaction or a separate `SI4.1` use?
