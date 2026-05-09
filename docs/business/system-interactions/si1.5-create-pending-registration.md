# SI1.5 Create pending registration

## Flow

Source: `BP1.2-register-equipment.bpmn`.

The customer chooses an equipment type and submits the registration form. The system validates the customer and equipment data, creates a pending registration, and returns that registration to the customer. The BPMN then routes the pending registration to a backoffice worker for an accept or reject decision.

## Steps

| Step | PlantUML step | Actions performed |
|---|---|---|
| 1 | Consume business event: `Equipment registration submitted` | Receives the submitted equipment registration form. |
| 2 | Decide whether customer and equipment data are valid | Validates the customer identity and required equipment registration data. |
| 3 | Create pending equipment registration | Persists the equipment registration in a pending review state. |
| 4 | Produce business event: `Pending equipment registration created` | Publishes that a pending equipment registration exists for review. |
| 5 | Return pending registration | Returns the created pending registration to the caller. |
| 6 | Return failure | Returns an error when validation fails. |

## Business Events

Consumed:
- `Equipment registration submitted` from the customer submitting the equipment registration form.

Produced:
- `Pending equipment registration created`, which starts the backoffice decision flow in `BP1.2-register-equipment.bpmn`.

## Questions / Answers

| Question | Answer |
|---|---|
| What exact equipment attributes are required by equipment type? | Open. The diagram validates equipment data but does not define type-specific required fields. |
| Should duplicate equipment registrations be rejected, merged, or routed for manual review? | Open. The diagram has a generic validation failure path but no duplicate-handling rule. |
