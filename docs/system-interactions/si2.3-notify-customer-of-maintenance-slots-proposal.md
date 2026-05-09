# SI2.3 Notify customer of maintenance slots proposal

## Flow

Source: `BP1.4-schedule-maintenance-job.bpmn`.

After the backoffice worker confirms the maintenance slots proposal, the system notifies the customer that the proposal is available. The system validates that the proposal can be sent, loads the proposal, resolves the customer notification channel, renders the proposal notification, sends it, and returns success or failure.

## Business Events

Consumed:
- `Maintenance slots proposal confirmed` from the backoffice worker confirming the proposal.

Produced:
- `Maintenance slots proposal notification sent`, which the customer receives before selecting a slot.

## Questions

- Should this stay as a dedicated `SI2.3`, or should it be merged into generic `SI2.1 Notify user` with a specific template and event type?
