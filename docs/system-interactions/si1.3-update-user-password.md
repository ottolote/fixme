# SI1.3 Update user password

## Flow

Source: `BP1.1-onboard.bpmn`.

After confirming the email, the user enters a password. The system validates the request and password policy, loads the account, hashes and stores the password credential, and returns success.

## Business Events

Consumed:
- `User password submitted` from the user entering the password.

Produced:
- `User password updated` after the credential is stored.

## Questions

- Is this interaction only for first-time password setup, or also for later password changes?
- Which password policy, credential history, and session rules apply?
