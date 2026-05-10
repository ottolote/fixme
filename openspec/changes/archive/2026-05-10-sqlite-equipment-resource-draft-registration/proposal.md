## Why

SI1.5 creates a draft equipment registration before equipment details are submitted, but the current equipment access implementation is in-memory and treats equipment type as required too early. Equipment persistence needs a resource-owned SQLite backing store so draft registrations can survive access instances without introducing cross-resource coupling.

## What Changes

- Back `EquipmentResource` with SQLite persistence for equipment types, equipment, and pending registrations.
- Preserve the existing `EquipmentAccess` public contract while delegating persistence through `EquipmentResource`.
- Allow `PendingRegistrationStatus.Draft` records to be stored with minimal identity/customer state and no `EquipmentTypeId`.
- Continue validating state-specific fields for submitted/pending-review, accepted, and rejected registrations.
- Keep `EquipmentResource` independent: no calls to other resources and no foreign keys to other resources.
- Add tests covering SQLite-backed persistence and draft registration storage.

## Capabilities

### New Capabilities

- None

### Modified Capabilities

- `equipment-access`: draft pending registrations can be stored without equipment type details.
- `equipment-resource`: equipment resource persists equipment access data in SQLite while remaining resource-independent.

## Impact

- Affected code: `Component/Access/Equipment/Service`, `Component/Access/Equipment/Interface`, and equipment access tests.
- Dependencies: add or use a SQLite data provider/package if one is not already available to the equipment service test path.
- No API, CustomerAccess, or MembershipManager changes are intended.
