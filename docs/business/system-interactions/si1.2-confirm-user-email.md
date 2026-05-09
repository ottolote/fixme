# SI1.2 Confirm user email

## Flow

Source: `BP1.1-onboard.bpmn`.

The user clicks the activation link in the registration email. The system receives the confirmation request, validates the token, loads the matching unconfirmed account, marks the email address as confirmed, invalidates the token, and returns success. The BPMN then continues with the user entering a password.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `Email confirmation requested` | Receives the activation-link request. |
| 2 | Decide whether the confirmation token is valid | Checks that the token exists, has not expired, and can still be used. |
| 3 | Load unconfirmed user account | Loads the account associated with the token. |
| 4 | Mark email as confirmed | Updates the user account to record successful email confirmation. |
| 5 | Invalidate confirmation token | Prevents the confirmation token from being reused. |
| 6 | Produce business event: `User email confirmed` | Publishes that the email address has been confirmed. |
| 7 | Return success | Returns a successful confirmation response. |
| 8 | Return failure | Returns an error when the token is invalid. |

## Business Events

Consumed:
- `Email confirmation requested` from the user clicking the activation link.

Produced:
- `User email confirmed` after the confirmation token is accepted and invalidated.

## Questions / Answers

| Question | Answer |
|---|---|
| Should confirming an already-confirmed email be treated as success for idempotency? | Open. The current flow only distinguishes valid and invalid tokens. |
| What user-facing result is expected when the confirmation token is expired? | Open. The diagram returns failure for invalid tokens but does not distinguish expiry. |
