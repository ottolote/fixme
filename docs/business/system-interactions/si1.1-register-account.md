# SI1.1 Register account

## Flow

Source: `BP1.1-onboard.bpmn`.

The user enters an email address and clicks **Register**. The system receives the registration request from the client browser, validates the email format, checks whether a user already exists for that email, creates an unconfirmed user account, creates an email confirmation token, and returns that registration was accepted. The BPMN then continues with `SI2.1 Notify user` to send the activation email.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive registration request | Receives the registration request from the client browser. |
| 2 | Check whether email is valid | Validates the submitted email format. |
| 2a | Return invalid-email failure | Returns a validation error when the submitted email is invalid. |
| 3 | Check whether user exists | Looks up whether an account already exists for the submitted email. |
| 3a | Return user-exists failure | Returns an error when an account already exists for the submitted email. |
| 4 | Create user account in unconfirmed state | Persists a new user account that cannot be used until email confirmation succeeds. |
| 5 | Create email confirmation token | Generates and stores the token used by the activation link. |
| 6 | Produce business event: `preliminaryUserCreated` | Publishes that the preliminary account and confirmation token were created. |
| 7 | Return registration accepted | Returns a successful registration response to the caller. |

## Business Events

Consumed:
- Registration request from the client browser after the user clicks **Register**.

Produced:
- `preliminaryUserCreated` after the unconfirmed account and confirmation token are created.

## Questions / Answers

| Question | Answer |
|---|---|
| Should duplicate registration attempts be idempotent, rejected, or should they resend the activation notification? | Open. The current flow only models the validation failure path for already-registered accounts. |
| What expiry and retry policy applies to the email confirmation token? | Open. The diagram creates the token but does not define expiry, retry, or resend behavior. |
