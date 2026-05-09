# SI1.1 Register account

## Flow

Source: `BP1.1-onboard.bpmn`.

The user enters an email address and clicks **Register**. The system receives the registration request from the client browser, validates the email format, and checks whether a user already exists for that email. If the user is already confirmed, the system returns `user-exists`. If the user is unconfirmed, the system refreshes the confirmation token and accepts the registration request. Otherwise, it creates an unconfirmed user account, creates a 24-hour email confirmation token, and returns that registration was accepted. The BPMN then continues with `SI2.1 Notify user` to send the activation email.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive registration request | Receives the registration request from the client browser. |
| 2 | Check whether email is valid | Validates the submitted email format. |
| 2a | Return invalid-email failure | Returns a validation error when the submitted email is invalid. |
| 3 | Check whether user exists | Looks up whether an account already exists for the submitted email. |
| 3a | Check whether user is unconfirmed | Determines whether an existing unconfirmed registration can be refreshed. |
| 3b | Return user-exists failure | Returns an error when a confirmed account already exists for the submitted email. |
| 4 | Invalidate previous unused confirmation token | Invalidates the previous token when refreshing an unconfirmed registration. |
| 5 | Create user account in unconfirmed state | Persists a new user account that cannot be used until email confirmation succeeds. |
| 6 | Create 24-hour email confirmation token | Generates and stores the token used by the activation link. |
| 7 | Produce business event: `preliminaryUserCreated` | Publishes that the preliminary account and confirmation token were created or refreshed. |
| 8 | Return registration accepted | Returns a successful registration response to the caller. |

## Business Events

Consumed:
- Registration request from the client browser after the user clicks **Register**.

Produced:
- `preliminaryUserCreated` after the unconfirmed account and confirmation token are created or after an unconfirmed registration token is refreshed.

## Questions / Answers

| Question | Answer |
|---|---|
| Should duplicate registration attempts be idempotent, rejected, or should they resend the activation notification? | Answered. If the account is already confirmed, return `user-exists`. If the account is still unconfirmed, treat the request as idempotent, refresh the confirmation token, and resend the activation notification. |
| What expiry and retry policy applies to the email confirmation token? | Answered. Confirmation tokens expire after 24 hours. A resend request invalidates the previous unused token and creates a new 24-hour token. Limit resend attempts with a simple rate limit to prevent abuse. |
