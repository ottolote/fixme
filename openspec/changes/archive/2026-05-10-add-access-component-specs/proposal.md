## Why

The P2 business documentation and methodsketch define access components and DTO-level calls, but the OpenSpec access component specs are currently empty. Adding these requirements makes each access component's persistence boundary explicit and traceable to the documented system interactions.

## What Changes

- Add OpenSpec requirements for every access component used by `docs/methodsketch.txt`: `CustomerAccess`, `EquipmentAccess`, `MaintenanceAccess`, `TaskAccess`, `NotificationAccess`, `AgreementAccess`, and `ESigningAccess`.
- Specify each supported access operation using the DTO/entity names from methodsketch, including criteria DTOs such as `CustomerCriteria`, `EquipmentCriteria`, `MaintenancePlanCriteria`, `TaskCriteria`, `NotificationTemplateCriteria`, and `AgreementCriteria`.
- Capture expected filter/store behavior, resource delegation, not-found/empty-result behavior, idempotent update expectations, and external eSigning request handling.
- No implementation code changes are planned; this change only updates OpenSpec documentation.

## Capabilities

### New Capabilities


### Modified Capabilities

- `customer-access`: Add requirements for `Filter(CustomerCriteria): Customer` and `Store(Customer): Customer` against `CustomerResource`.
- `equipment-access`: Add requirements for `Filter(EquipmentCriteria): Equipment`, `Filter(EquipmentTypeCriteria): EquipmentType`, `Filter(PendingRegistrationCriteria): PendingRegistration`, `Store(PendingRegistration): PendingRegistration`, and `Store(Equipment): Equipment` against `EquipmentResource`.
- `maintenance-access`: Add requirements for maintenance offering, plan, job slot, job slot proposal, and job filter/store operations against `MaintenanceResource`.
- `task-access`: Add requirements for `Filter(TaskCriteria): Task` and `Store(Task): Task` against `TaskResource`.
- `notification-access`: Add requirements for `NotificationTemplateFilter(NotificationTemplateCriteria): NotificationTemplate` and `Store(Notification): Notification` against `NotificationResource`.
- `agreement-access`: Add requirements for `Filter(AgreementCriteria): Agreement` and `Store(Agreement): Agreement` against `AgreementResource`.
- `e-signing-access`: Add requirements for `SignatureRequestStore(SignatureRequest): SignatureRequest` against `ExternalESigningResource`.

## Impact

- Affects OpenSpec documentation under `openspec/specs/*-access/` only.
- Aligns access component contracts with P2 documentation in `docs/project/P2/` and `docs/methodsketch.txt`.
- No runtime APIs, code, dependencies, database schemas, or generated artifacts are changed by this proposal.
