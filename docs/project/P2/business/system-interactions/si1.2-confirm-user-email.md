# SI1.2 Confirm user email

## Flow

Source: `BP1.1-onboard.bpmn`.

The user clicks the activation link in the registration email. The system receives the email confirmation request from the client browser, validates the token in separate checks, loads the matching unconfirmed account, marks the email address as confirmed, invalidates the token, and returns success. If the token belongs to an already-confirmed account, repeated activation-link clicks also return success even when the token is already used or expired. The BPMN then continues with the user entering a password.

Email confirmation is unauthenticated, so failed confirmation responses must not reveal whether a token, account, or email address exists. Unknown, expired, unrelated, and already-used tokens that do not belong to an already-confirmed account return the same generic confirmation failure.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive email confirmation request | Receives the activation-link request from the client browser. |
| 2 | Check whether confirmation token exists | Looks up the submitted confirmation token. |
| 2a | Return confirmation failure | Returns the generic confirmation failure when the token is unknown. |
| 3 | Check whether confirmation token belongs to already-confirmed account | Supports idempotent repeated activation-link clicks before applying expiry or used-token failures. |
| 4 | Check whether confirmation token is expired | Verifies that the token is still within its validity period. |
| 4a | Return confirmation failure | Returns the generic confirmation failure when the token has expired and does not belong to an already-confirmed account. |
| 5 | Check whether confirmation token is unused | Verifies that the token has not already been consumed. |
| 5a | Return confirmation failure | Returns the generic confirmation failure when a used token does not belong to an already-confirmed account. |
| 6 | Load unconfirmed user account | Loads the account associated with the token. |
| 6a | Return confirmation failure | Returns the generic confirmation failure when the token does not resolve to an unconfirmed account and does not belong to an already-confirmed account. |
| 7 | Mark email as confirmed | Updates the user account to record successful email confirmation. |
| 8 | Invalidate confirmation token | Prevents the confirmation token from being reused. |
| 9 | Produce business event: `User email confirmed` | Publishes that the email address has been confirmed. |
| 10 | Return success | Returns a successful confirmation response. |

## Business Events

Consumed:
- Email confirmation request from the client browser after the user clicks the activation link.

Produced:
- `User email confirmed` after the confirmation token is accepted and invalidated. Repeated activation-link clicks for an already-confirmed account return success without producing a new event.

## Questions / Answers

| Question | Answer |
|---|---|
| Should confirming an already-confirmed email be treated as success for idempotency? | Answered. Yes, if the token belongs to the already-confirmed account. Check this before token expiry or used-token failures so repeated activation-link clicks are harmless. |
| What user-facing result is expected when the confirmation token is expired? | Answered. Return the generic confirmation failure unless the token belongs to an already-confirmed account. The UI should guide the user to request a new confirmation email without revealing whether any account exists. |
| How does email confirmation prevent unauthenticated account enumeration? | Answered. Use one generic confirmation failure for unknown, expired, unrelated, and already-used tokens that are not idempotent already-confirmed clicks. Do not expose account-not-found, invalid-token, used-token, or expired-token as distinct unauthenticated responses. |
