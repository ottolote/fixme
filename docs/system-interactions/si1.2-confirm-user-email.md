# SI1.2 Confirm user email

## Flow

Source: `BP1.1-onboard.bpmn`.

The user clicks the activation link in the registration email. The system receives the confirmation request, validates the token, loads the matching unconfirmed account, marks the email address as confirmed, invalidates the token, and returns success. The BPMN then continues with the user entering a password.

## Questions

- Should confirming an already-confirmed email be treated as success for idempotency?
- What user-facing result is expected when the confirmation token is expired?
