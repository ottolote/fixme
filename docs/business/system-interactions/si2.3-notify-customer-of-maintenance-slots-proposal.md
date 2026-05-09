# SI2.3 Notify customer of maintenance slots proposal

## Flow

Source: `BP1.4-schedule-maintenance-job.bpmn`.

After the backoffice worker confirms the maintenance slots proposal, the system notifies the customer that the proposal is available. The system validates that the proposal can be sent, loads the proposal, resolves the customer notification channel, renders the proposal notification, sends it, and returns success or failure.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `Maintenance slots proposal confirmed` | Receives the event that the proposal is ready for customer notification. |
| 2 | Decide whether proposal notification can be sent | Validates proposal state, customer contactability, and notification prerequisites. |
| 3 | Load maintenance slots proposal | Loads proposal details and proposed slot data for the notification. |
| 4 | Resolve customer notification channel | Determines the customer's delivery channel. |
| 5 | Render maintenance slots proposal notification | Builds the customer-facing proposal message. |
| 6 | Send notification to customer | Delivers the proposal notification to the customer. |
| 7 | Produce business event: `Maintenance slots proposal notification sent` | Publishes that the customer notification was sent. |
| 8 | Return success | Returns a successful notification response. |
| 9 | Return failure | Returns an error when the notification cannot be sent. |

## Business Events

Consumed:
- `Maintenance slots proposal confirmed` from the backoffice worker confirming the proposal.

Produced:
- `Maintenance slots proposal notification sent`, which the customer receives before selecting a slot.

## Questions / Answers

| Question | Answer |
|---|---|
| Should this stay as a dedicated `SI2.3`, or should it be merged into generic `SI2.1 Notify user` with a specific template and event type? | Open. The BPMN models this as a distinct interaction, while its steps resemble the generic notification interaction. |
