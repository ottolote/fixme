# SI1.4 Set user preferences

## Flow

Source: `BP1.1-onboard.bpmn`.

The user sets initial profile details after entering a password. The initial required fields are name, email, phone number, and location. Location is used later to match customers with nearby maintenance providers. The system receives the profile update request from the client browser, validates the submitted profile data in separate checks, loads the user profile, stores the details, and returns success.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive preferences update request | Receives the submitted profile/preference data from the client browser. |
| 2 | Check whether name is valid | Validates the submitted name. |
| 2a | Return invalid-name failure | Returns an error when the name is missing or invalid. |
| 3 | Check whether email is valid | Validates the submitted email. |
| 3a | Return invalid-email failure | Returns an error when the email is missing or invalid. |
| 4 | Check whether phone number is valid | Validates the submitted phone number. |
| 4a | Return invalid-phone-number failure | Returns an error when the phone number is missing or invalid. |
| 5 | Check whether location is valid | Validates the submitted customer location. |
| 5a | Return invalid-location failure | Returns an error when the location is missing or invalid. |
| 6 | Load user profile | Loads the profile to be updated. |
| 6a | Return profile-not-found failure | Returns an error when the user profile cannot be loaded. |
| 7 | Store name, email, phone number, and location | Persists the submitted profile details. |
| 8 | Produce business event: `User preferences set` | Publishes that the profile details were stored. |
| 9 | Return success | Returns a successful profile update response. |

## Business Events

Consumed:
- Preferences update request from the client browser after the user sets name, email, phone number, and location.

Produced:
- `User preferences set` after the profile data is stored.

## Questions / Answers

| Question | Answer |
|---|---|
| Are name, email, phone number, and location the complete initial profile set, or examples of a broader preferences model? | Answered. Keep the initial profile simple: name, email, phone number, and location. Do not add a broader preferences model until a concrete use case needs it. |
| Should location validation call an external address or geocoding service? | Answered. No external service initially. Validate that location is present and structurally usable for provider matching; provider-distance matching can become more precise later. |
