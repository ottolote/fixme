## Purpose

The `EquipmentAccess` component owns equipment, equipment type, and pending registration persistence access and delegates reads and writes to `EquipmentResource`.
## Requirements
### Requirement: Equipment filtering
`EquipmentAccess` SHALL expose `Filter(EquipmentCriteria): Equipment` for registered equipment lookup operations. The access component SHALL delegate reads to `EquipmentResource` and SHALL return equipment records matching the supplied `EquipmentCriteria`.

#### Scenario: Matching equipment is returned
- **WHEN** `Filter(EquipmentCriteria)` is called with criteria matching existing registered equipment
- **THEN** `EquipmentAccess` returns the matching `Equipment` from `EquipmentResource`

#### Scenario: Equipment is not found
- **WHEN** `Filter(EquipmentCriteria)` is called with criteria matching no registered equipment
- **THEN** `EquipmentAccess` returns no `Equipment` result

### Requirement: Equipment type filtering
`EquipmentAccess` SHALL expose `Filter(EquipmentTypeCriteria): EquipmentType` for validating registration input against supported equipment types. The access component SHALL delegate reads to `EquipmentResource` and SHALL return equipment types matching the supplied `EquipmentTypeCriteria`.

#### Scenario: Matching equipment type is returned
- **WHEN** `Filter(EquipmentTypeCriteria)` is called with criteria matching a supported equipment type
- **THEN** `EquipmentAccess` returns the matching `EquipmentType` from `EquipmentResource`

#### Scenario: Equipment type is not found
- **WHEN** `Filter(EquipmentTypeCriteria)` is called with criteria matching no supported equipment type
- **THEN** `EquipmentAccess` returns no `EquipmentType` result

### Requirement: Pending registration filtering
`EquipmentAccess` SHALL expose `Filter(PendingRegistrationCriteria): PendingRegistration` for draft and pending equipment registration lookup operations. The access component SHALL delegate reads to `EquipmentResource` and SHALL return pending registration records matching the supplied `PendingRegistrationCriteria`.

#### Scenario: Matching pending registration is returned
- **WHEN** `Filter(PendingRegistrationCriteria)` is called with criteria matching an existing draft or pending registration
- **THEN** `EquipmentAccess` returns the matching `PendingRegistration` from `EquipmentResource`

#### Scenario: Pending registration is not found
- **WHEN** `Filter(PendingRegistrationCriteria)` is called with criteria matching no draft or pending registration
- **THEN** `EquipmentAccess` returns no `PendingRegistration` result

### Requirement: Pending registration storage
`EquipmentAccess` SHALL expose `Store(PendingRegistration): PendingRegistration` for creating and updating draft, pending, accepted, or rejected equipment registration records. The access component SHALL delegate writes to `EquipmentResource` and SHALL return the persisted `PendingRegistration` state. Draft registrations SHALL require pending registration identity, customer identity, and draft status but SHALL NOT require `EquipmentTypeId` before equipment details are submitted.

#### Scenario: Pending registration is stored
- **WHEN** `Store(PendingRegistration)` is called with a valid draft, submitted, pending-review, accepted, or rejected registration state
- **THEN** `EquipmentAccess` persists the `PendingRegistration` through `EquipmentResource` and returns the stored `PendingRegistration`

#### Scenario: Draft pending registration is stored before equipment details
- **WHEN** `Store(PendingRegistration)` is called with `Status` set to `Draft`, a pending registration identity, and a customer identity but without `EquipmentTypeId`
- **THEN** `EquipmentAccess` persists the draft `PendingRegistration` through `EquipmentResource` and returns the stored `PendingRegistration`

#### Scenario: Pending registration cannot be stored
- **WHEN** `Store(PendingRegistration)` is called with missing customer, status, or state-specific data required for its state
- **THEN** `EquipmentAccess` rejects the store operation and does not persist a partial `PendingRegistration`

### Requirement: Equipment storage
`EquipmentAccess` SHALL expose `Store(Equipment): Equipment` for creating or updating registered equipment records after equipment registration is accepted. The access component SHALL delegate writes to `EquipmentResource` and SHALL return the persisted `Equipment` state.

#### Scenario: Equipment is stored
- **WHEN** `Store(Equipment)` is called with valid registered equipment data associated with a customer
- **THEN** `EquipmentAccess` persists the `Equipment` through `EquipmentResource` and returns the stored `Equipment`

#### Scenario: Equipment cannot be stored
- **WHEN** `Store(Equipment)` is called with missing ownership, equipment type, registration, or identity data
- **THEN** `EquipmentAccess` rejects the store operation and does not persist a partial `Equipment`

