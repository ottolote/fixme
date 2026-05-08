# SI2.3 Notify customer of maintenance slots proposal

## Flow

Source: `BP1.4-schedule-maintenance-job.bpmn`.

After a backoffice worker confirms the maintenance slot proposal, the system notifies the customer. It validates the proposal, loads the customer and proposed slots, renders the notification, sends it, and returns success. The BPMN then shows the customer receiving the maintenance slots proposal and choosing whether to accept it.

## Questions

- How long should proposed slots remain reserved before expiring?
- Should the notification include every proposed slot or only a link to view them?
