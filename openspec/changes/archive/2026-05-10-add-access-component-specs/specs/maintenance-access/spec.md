## ADDED Requirements

### Requirement: Maintenance plan offering filtering
`MaintenanceAccess` SHALL expose `MaintenancePlanOfferingFilter(MaintenancePlanOfferingCriteria): MaintenancePlanOffering` for retrieving offerings used by matching and pending plan creation. The access component SHALL delegate reads to `MaintenanceResource` and SHALL return offerings matching the supplied `MaintenancePlanOfferingCriteria`.

#### Scenario: Matching maintenance plan offerings are returned
- **WHEN** `MaintenancePlanOfferingFilter(MaintenancePlanOfferingCriteria)` is called with criteria matching available or applicable offerings
- **THEN** `MaintenanceAccess` returns the matching `MaintenancePlanOffering` records from `MaintenanceResource`

#### Scenario: No maintenance plan offering matches
- **WHEN** `MaintenancePlanOfferingFilter(MaintenancePlanOfferingCriteria)` is called with criteria matching no offering
- **THEN** `MaintenanceAccess` returns no `MaintenancePlanOffering` result

### Requirement: Maintenance plan filtering
`MaintenanceAccess` SHALL expose `MaintenancePlanFilter(MaintenancePlanCriteria): MaintenancePlan` for retrieving pending, approved-pending-signature, active, rejected, or otherwise resolvable maintenance plans. The access component SHALL delegate reads to `MaintenanceResource` and SHALL return plans matching the supplied `MaintenancePlanCriteria`.

#### Scenario: Matching maintenance plan is returned
- **WHEN** `MaintenancePlanFilter(MaintenancePlanCriteria)` is called with criteria matching an existing maintenance plan
- **THEN** `MaintenanceAccess` returns the matching `MaintenancePlan` from `MaintenanceResource`

#### Scenario: Maintenance plan is not found
- **WHEN** `MaintenancePlanFilter(MaintenancePlanCriteria)` is called with criteria matching no maintenance plan
- **THEN** `MaintenanceAccess` returns no `MaintenancePlan` result

### Requirement: Maintenance plan storage
`MaintenanceAccess` SHALL expose `MaintenancePlanStore(MaintenancePlan): MaintenancePlan` for creating and updating maintenance plans. The access component SHALL delegate writes to `MaintenanceResource` and SHALL return the persisted `MaintenancePlan` state.

#### Scenario: Maintenance plan is stored
- **WHEN** `MaintenancePlanStore(MaintenancePlan)` is called with a valid pending, approved-pending-signature, active, or rejected plan state
- **THEN** `MaintenanceAccess` persists the `MaintenancePlan` through `MaintenanceResource` and returns the stored `MaintenancePlan`

#### Scenario: Maintenance plan cannot be stored
- **WHEN** `MaintenancePlanStore(MaintenancePlan)` is called with missing equipment, offering, customer, status, signature reference, or signed-evidence data required for its state
- **THEN** `MaintenanceAccess` rejects the store operation and does not persist a partial `MaintenancePlan`

### Requirement: Maintenance job slot filtering
`MaintenanceAccess` SHALL expose `MaintenanceJobSlotFilter(MaintenanceJobSlotCriteria): MaintenanceJobSlot` for retrieving slots used by proposal creation and proposal acceptance. The access component SHALL delegate reads to `MaintenanceResource` and SHALL return slots matching the supplied `MaintenanceJobSlotCriteria`.

#### Scenario: Matching maintenance job slots are returned
- **WHEN** `MaintenanceJobSlotFilter(MaintenanceJobSlotCriteria)` is called with criteria matching available, reserved, selected, or otherwise relevant job slots
- **THEN** `MaintenanceAccess` returns the matching `MaintenanceJobSlot` records from `MaintenanceResource`

#### Scenario: No maintenance job slot matches
- **WHEN** `MaintenanceJobSlotFilter(MaintenanceJobSlotCriteria)` is called with criteria matching no job slot
- **THEN** `MaintenanceAccess` returns no `MaintenanceJobSlot` result

### Requirement: Maintenance job slot storage
`MaintenanceAccess` SHALL expose `MaintenanceJobSlotStore(MaintenanceJobSlot): MaintenanceJobSlot` for reserving, releasing, selecting, or otherwise updating maintenance job slots. The access component SHALL delegate writes to `MaintenanceResource` and SHALL return the persisted `MaintenanceJobSlot` state.

#### Scenario: Maintenance job slot is stored
- **WHEN** `MaintenanceJobSlotStore(MaintenanceJobSlot)` is called with a valid available, reserved, released, or selected slot state
- **THEN** `MaintenanceAccess` persists the `MaintenanceJobSlot` through `MaintenanceResource` and returns the stored `MaintenanceJobSlot`

