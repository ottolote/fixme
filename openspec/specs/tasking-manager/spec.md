## Purpose

The `TaskingManager` owns creation and routing of backoffice tasks for P2 manager workflow events.

## Requirements

### Requirement: SI4.1 Create backoffice task
The `TaskingManager` SHALL expose `CreateBackofficeTask(CreateBackofficeTaskRequest): CreateBackofficeTaskResponse` for supported backoffice-work events. The manager SHALL validate the task type and referenced subject, check for duplicate open tasks with `TaskAccess.Filter(TaskCriteria)`, persist new tasks with `TaskAccess.Store(Task)`, and route tasks to the relevant provider or backoffice shared work pile without assigning an individual worker.

#### Scenario: Equipment registration review task is created
- **WHEN** `CreateBackofficeTask(CreateBackofficeTaskRequest)` processes `Equipment registration review requested` for an existing pending registration without an equivalent open task
- **THEN** the manager creates an `equipment_registration_review` task, routes it to the backoffice shared work pile, returns `CreateBackofficeTaskResponse` as successful, and records `Equipment registration review task created`

#### Scenario: Maintenance plan review task is created
- **WHEN** `CreateBackofficeTask(CreateBackofficeTaskRequest)` processes `Pending maintenance plan created` for an existing pending maintenance plan without an equivalent open task
- **THEN** the manager creates a `maintenance_plan_review` task, routes it to the backoffice shared work pile, returns `CreateBackofficeTaskResponse` as successful, and records `Maintenance plan review task created`

#### Scenario: Maintenance slots proposal confirmation task is created
- **WHEN** `CreateBackofficeTask(CreateBackofficeTaskRequest)` processes `Maintenance slots proposal created` for an existing proposal without an equivalent open task
- **THEN** the manager creates a `maintenance_slots_proposal_confirmation` task, routes it to the relevant provider or backoffice shared work pile, returns `CreateBackofficeTaskResponse` as successful, and records `Maintenance slots proposal confirmation task created`

#### Scenario: Maintenance provider cancellation task is created
- **WHEN** `CreateBackofficeTask(CreateBackofficeTaskRequest)` processes `Maintenance job cancelled` for an existing cancelled maintenance job without an equivalent open task
- **THEN** the manager creates a `maintenance_provider_cancellation_notification` task, routes it to the relevant provider or backoffice shared work pile, returns `CreateBackofficeTaskResponse` as successful, and records `Maintenance provider cancellation task created`

#### Scenario: Duplicate open task exists
- **WHEN** `CreateBackofficeTask(CreateBackofficeTaskRequest)` references a subject and task type that already has an open task
- **THEN** the manager rejects the event for dead-letter handling with `duplicate-task` and does not create another task

#### Scenario: Task event cannot be processed
- **WHEN** `CreateBackofficeTask(CreateBackofficeTaskRequest)` contains an unsupported task type or references a subject that does not exist
- **THEN** the manager rejects the event for dead-letter handling with `unsupported-task-type` or `subject-not-found`

### Requirement: Supported backoffice task types
The `TaskingManager` SHALL support the task type enum values documented for P2 manager workflows.

#### Scenario: Supported task type is received
- **WHEN** `CreateBackofficeTask(CreateBackofficeTaskRequest)` contains `equipment_registration_review`, `maintenance_plan_review`, `maintenance_slots_proposal_confirmation`, or `maintenance_provider_cancellation_notification`
- **THEN** the manager treats the task type as eligible for validation and task creation
