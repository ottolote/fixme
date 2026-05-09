# SI3.1 Match maintenance plan offering

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn`.

The user selects already-registered equipment and chooses to enroll. The system receives the enrollment request from the client browser, validates registration and eligibility in separate checks, loads the equipment details, finds matching maintenance plan offerings, and returns available options with pricing when a match exists.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive maintenance plan enrollment request | Receives the enrollment request from the client browser. |
| 2 | Check whether equipment exists | Validates that the selected equipment can be found. |
| 2a | Return equipment-not-found failure | Returns an error when the selected equipment does not exist. |
| 3 | Check whether equipment is registered | Validates that the equipment is registered. |
| 3a | Return equipment-not-registered failure | Returns an error when the equipment is not registered. |
| 4 | Check whether equipment is eligible for maintenance plan | Validates that the equipment can receive a maintenance plan. |
| 4a | Return equipment-ineligible failure | Returns an error when the equipment is not eligible. |
| 5 | Load equipment details | Loads equipment attributes needed for matching and pricing. |
| 6 | Find matching maintenance plan offerings | Searches available plan offerings for matches. |
| 7 | Check whether matches were found | Determines whether any offerings can be presented. |
| 7a | Return no match | Returns that no suitable offering was found. |
| 8 | Produce business event: `Maintenance plan options matched` | Publishes that matching plan options are available. |
| 9 | Return options with pricing | Returns matching plans and prices to the caller. |

## Business Events

Consumed:
- Maintenance plan enrollment request from the client browser after the user selects registered equipment and chooses to enroll.

Produced:
- `Maintenance plan options matched` when matching offerings are available.

## Questions / Answers

| Question | Answer |
|---|---|
| The BPMN only shows the `Match? yes` path. What should the system return when no maintenance plan offering matches? | Answered. Return a successful no-match result with no options, not a system failure. The customer cannot continue to plan selection until at least one matching offering exists. |
| Which equipment attributes drive matching and pricing? | Open. The diagram loads equipment details but does not list matching or pricing inputs. |
