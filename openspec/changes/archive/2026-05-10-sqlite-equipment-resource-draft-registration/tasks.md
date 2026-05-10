## 1. Resource Persistence

- [x] 1.1 Add SQLite-backed storage to `EquipmentResource` for equipment types, equipment, and pending registrations.
- [x] 1.2 Ensure the SQLite schema has no foreign keys to other resource-owned tables.
- [x] 1.3 Preserve default supported equipment type lookup behavior through seeded SQLite rows.

## 2. Access Validation

- [x] 2.1 Allow draft pending registrations with identity, customer, and status but no equipment type.
- [x] 2.2 Keep state-specific validation for submitted/pending-review, accepted, rejected, and equipment storage.

## 3. Tests And Verification

- [x] 3.1 Add or adjust equipment access tests for SQLite persistence across access instances.
- [x] 3.2 Add or adjust tests for draft registration storage without `EquipmentTypeId`.
- [x] 3.3 Run relevant restore/build/test/format checks for equipment access changes.
