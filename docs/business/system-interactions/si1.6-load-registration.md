# SI1.6 Load registration

## Flow

Source: `system-interactions.md`; no direct call activity found in the BPMN files.

This interaction is modeled as a registration lookup. The system receives a load request, validates that the request contains a registration identifier, checks that the requester is authorized to access the registration, loads the registration by identifier, and returns either registration details or a not-found response.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive load registration request | Receives a request to retrieve a registration by identifier. |
| 2 | Check whether request contains registration identifier | Validates the lookup request shape. |
| 2a | Return invalid-request failure | Returns an error when the request is missing a registration identifier. |
| 3 | Check whether requester may access registration | Authorizes the requester for the target registration. |
| 3a | Return unauthorized failure | Returns an authorization failure when access is denied. |
| 4 | Load registration by identifier | Looks up the registration record. |
| 5 | Check whether registration exists | Determines whether the lookup found a registration. |
| 5a | Return not found | Returns a not-found response when no registration exists for the identifier. |
| 6 | Return registration details | Returns the registration data when found and authorized. |

## Business Events

Consumed:
- None found in the BPMN files.

Produced:
- None found in the BPMN files.

## Questions / Answers

| Question | Answer |
|---|---|
| Which business process should invoke `SI1.6 Load registration`? | Open. No direct call activity for this interaction was found in the BPMN files. |
| Is this intended for the customer, the backoffice worker, or both? | Open. The diagram only models requester authorization, not requester roles. |
| Should it return pending, accepted, and rejected registrations, or only pending registrations? | Open. The diagram returns registration details but does not restrict registration states. |
