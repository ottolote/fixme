## Purpose

The `MaintenanceManager` owns maintenance plan enrollment, e-signing, scheduling proposal, slot acceptance, and job cancellation workflows documented by P2 SI2 interactions.

## Requirements

### Requirement: SI2.1 Match maintenance plan offering
The `MaintenanceManager` SHALL expose `MatchMaintenancePlanOffering(MatchMaintenancePlanOfferingRequest): MatchMaintenancePlanOfferingResponse` for matching maintenance plan offerings to registered eligible equipment. The manager SHALL load equipment with `EquipmentAccess.Filter(EquipmentCriteria)`, load candidate offerings with `MaintenanceAccess.MaintenancePlanOfferingFilter(MaintenancePlanOfferingCriteria)`, and delegate offer matching to `MatchingEngine.MatchMaintenancePlanOffering(MatchMaintenancePlanOfferingRequest)`.

#### Scenario: Matching offerings are returned
- **WHEN** a `MatchMaintenancePlanOfferingRequest` references equipment that exists, is registered, and is eligible, and matching offerings exist
- **THEN** the manager returns `MatchMaintenancePlanOfferingResponse` with matching options and pricing and records `Maintenance plan options matched`

#### Scenario: No offering matches
- **WHEN** a `MatchMaintenancePlanOfferingRequest` references registered eligible equipment but no maintenance offering matches
- **THEN** the manager returns `MatchMaintenancePlanOfferingResponse` as a successful no-options result

#### Scenario: Equipment is invalid for matching
- **WHEN** a `MatchMaintenancePlanOfferingRequest` references missing, unregistered, or ineligible equipment
- **THEN** the manager returns `equipment-not-found`, `equipment-not-registered`, or `equipment-ineligible` without invoking the matching engine

### Requirement: SI2.2 Create pending maintenance plan
The `MaintenanceManager` SHALL expose `CreatePendingMaintenancePlan(CreatePendingMaintenancePlanRequest): CreatePendingMaintenancePlanResponse` for customer selection of a maintenance plan offering. The manager SHALL validate offering availability/applicability, load equipment with `EquipmentAccess.Filter(EquipmentCriteria)`, load offerings with `MaintenanceAccess.MaintenancePlanOfferingFilter(MaintenancePlanOfferingCriteria)`, check existing plans with `MaintenanceAccess.MaintenancePlanFilter(MaintenancePlanCriteria)`, and persist the pending plan with `MaintenanceAccess.MaintenancePlanStore(MaintenancePlan)`.

#### Scenario: Pending plan is created
- **WHEN** a `CreatePendingMaintenancePlanRequest` selects an existing available offering that applies to the equipment and no pending or active plan exists for that equipment
- **THEN** the manager locks the selected offering price, stores a pending maintenance plan, returns `CreatePendingMaintenancePlanResponse` as successful, and records `Pending maintenance plan created` for downstream `CreateBackofficeTask(CreateBackofficeTaskRequest): CreateBackofficeTaskResponse`

#### Scenario: Offering is invalid
- **WHEN** a `CreatePendingMaintenancePlanRequest` selects an offering that is missing, unavailable, or not applicable to the equipment
- **THEN** the manager returns `offering-not-found`, `offering-unavailable`, or `offering-not-applicable` without creating a pending plan

#### Scenario: Duplicate plan exists
- **WHEN** a `CreatePendingMaintenancePlanRequest` references equipment that already has a pending or active maintenance plan
- **THEN** the manager returns `duplicate-plan` without creating another plan

### Requirement: SI2.3 Resolve pending maintenance plan
The `MaintenanceManager` SHALL expose `ResolvePendingMaintenancePlan(ResolvePendingMaintenancePlanRequest): ResolvePendingMaintenancePlanResponse` for queued rejection or signed-agreement outcomes. The manager SHALL load plans with `MaintenanceAccess.MaintenancePlanFilter(MaintenancePlanCriteria)`, load signed evidence with `AgreementAccess.Filter(AgreementCriteria)` when activating, persist plan changes with `MaintenanceAccess.MaintenancePlanStore(MaintenancePlan)`, and reject invalid queued events for dead-letter handling.

#### Scenario: Pending plan is rejected
- **WHEN** a `ResolvePendingMaintenancePlanRequest` carries a valid rejection outcome for a resolvable pending maintenance plan
- **THEN** the manager stores the rejection reason when supplied, marks the plan rejected, returns `ResolvePendingMaintenancePlanResponse` as successful, and emits or queues `Maintenance plan rejected` notification work through `NotifyUser(NotifyUserRequest): NotifyUserResponse`

