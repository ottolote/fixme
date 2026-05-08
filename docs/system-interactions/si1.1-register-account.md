# SI1.1 Register account

## Flow

Source: `BP1.1-onboard.bpmn`.

The user enters an email address and clicks **Register**. The system receives the registration command, validates that the email can be used, creates an unconfirmed user account, creates an email confirmation token, and returns that registration was accepted. The BPMN then continues with `SI2.1 Notify user` to send the activation email.

## Questions

- Should duplicate registration attempts be idempotent, rejected, or should they resend the activation notification?
- What expiry and retry policy applies to the email confirmation token?
