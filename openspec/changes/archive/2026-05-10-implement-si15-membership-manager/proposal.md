## Why

SI1.5 draft equipment registration needs a concrete `MembershipManager` implementation after customer and equipment SQLite access support has landed. The manager must coordinate customer validation and equipment draft persistence while preserving component boundaries and avoiding direct backoffice task creation.

## What Changes

- Implement `CreatePendingRegistration(CreatePendingRegistrationRequest)` on `MembershipManager` for draft registration creation.
- Validate that the referenced customer exists and is allowed to register equipment before storing a draft.
- Persist draft `PendingRegistration` records through `EquipmentAccess.Store(PendingRegistration)` and return the created draft.
- Represent the `Pending equipment registration created` business event on the manager response without invoking tasking/backoffice work.
- Preserve optional equipment-type validation only when draft input includes an equipment type.
- Add manager-level tests for success, invalid customer, optional equipment type validation, and no direct task creation.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `membership-manager`: Clarify and implement SI1.5 draft registration manager behavior and response/event representation.

## Impact

- Affected code: `Component/Manager/Membership/**` plus manager tests.
- Affected interfaces: small additions to membership draft-registration request/response models if needed.
- Dependencies: uses existing `CustomerAccess` and `EquipmentAccess` component interfaces; no new resource calls, cross-resource foreign keys, REST endpoint, or tasking-manager dependency.
- Operational impact: none expected for hosting, containers, CI, or observability.