#### Scenario: Pending plan is activated
- **WHEN** a `ResolvePendingMaintenancePlanRequest` carries a signed-agreement outcome for a plan that is approved pending signature and has signed agreement evidence
- **THEN** the manager attaches or links the signed evidence, marks the plan active, returns `ResolvePendingMaintenancePlanResponse` as successful, and records `Maintenance plan activated`

#### Scenario: Event cannot be resolved
- **WHEN** a `ResolvePendingMaintenancePlanRequest` has an invalid outcome, references no plan, references an already-resolved plan, or lacks required signature evidence
- **THEN** the manager rejects the event for dead-letter handling with `invalid-outcome`, `plan-not-found`, `already-resolved`, or `missing-signature-evidence`

### Requirement: SI2.4 Initiate eSigning for maintenance plan
The `MaintenanceManager` SHALL expose `InitiateESigningForMaintenancePlan(InitiateESigningForMaintenancePlanRequest): InitiateESigningForMaintenancePlanResponse` for approved pending plans. The manager SHALL load the plan with `MaintenanceAccess.MaintenancePlanFilter(MaintenancePlanCriteria)`, create an agreement with `AgreementAccess.Store(Agreement)`, request signature with `ESigningAccess.SignatureRequestStore(SignatureRequest)`, store the signature reference, and persist plan status with `MaintenanceAccess.MaintenancePlanStore(MaintenancePlan)`.

#### Scenario: Signature request is created
- **WHEN** an `InitiateESigningForMaintenancePlanRequest` references an approved pending plan whose customer can sign
- **THEN** the manager marks the plan approved pending signature, generates the maintenance plan agreement, creates the external signature request, stores the signature order reference, returns `InitiateESigningForMaintenancePlanResponse` as successful, and records `Maintenance plan signature requested`

#### Scenario: Signature initiation cannot proceed
- **WHEN** an `InitiateESigningForMaintenancePlanRequest` references no plan, a plan that is not approved, a customer that cannot sign, or an e-signing request that is not accepted
- **THEN** the manager rejects the event for dead-letter handling with `plan-not-found`, `plan-not-approved`, `customer-not-signable`, or `esigning-request-failed`

### Requirement: SI2.5 Create maintenance job slots proposal
The `MaintenanceManager` SHALL expose `CreateMaintenanceJobSlotsProposal(CreateMaintenanceJobSlotsProposalRequest): CreateMaintenanceJobSlotsProposalResponse` for customer maintenance-slot requests. The manager SHALL load equipment with `EquipmentAccess.Filter(EquipmentCriteria)`, load active plans with `MaintenanceAccess.MaintenancePlanFilter(MaintenancePlanCriteria)`, load available slots with `MaintenanceAccess.MaintenanceJobSlotFilter(MaintenanceJobSlotCriteria)`, ask `SchedulingEngine.CreateMaintenanceJobSlotsProposal(CreateMaintenanceJobSlotsProposalRequest)` to select slots, store slot reservations with `MaintenanceAccess.MaintenanceJobSlotStore(MaintenanceJobSlot)`, and store the proposal with `MaintenanceAccess.MaintenanceJobSlotsProposalStore(MaintenanceJobSlotsProposal)`.

#### Scenario: Proposal is created
- **WHEN** a `CreateMaintenanceJobSlotsProposalRequest` references equipment with an active maintenance plan and at least three system-owned available maintenance job slots
- **THEN** the manager reserves three proposed slots for 24 hours, stores a maintenance job slots proposal, returns `CreateMaintenanceJobSlotsProposalResponse` as successful, and records `Maintenance slots proposal created` for downstream `CreateBackofficeTask(CreateBackofficeTaskRequest): CreateBackofficeTaskResponse`

#### Scenario: Proposal cannot be created
- **WHEN** a `CreateMaintenanceJobSlotsProposalRequest` references missing equipment, equipment without an active plan, or fewer than three eligible slots
- **THEN** the manager returns `equipment-not-found`, `no-active-plan`, or `insufficient-slots` without creating a proposal

#### Scenario: Reserved slots expire
- **WHEN** reserved proposal slots remain unconfirmed or unsent for 24 hours
- **THEN** the manager treats those reservations as expired and unavailable for customer selection through that proposal

### Requirement: SI2.6 Confirm maintenance slots proposal
The `MaintenanceManager` SHALL expose `ConfirmMaintenanceSlotsProposal(ConfirmMaintenanceSlotsProposalRequest): ConfirmMaintenanceSlotsProposalResponse` for backoffice/provider confirmation of proposed slots. The manager SHALL load the task with `TaskAccess.Filter(TaskCriteria)`, close it with `TaskAccess.Store(Task)`, load the proposal with `MaintenanceAccess.MaintenanceJobSlotsProposalFilter(MaintenanceJobSlotsProposalCriteria)`, and persist confirmation with `MaintenanceAccess.MaintenanceJobSlotsProposalStore(MaintenanceJobSlotsProposal)`.

