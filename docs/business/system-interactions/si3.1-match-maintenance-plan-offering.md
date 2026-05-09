# SI3.1 Match maintenance plan offering

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn`.

The user selects already-registered equipment and chooses to enroll. The system validates equipment eligibility, loads the equipment details, finds matching maintenance plan offerings, and returns available options with pricing when a match exists.

## Business Events

Consumed:
- `Maintenance plan enrollment requested` from the user selecting registered equipment and choosing to enroll.

Produced:
- `Maintenance plan options matched` when matching offerings are available.

## Questions

- The BPMN only shows the `Match? yes` path. What should the system return when no maintenance plan offering matches?
- Which equipment attributes drive matching and pricing?
