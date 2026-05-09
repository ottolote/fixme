# SI3.1 Match maintenance plan offering

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn`.

The user selects already-registered equipment and chooses to enroll. The system validates equipment eligibility, loads the equipment details, finds matching maintenance plan offerings, and returns available options with pricing when a match exists.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `Maintenance plan enrollment requested` | Receives the user's request to enroll registered equipment in a plan. |
| 2 | Decide whether equipment is registered and eligible | Validates that the equipment can receive a maintenance plan. |
| 3 | Load equipment details | Loads equipment attributes needed for matching and pricing. |
| 4 | Find matching maintenance plan offerings | Searches available plan offerings for matches. |
| 5 | Decide whether matches were found | Determines whether any offerings can be presented. |
| 6 | Produce business event: `Maintenance plan options matched` | Publishes that matching plan options are available. |
| 7 | Return options with pricing | Returns matching plans and prices to the caller. |
| 8 | Return no match | Returns that no suitable offering was found. |
| 9 | Return failure | Returns an error when the equipment is not registered or eligible. |

## Business Events

Consumed:
- `Maintenance plan enrollment requested` from the user selecting registered equipment and choosing to enroll.

Produced:
- `Maintenance plan options matched` when matching offerings are available.

## Questions / Answers

| Question | Answer |
|---|---|
| The BPMN only shows the `Match? yes` path. What should the system return when no maintenance plan offering matches? | Open. The PlantUML adds a no-match return path, but the BPMN does not show the downstream behavior. |
| Which equipment attributes drive matching and pricing? | Open. The diagram loads equipment details but does not list matching or pricing inputs. |
