# SI1.3 Update user password

## Flow

Source: `BP1.1-onboard.bpmn`.

After confirming the email, the user enters a password. The system validates the request and password policy, loads the account, hashes and stores the password credential, and returns success.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `User password submitted` | Receives the password submission. |
| 2 | Decide whether the request and password are valid | Validates the request context and password policy. |
| 3 | Load user account | Loads the user account that will receive the credential update. |
| 4 | Hash password | Converts the submitted password into a secure password hash. |
| 5 | Store password credential | Persists the hashed credential for the account. |
| 6 | Produce business event: `User password updated` | Publishes that the account password was updated. |
| 7 | Return success | Returns a successful password update response. |
| 8 | Return failure | Returns an error when the request or password fails validation. |

## Business Events

Consumed:
- `User password submitted` from the user entering the password.

Produced:
- `User password updated` after the credential is stored.

## Questions / Answers

| Question | Answer |
|---|---|
| Is this interaction only for first-time password setup, or also for later password changes? | Open. The BPMN context is onboarding, but the interaction name is generic. |
| Which password policy, credential history, and session rules apply? | Open. The diagram validates the password but does not specify policy details. |
