# SI2.4 Notify customer of accepted registration

## Flow

Source: `BP1.2-register-equipment.bpmn`.

After a backoffice worker accepts a pending equipment registration, the system notifies the customer. It validates the accepted-registration event, loads the customer and registration, renders the notification, sends it, and returns success. The BPMN then shows the customer receiving the accepted registration.

## Questions

- Should the notification include details of the registered equipment and next available actions?
- Should an accepted registration notification be sent if the customer is already viewing the result in-app?
