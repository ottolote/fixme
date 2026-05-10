## MODIFIED Requirements

### Requirement: SI1.5 Create draft registration
The `MembershipManager` SHALL create draft equipment registrations for customers permitted to register equipment. The manager SHALL expose `CreatePendingRegistration(CreatePendingRegistrationRequest)` for draft creation, SHALL validate customer eligibility with `CustomerAccess.Filter(CustomerCriteria)`, SHALL store draft `PendingRegistration` records with `EquipmentAccess.Store(PendingRegistration)`, and SHALL NOT directly create a backoffice task. The manager SHALL support creating drafts without equipment details or an equipment type. If the request includes an equipment type, the manager SHALL validate it with `EquipmentAccess.Filter(EquipmentTypeCriteria)` before storing the draft.

#### Scenario: Draft registration is created without equipment type
- **WHEN** a confirmed customer starts equipment registration without supplying an equipment type
- **THEN** the manager validates the customer, does not look up an equipment type, stores a draft or pending registration record using the equipment-registration persistence path, returns the draft registration, and records `Pending equipment registration created`

#### Scenario: Draft registration is created with valid equipment type
- **WHEN** a confirmed customer starts equipment registration with an equipment type that exists
- **THEN** the manager validates the customer, validates the equipment type, stores a draft or pending registration record using the equipment-registration persistence path, returns the draft registration, and records `Pending equipment registration created`

#### Scenario: Customer is invalid
- **WHEN** the draft registration request cannot be associated with a customer permitted to register equipment
- **THEN** the manager returns `invalid-customer` without storing a draft registration or validating equipment type

#### Scenario: Equipment type is invalid
- **WHEN** the draft registration request includes an equipment type that cannot be resolved
- **THEN** the manager returns `invalid-equipment-type` without storing a draft registration

#### Scenario: Draft creation does not create backoffice work
- **WHEN** a draft equipment registration is created
- **THEN** the manager does not call or enqueue `CreateBackofficeTask(CreateBackofficeTaskRequest): CreateBackofficeTaskResponse`
