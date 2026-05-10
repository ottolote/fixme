## MODIFIED Requirements

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
