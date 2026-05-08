# SI2.1 Notify user

## Flow

Source: `BP1.1-onboard.bpmn`.

After `SI1.1 Register account`, the system notifies the user. In the onboarding BPMN this notification is the email containing the activation link. The system validates the notification request, resolves the recipient and channel, renders the message content, sends it, and returns success.

## Questions

- Is `SI2.1 Notify user` a generic notification primitive used by all specialized notification interactions, or only the onboarding activation email?
- Should failed notification delivery fail the registration flow or be retried asynchronously?
