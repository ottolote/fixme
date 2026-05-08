# SI3.3 Reject maintenance plan

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn`.

After review, the maintenance plan request can be rejected. The system validates that the plan is still pending, marks it as rejected, stores any rejection reason, and returns success. The BPMN then continues with `SI2.2 Notify customer of rejected maintenance plan`.

## Questions

- What actor or system performs the approval/rejection decision? The BPMN has a second participant also named `System` for this decision.
- Is a rejection reason required for customer notification?
