# SI1.5 Create draft registration

## Flow

Source: `BP1.2-register-equipment.bpmn`.

The customer clicks "register equipment" before submitting equipment details. The system receives the draft registration request from the client browser, validates that the customer may start a registration, creates a draft registration, produces the same business event as before, and returns the draft registration to the customer.

This interaction does not create or directly trigger a backoffice task. Any tasking remains decoupled through business events and later submission flow.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive draft registration request | Receives the request to start an equipment registration from the client browser. |
| 2 | Check whether customer may register equipment | Validates the customer identity and permission to register equipment. |
| 2a | Return invalid-customer failure | Returns an error when the customer cannot register equipment. |
| 3 | Create draft equipment registration | Persists the equipment registration in a draft state. |
| 4 | Produce business event: `Pending equipment registration created` | Publishes the existing registration-created business event for downstream consumers. |
| 5 | Return draft registration | Returns the created draft registration to the caller. |

## Business Events

Consumed:
- Draft registration request from the client browser after the customer clicks "register equipment".

Produced:
- `Pending equipment registration created` when the draft registration is created. This interaction does not directly invoke `SI4.1 Create backoffice task`.

## Questions / Answers

| Question | Answer |
|---|---|
| What exact equipment attributes are required by equipment type? | Deferred to `SI1.6 Submit registration`. Data models and type-specific equipment fields will be defined separately. |
| Should duplicate equipment registrations be rejected, merged, or routed for manual review? | Answered. Do not handle duplicates for now. Allow customers to submit duplicate equipment registrations; cleanup or correction is the customer's responsibility. |
