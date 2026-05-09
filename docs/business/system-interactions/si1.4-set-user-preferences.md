# SI1.4 Set user preferences

## Flow

Source: `BP1.1-onboard.bpmn`.

The user sets preferences after entering a password. The BPMN names address and name specifically. The system validates the submitted profile data, loads the user profile, stores the preferences, and returns success.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `User preferences submitted` | Receives the user's submitted profile/preference data. |
| 2 | Decide whether preferences are valid | Validates the submitted name, address, and preference values. |
| 3 | Load user profile | Loads the profile to be updated. |
| 4 | Store name and address preferences | Persists the submitted name and address preferences. |
| 5 | Produce business event: `User preferences set` | Publishes that the preferences were stored. |
| 6 | Return success | Returns a successful preference update response. |
| 7 | Return failure | Returns an error when preference validation fails. |

## Business Events

Consumed:
- `User preferences submitted` from the user setting address and name.

Produced:
- `User preferences set` after the profile data is stored.

## Questions / Answers

| Question | Answer |
|---|---|
| Are name and address the complete preference set, or examples of a broader profile/preferences model? | Open. The BPMN explicitly names address and name, while the interaction uses the broader term preferences. |
| Should address validation call an external address service? | Open. The diagram validates preferences but does not define validation mechanisms. |
