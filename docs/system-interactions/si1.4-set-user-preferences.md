# SI1.4 Set user preferences

## Flow

Source: `BP1.1-onboard.bpmn`.

The user sets preferences after entering a password. The BPMN names address and name specifically. The system validates the submitted profile data, loads the user profile, stores the preferences, and returns success.

## Business Events

Consumed:
- `User preferences submitted` from the user setting address and name.

Produced:
- `User preferences set` after the profile data is stored.

## Questions

- Are name and address the complete preference set, or examples of a broader profile/preferences model?
- Should address validation call an external address service?
