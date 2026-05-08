# SI1.8 Reject registration

## Flow

Source: `BP1.2-register-equipment.bpmn`.

A backoffice worker decides not to accept the pending equipment registration. The system receives the reject command, validates that the registration is still pending and rejectable, marks it as rejected, stores any rejection reason, and returns success.

## Questions

- The BPMN has no customer notification after registration rejection. Should there be a `Notify customer of rejected registration` interaction?
- Is a rejection reason required, optional, or not supported?
