# SI1.6 Submit registration

## Flow

Source: `BP1.2-register-equipment.bpmn`.

The customer submits equipment details for an existing draft registration. The system receives the submission request from the client browser, validates the request shape and required non-business data, loads the draft registration, checks the registration policy through `PolicyEngine.ValidateEquipmentRegistration()`, and then either creates registered equipment immediately, marks the registration as pending for review, or returns a registration failure.

The policy engine encapsulates the different validation strategies for e-bikes, boats, cars, lawnmowers, and other equipment. All types must have at least one maintenance service provider for that equipment type; if none exist, the policy result is `invalid`. Cars and e-bikes also require official registration validation using the license number. Cars, e-bikes, and lawnmowers normally return `valid` and create the registered equipment directly without marking the registration as pending. Equipment that requires manual handling returns `needs review`, is marked pending, and produces the generic review event that creates a backoffice task. A backoffice worker later resolves the pending registration through `SI1.7 Resolve pending registration`.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive registration submission request | Receives the submitted equipment details from the client browser. |
| 2 | Validate request | Checks non-business validation such as required request fields, malformed data, and identifiers needed to load the draft. |
| 2a | Return invalid-request failure | Returns an error when the request is incomplete or malformed. |
| 3 | Load draft registration | Loads the draft registration being submitted. |
| 3a | Return draft-registration-not-found failure | Returns an error when the referenced draft registration does not exist. |
| 4 | Check policy engine: `PolicyEngine.ValidateEquipmentRegistration()` | Evaluates the equipment registration business policy for the selected type and returns `valid`, `needs review`, or `invalid`. |
| 5 | Create registered equipment | For `valid`, creates the registered equipment directly. |
| 6 | Mark registration as accepted | For `valid`, marks the draft registration accepted after direct equipment creation. |
| 7 | Produce business event: `Equipment registration accepted` | For `valid`, publishes that equipment was registered without manual review. |
| 8 | Mark draft registration as pending | For `needs review`, updates the draft registration to a pending submitted state. |
| 9 | Produce business event: `Equipment registration review requested` | For `needs review`, publishes that manual registration review is required. |
| 10 | Return registration failure | For `invalid`, returns the policy failure reason to the caller. |
| 11 | Return registration result | Returns the accepted registration or pending registration to the caller. |

## Business Events

Consumed:
- Registration submission request from the client browser after the customer submits equipment details for a draft registration.

Produced:
- `Equipment registration accepted` when the policy engine returns `valid` and the equipment is registered directly.
- `Equipment registration review requested` when the policy engine returns `needs review`, followed by `SI4.1 Create backoffice task` for equipment registration review.

## Questions / Answers

| Question | Answer |
|---|---|
| Which component decides whether registration is valid, needs review, or invalid? | Answered. `PolicyEngine.ValidateEquipmentRegistration()` evaluates the equipment registration and returns `valid`, `needs review`, or `invalid`. |
| Where are non-business request checks handled? | Answered. `Validate request` handles non-business checks such as incomplete or malformed request data before the draft registration is loaded. |
| Where are ownership/security checks handled? | Answered. Security and ownership are assumed to be built into the platform and are not modeled in this system interaction. |
| Which equipment types have explicit validation strategies? | Answered. The policy engine uses separate validation strategies for e-bikes, boats, cars, lawnmowers, and other equipment. |
| What provider availability is required before submission can proceed? | Answered. Every equipment type must have at least one service provider that performs maintenance for that type; otherwise the policy engine returns `invalid`. |
| Which types require official registration validation? | Answered. Cars and e-bikes require official registration validation by license number in the policy engine. |
| What special handling applies to boats? | Answered. There is no explicit boat branch in this interaction. If the policy engine returns `needs review`, the generic equipment-registration-review backoffice task is created and a backoffice worker resolves the pending registration through `SI1.7 Resolve pending registration`. |
| Which policy result registers equipment without pending review? | Answered. `valid` creates the registered equipment directly without marking the registration pending. |
| Which policy result remains pending after submission? | Answered. `needs review` marks the registration pending and emits a review event. |
