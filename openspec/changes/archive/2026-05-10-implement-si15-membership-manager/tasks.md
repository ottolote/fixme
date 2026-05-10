## 1. Manager Contract

- [x] 1.1 Inspect current membership, customer access, and equipment access contracts for draft-registration support.
- [x] 1.2 Add minimal request/response/model fields needed for SI1.5 while preserving existing public method names.

## 2. Manager Implementation

- [x] 2.1 Implement customer validation using `CustomerAccess.Filter(CustomerCriteria)` and require a confirmed customer.
- [x] 2.2 Implement optional equipment-type validation using `EquipmentAccess.Filter(EquipmentTypeCriteria)` only when an equipment type is supplied.
- [x] 2.3 Store draft `PendingRegistration` through `EquipmentAccess.Store(PendingRegistration)` and return the stored draft.
- [x] 2.4 Represent `Pending equipment registration created` on success without invoking tasking/backoffice work.

## 3. Tests

- [x] 3.1 Add success test for draft creation without equipment type.
- [x] 3.2 Add invalid-customer test that verifies no equipment store occurs.
- [x] 3.3 Add equipment-type validation test for supplied invalid equipment type.
- [x] 3.4 Add test coverage proving draft creation does not directly create backoffice tasking work.

## 4. Verification

- [x] 4.1 Run relevant membership manager tests.
- [x] 4.2 Run solution build or broader relevant test suite.
- [x] 4.3 Run OpenSpec verification and resolve significant shortcomings.
