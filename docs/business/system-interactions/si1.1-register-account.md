# SI1.1 Register account

## Flow

Source: `BP1.1-onboard.bpmn`.

The user enters an email address and clicks **Register**. The system receives the registration command, validates that the email can be used, creates an unconfirmed user account, creates an email confirmation token, and returns that registration was accepted. The BPMN then continues with `SI2.1 Notify user` to send the activation email.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `Register account requested` | Receives the registration request submitted by the user. |
| 2 | Decide whether the email is valid and the account is not already registered | Validates the email format and checks that no existing account blocks registration. |
| 3 | Create user account in unconfirmed state | Persists a new user account that cannot be used until email confirmation succeeds. |
| 4 | Create email confirmation token | Generates and stores the token used by the activation link. |
| 5 | Produce business event: `User account registered pending email confirmation` | Publishes that the account is waiting for email confirmation. |
| 6 | Return registration accepted | Returns a successful registration response to the caller. |
| 7 | Return failure | Returns an error when validation fails or the account is already registered. |

## Business Events

Consumed:
- `Register account requested` from the user clicking **Register**.

Produced:
- `User account registered pending email confirmation` after the unconfirmed account and confirmation token are created.

## Questions / Answers

| Question | Answer |
|---|---|
| Should duplicate registration attempts be idempotent, rejected, or should they resend the activation notification? | Open. The current flow only models the validation failure path for already-registered accounts. |
| What expiry and retry policy applies to the email confirmation token? | Open. The diagram creates the token but does not define expiry, retry, or resend behavior. |
