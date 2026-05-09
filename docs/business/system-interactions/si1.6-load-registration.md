# SI1.6 Load registration

## Flow

Source: `system-interactions.md`; no direct call activity found in the BPMN files.

This interaction is modeled as a registration lookup. The system receives a load request, checks that the requester is authorized to access the registration, loads the registration by identifier, and returns either registration details or a not-found response.

## Business Events

Consumed:
- None found in the BPMN files.

Produced:
- None found in the BPMN files.

## Questions

- Which business process should invoke `SI1.6 Load registration`?
- Is this intended for the customer, the backoffice worker, or both?
- Should it return pending, accepted, and rejected registrations, or only pending registrations?