#### Scenario: Proposal is confirmed
- **WHEN** a `ConfirmMaintenanceSlotsProposalRequest` references an open maintenance-slots-proposal confirmation task and a confirmable proposal whose active reserved slots match the submitted confirmation
- **THEN** the manager records provider slot confirmation, closes the task, marks the proposal confirmed, returns `ConfirmMaintenanceSlotsProposalResponse` as successful, and emits or queues `Maintenance slots proposal confirmed` notification work through `NotifyUser(NotifyUserRequest): NotifyUserResponse`

#### Scenario: Confirmation task is invalid
- **WHEN** a `ConfirmMaintenanceSlotsProposalRequest` references a missing task, closed task, or task whose type is not maintenance slots proposal confirmation
- **THEN** the manager returns `task-not-found`, `task-not-open`, or `invalid-task-type` without changing the proposal

#### Scenario: Proposal cannot be confirmed
- **WHEN** a `ConfirmMaintenanceSlotsProposalRequest` references a missing proposal, non-confirmable proposal, expired/cancelled/already-confirmed proposal, or slots that do not match the proposal
- **THEN** the manager returns `proposal-not-found`, `proposal-not-confirmable`, or `slot-mismatch` without closing the task

### Requirement: SI2.7 Accept maintenance slots proposal
The `MaintenanceManager` SHALL expose `AcceptMaintenanceSlotsProposal(AcceptMaintenanceSlotsProposalRequest): AcceptMaintenanceSlotsProposalResponse` for customer selection of a confirmed proposal slot. The manager SHALL load proposals with `MaintenanceAccess.MaintenanceJobSlotsProposalFilter(MaintenanceJobSlotsProposalCriteria)`, load selected slots with `MaintenanceAccess.MaintenanceJobSlotFilter(MaintenanceJobSlotCriteria)`, delegate atomic selection to `SchedulingEngine.AcceptMaintenanceSlotsProposal(AcceptMaintenanceSlotsProposalRequest)`, store the maintenance job with `MaintenanceAccess.MaintenanceJobStore(MaintenanceJob)`, and update slots with `MaintenanceAccess.MaintenanceJobSlotStore(MaintenanceJobSlot)`.

#### Scenario: Slot is accepted
- **WHEN** an `AcceptMaintenanceSlotsProposalRequest` selects an available, unexpired slot that belongs to a selectable confirmed proposal
- **THEN** the manager atomically schedules the selected slot as a maintenance job, releases all non-selected reserved slots, keeps the proposal for audit history, returns `AcceptMaintenanceSlotsProposalResponse` with the scheduled maintenance slot, and records `Maintenance job scheduled`

#### Scenario: Slot cannot be selected
- **WHEN** an `AcceptMaintenanceSlotsProposalRequest` references a missing proposal, non-selectable proposal, selected slot outside the proposal, or unavailable selected slot
- **THEN** the manager returns `proposal-not-found`, `proposal-not-selectable`, `selected-slot-not-in-proposal`, or `selected-slot-unavailable` without scheduling a maintenance job

### Requirement: SI2.8 Cancel maintenance job
The `MaintenanceManager` SHALL expose `CancelMaintenanceJob(CancelMaintenanceJobRequest): CancelMaintenanceJobResponse` for customer self-service cancellation. The manager SHALL load jobs with `MaintenanceAccess.MaintenanceJobFilter(MaintenanceJobCriteria)`, persist cancellations with `MaintenanceAccess.MaintenanceJobStore(MaintenanceJob)`, and create downstream backoffice work for provider cancellation notification.

#### Scenario: Job is cancelled
- **WHEN** a `CancelMaintenanceJobRequest` references a scheduled cancellable job more than 24 hours before its start time
- **THEN** the manager marks the job cancelled, returns `CancelMaintenanceJobResponse` as successful, and records `Maintenance job cancelled` for downstream `CreateBackofficeTask(CreateBackofficeTaskRequest): CreateBackofficeTaskResponse`

#### Scenario: Job cannot be cancelled
- **WHEN** a `CancelMaintenanceJobRequest` references no job, a job that is not scheduled, or a job less than 24 hours before start time
- **THEN** the manager returns `job-not-found`, `job-not-scheduled`, or `cancellation-not-allowed` without changing the job state
