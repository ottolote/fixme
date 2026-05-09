# SI1.4 Set user preferences

## Flow

Source: `BP1.1-onboard.bpmn`.

The user sets preferences after entering a password. The BPMN names address and name specifically. The system receives the preferences update request from the client browser, validates the submitted profile data in separate checks, loads the user profile, stores the preferences, and returns success.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive preferences update request | Receives the submitted profile/preference data from the client browser. |
| 2 | Check whether name is valid | Validates the submitted name. |
| 2a | Return invalid-name failure | Returns an error when the name is missing or invalid. |
| 3 | Check whether address is valid | Validates the submitted address. |
| 3a | Return invalid-address failure | Returns an error when the address is missing or invalid. |
| 4 | Check whether preference values are valid | Validates any additional preference values supplied with the request. |
| 4a | Return invalid-preferences failure | Returns an error when additional preference values are invalid. |
| 5 | Load user profile | Loads the profile to be updated. |
| 5a | Return profile-not-found failure | Returns an error when the user profile cannot be loaded. |
| 6 | Store name and address preferences | Persists the submitted name and address preferences. |
| 7 | Produce business event: `User preferences set` | Publishes that the preferences were stored. |
| 8 | Return success | Returns a successful preference update response. |

## Business Events

Consumed:
- Preferences update request from the client browser after the user sets address and name.

Produced:
- `User preferences set` after the profile data is stored.

## Questions / Answers

| Question | Answer |
|---|---|
| Are name and address the complete preference set, or examples of a broader profile/preferences model? | Open. The BPMN explicitly names address and name, while the interaction uses the broader term preferences. |
| Should address validation call an external address service? | Open. The diagram validates preferences but does not define validation mechanisms. |
