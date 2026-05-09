# SI1.5 Create pending registration

## Flow

Source: `BP1.2-register-equipment.bpmn`.

The customer chooses an equipment type and submits the registration form. The system receives the equipment registration request from the client browser, validates the customer and equipment data in separate checks, creates a pending registration, and returns that registration to the customer. The BPMN then routes the pending registration to a backoffice worker for an accept or reject decision.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Receive equipment registration request | Receives the submitted equipment registration form from the client browser. |
| 2 | Check whether customer may register equipment | Validates the customer identity and permission to register equipment. |
| 2a | Return invalid-customer failure | Returns an error when the customer cannot register equipment. |
| 3 | Check whether equipment type is supported | Validates that the selected equipment type can be registered. |
| 3a | Return unsupported-equipment-type failure | Returns an error when the selected equipment type is unsupported. |
| 4 | Check whether equipment data is valid for type | Validates required equipment attributes for the selected type. |
| 4a | Return invalid-equipment-data failure | Returns an error when required equipment data is missing or invalid. |
| 5 | Create pending equipment registration | Persists the equipment registration in a pending review state. |
| 6 | Produce business event: `Pending equipment registration created` | Publishes that a pending equipment registration exists for review. |
| 7 | Return pending registration | Returns the created pending registration to the caller. |

## Business Events

Consumed:
- Equipment registration request from the client browser after the customer submits the equipment registration form.

Produced:
- `Pending equipment registration created`, which starts the backoffice decision flow in `BP1.2-register-equipment.bpmn`.

## Questions / Answers

| Question | Answer |
|---|---|
| What exact equipment attributes are required by equipment type? | Open. The diagram validates equipment data but does not define type-specific required fields. |
| Should duplicate equipment registrations be rejected, merged, or routed for manual review? | Open. The diagram has a generic validation failure path but no duplicate-handling rule. |
