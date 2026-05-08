# SI4.1 Create backoffice review task

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn`.

After the system creates a pending maintenance plan, it creates a backoffice review task. The system validates the review subject, creates the task, assigns or queues it for review, and returns success. The BPMN then routes the review decision to either reject the maintenance plan or approve it for eSigning.

## Questions

- Is this interaction used only for maintenance plan review, or also for registration and maintenance slot confirmation tasks?
- What assignment rules decide which backoffice worker receives the task?
