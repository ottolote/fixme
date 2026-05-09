# SI1.2 Confirm user email

## Flow

Source: `BP1.1-onboard.bpmn`.

The user clicks the activation link in the registration email. The system receives the email confirmation request from the client browser, validates the token in separate checks, loads the matching unconfirmed account, marks the email address as confirmed, invalidates the token, and returns success. If the token belongs to an already-confirmed account, repeated activation-link clicks also return success. The BPMN then continues with the user entering a password.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive email confirmation request | Receives the activation-link request from the client browser. |
| 2 | Check whether confirmation token exists | Looks up the submitted confirmation token. |
| 2a | Return invalid-token failure | Returns an error when the token is unknown. |
| 3 | Check whether confirmation token is expired | Verifies that the token is still within its validity period. |
| 3a | Return expired-token failure | Returns an error when the token has expired. |
| 4 | Check whether confirmation token is unused | Verifies that the token has not already been consumed. |
| 4a | Load already-confirmed account for token | Checks whether a used token belongs to an already-confirmed account. |
| 4b | Return used-token failure | Returns an error when the used token does not belong to an already-confirmed account. |
| 5 | Load unconfirmed user account | Loads the account associated with the token. |
| 5a | Load already-confirmed account for token | Checks whether the token belongs to an already-confirmed account. |
| 5b | Return account-not-found failure | Returns an error when the token does not resolve to an unconfirmed or already-confirmed account. |
| 6 | Mark email as confirmed | Updates the user account to record successful email confirmation. |
| 7 | Invalidate confirmation token | Prevents the confirmation token from being reused. |
| 8 | Produce business event: `User email confirmed` | Publishes that the email address has been confirmed. |
| 9 | Return success | Returns a successful confirmation response. |

## Business Events

Consumed:
- Email confirmation request from the client browser after the user clicks the activation link.

Produced:
- `User email confirmed` after the confirmation token is accepted and invalidated. Repeated activation-link clicks for an already-confirmed account return success without producing a new event.

## Questions / Answers

| Question | Answer |
|---|---|
| Should confirming an already-confirmed email be treated as success for idempotency? | Answered. Yes, if the token belongs to the already-confirmed account. Return success so repeated activation-link clicks are harmless. Unknown, expired, or unrelated tokens still return the specific failure. |
| What user-facing result is expected when the confirmation token is expired? | Answered. Return an expired-token failure, because the interaction already performs a separate expiry check. The UI should guide the user to request a new confirmation email. |
