# SI1.3 Update user password

## Flow

Source: `BP1.1-onboard.bpmn`.

After confirming the email, the user enters a password. The system receives the password update request from the client browser, validates the request context, validates the password policy, loads the account, hashes and stores the password credential, and returns success.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive password update request | Receives the password submission from the client browser. |
| 2 | Check whether request context is valid | Validates that the password request is authorized for the target account. |
| 2a | Return invalid-request failure | Returns an error when the request context is invalid. |
| 3 | Check whether password satisfies policy | Validates the submitted password against the password policy. |
| 3a | Return password-policy failure | Returns an error when the password does not satisfy policy. |
| 4 | Load user account | Loads the user account that will receive the credential update. |
| 4a | Return account-not-found failure | Returns an error when the target account cannot be loaded. |
| 5 | Hash password | Converts the submitted password into a secure password hash. |
| 6 | Store password credential | Persists the hashed credential for the account. |
| 7 | Produce business event: `User password updated` | Publishes that the account password was updated. |
| 8 | Return success | Returns a successful password update response. |

## Business Events

Consumed:
- Password update request from the client browser after the user enters the password.

Produced:
- `User password updated` after the credential is stored.

## Questions / Answers

| Question | Answer |
|---|---|
| Is this interaction only for first-time password setup, or also for later password changes? | Open. The BPMN context is onboarding, but the interaction name is generic. |
| Which password policy, credential history, and session rules apply? | Open. The diagram validates the password but does not specify policy details. |