#### Scenario: Maintenance job slot cannot be stored
- **WHEN** `MaintenanceJobSlotStore(MaintenanceJobSlot)` is called with missing schedule, provider, status, reservation, or proposal association data required for its state
- **THEN** `MaintenanceAccess` rejects the store operation and does not persist a partial `MaintenanceJobSlot`

### Requirement: Maintenance job slots proposal filtering
`MaintenanceAccess` SHALL expose `MaintenanceJobSlotsProposalFilter(MaintenanceJobSlotsProposalCriteria): MaintenanceJobSlotsProposal` for retrieving maintenance slot proposals used by confirmation and customer acceptance. The access component SHALL delegate reads to `MaintenanceResource` and SHALL return proposals matching the supplied `MaintenanceJobSlotsProposalCriteria`.

#### Scenario: Matching maintenance job slots proposal is returned
- **WHEN** `MaintenanceJobSlotsProposalFilter(MaintenanceJobSlotsProposalCriteria)` is called with criteria matching an existing proposal
- **THEN** `MaintenanceAccess` returns the matching `MaintenanceJobSlotsProposal` from `MaintenanceResource`

#### Scenario: Maintenance job slots proposal is not found
- **WHEN** `MaintenanceJobSlotsProposalFilter(MaintenanceJobSlotsProposalCriteria)` is called with criteria matching no proposal
- **THEN** `MaintenanceAccess` returns no `MaintenanceJobSlotsProposal` result

### Requirement: Maintenance job slots proposal storage
`MaintenanceAccess` SHALL expose `MaintenanceJobSlotsProposalStore(MaintenanceJobSlotsProposal): MaintenanceJobSlotsProposal` for creating and updating maintenance slot proposals. The access component SHALL delegate writes to `MaintenanceResource` and SHALL return the persisted `MaintenanceJobSlotsProposal` state.

#### Scenario: Maintenance job slots proposal is stored
- **WHEN** `MaintenanceJobSlotsProposalStore(MaintenanceJobSlotsProposal)` is called with a valid created, confirmed, selected, expired, or cancelled proposal state
- **THEN** `MaintenanceAccess` persists the `MaintenanceJobSlotsProposal` through `MaintenanceResource` and returns the stored `MaintenanceJobSlotsProposal`

#### Scenario: Maintenance job slots proposal cannot be stored
- **WHEN** `MaintenanceJobSlotsProposalStore(MaintenanceJobSlotsProposal)` is called with missing maintenance plan, slot, customer, status, expiry, or confirmation data required for its state
- **THEN** `MaintenanceAccess` rejects the store operation and does not persist a partial `MaintenanceJobSlotsProposal`

### Requirement: Maintenance job filtering
`MaintenanceAccess` SHALL expose `MaintenanceJobFilter(MaintenanceJobCriteria): MaintenanceJob` for retrieving scheduled maintenance jobs used by cancellation workflows. The access component SHALL delegate reads to `MaintenanceResource` and SHALL return jobs matching the supplied `MaintenanceJobCriteria`.

#### Scenario: Matching maintenance job is returned
- **WHEN** `MaintenanceJobFilter(MaintenanceJobCriteria)` is called with criteria matching an existing maintenance job
- **THEN** `MaintenanceAccess` returns the matching `MaintenanceJob` from `MaintenanceResource`

#### Scenario: Maintenance job is not found
- **WHEN** `MaintenanceJobFilter(MaintenanceJobCriteria)` is called with criteria matching no maintenance job
- **THEN** `MaintenanceAccess` returns no `MaintenanceJob` result

### Requirement: Maintenance job storage
`MaintenanceAccess` SHALL expose `MaintenanceJobStore(MaintenanceJob): MaintenanceJob` for creating scheduled jobs and updating job cancellation state. The access component SHALL delegate writes to `MaintenanceResource` and SHALL return the persisted `MaintenanceJob` state.

#### Scenario: Maintenance job is stored
- **WHEN** `MaintenanceJobStore(MaintenanceJob)` is called with a valid scheduled or cancelled maintenance job state
- **THEN** `MaintenanceAccess` persists the `MaintenanceJob` through `MaintenanceResource` and returns the stored `MaintenanceJob`

#### Scenario: Maintenance job cannot be stored
- **WHEN** `MaintenanceJobStore(MaintenanceJob)` is called with missing plan, equipment, selected slot, schedule, status, or cancellation data required for its state
- **THEN** `MaintenanceAccess` rejects the store operation and does not persist a partial `MaintenanceJob`
