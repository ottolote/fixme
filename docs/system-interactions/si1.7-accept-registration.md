# SI1.7 Accept registration

## Flow

Source: `BP1.2-register-equipment.bpmn`.

A backoffice worker decides to accept an equipment registration. The system receives the accept command, validates that the registration is still pending and can be accepted, creates or activates the registered equipment record, marks the registration as accepted, and returns success. The BPMN then continues with `SI2.4 Notify customer of accepted registration`.

## Questions

- Does accepting a registration create a new equipment record, update an existing pending equipment record, or both depending on state?
- Should acceptance record the backoffice worker and decision reason?
