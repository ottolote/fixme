# SI2.3 Notify customer of maintenance slots proposal

## Flow

Source: `BP1.4-schedule-maintenance-job.bpmn`.

After the backoffice worker confirms the maintenance slots proposal, the system notifies the customer that the proposal is available. The system consumes the proposal-confirmed business event, validates that the proposal can be sent in separate checks, loads the proposal, resolves the customer notification channel, renders the proposal notification, sends it, and returns success or failure.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `Maintenance slots proposal confirmed` | Receives the event that the proposal is ready for customer notification. |
| 2 | Load maintenance slots proposal | Loads proposal details and proposed slot data for the notification. |
| 2a | Return proposal-not-found failure | Returns an error when the proposal cannot be loaded. |
| 3 | Check whether proposal is confirmed | Validates that the proposal is in the confirmed state. |
| 3a | Return proposal-not-confirmed failure | Returns an error when the proposal is not confirmed. |
| 4 | Check whether proposal has active reserved slots | Validates that the proposed slots are still reserved and available. |
| 4a | Return slots-unavailable failure | Returns an error when the reserved slots are unavailable. |
| 5 | Resolve customer notification channel | Determines the customer's delivery channel. |
| 5a | Return channel-unavailable failure | Returns an error when the customer cannot be notified. |
| 6 | Render maintenance slots proposal notification | Builds the customer-facing proposal message. |
| 7 | Send notification to customer | Delivers the proposal notification to the customer. |
| 7a | Return delivery-failed failure | Returns an error when notification delivery fails synchronously. |
| 8 | Produce business event: `Maintenance slots proposal notification sent` | Publishes that the customer notification was sent. |
| 9 | Return success | Returns a successful notification response. |

## Business Events

Consumed:
- `Maintenance slots proposal confirmed` from the backoffice worker confirming the proposal.

Produced:
- `Maintenance slots proposal notification sent`, which the customer receives before selecting a slot.

## Questions / Answers

| Question | Answer |
|---|---|
| Should this stay as a dedicated `SI2.3`, or should it be merged into generic `SI2.1 Notify user` with a specific template and event type? | Open. The BPMN models this as a distinct interaction, while its steps resemble the generic notification interaction. |
