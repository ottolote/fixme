# SI1.5 Create pending registration

## Flow

Source: `BP1.2-register-equipment.bpmn`.

The customer chooses an equipment type and submits the registration form. The system validates the customer and equipment data, creates a pending registration, and returns that registration to the customer. The BPMN then routes the pending registration to a backoffice worker for an accept or reject decision.

## Business Events

Consumed:
- `Equipment registration submitted` from the customer submitting the equipment registration form.

Produced:
- `Pending equipment registration created`, which starts the backoffice decision flow in `BP1.2-register-equipment.bpmn`.

## Questions

- What exact equipment attributes are required by equipment type?
- Should duplicate equipment registrations be rejected, merged, or routed for manual review?
