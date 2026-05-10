## ADDED Requirements

### Requirement: Task filtering
`TaskAccess` SHALL expose `Filter(TaskCriteria): Task` for retrieving backoffice tasks and detecting duplicate open tasks. The access component SHALL delegate reads to `TaskResource` and SHALL return tasks matching the supplied `TaskCriteria`.

#### Scenario: Matching task is returned
- **WHEN** `Filter(TaskCriteria)` is called with criteria matching an existing backoffice task by subject, task type, status, or task identifier
- **THEN** `TaskAccess` returns the matching `Task` from `TaskResource`

#### Scenario: Task is not found
- **WHEN** `Filter(TaskCriteria)` is called with criteria matching no task
- **THEN** `TaskAccess` returns no `Task` result

### Requirement: Task storage
`TaskAccess` SHALL expose `Store(Task): Task` for creating and updating backoffice tasks used by `CreateBackofficeTask(CreateBackofficeTaskRequest)` and maintenance slot proposal confirmation. The access component SHALL delegate writes to `TaskResource` and SHALL return the persisted `Task` state.

#### Scenario: Task is stored
- **WHEN** `Store(Task)` is called with a valid task type, subject reference, routing target, and status
- **THEN** `TaskAccess` persists the `Task` through `TaskResource` and returns the stored `Task`

#### Scenario: Task cannot be stored
- **WHEN** `Store(Task)` is called with missing task type, subject reference, routing target, or invalid status transition data
- **THEN** `TaskAccess` rejects the store operation and does not persist a partial `Task`
