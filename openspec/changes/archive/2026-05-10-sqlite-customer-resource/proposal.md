## Why

SI1.5 requires membership workflows to validate that a customer exists and may start an equipment registration. Customer records currently live in an in-memory resource, so customer validation state is not durable across access instances or process restarts.

## What Changes

- Persist `CustomerResource` records in SQLite while keeping `CustomerAccess` public contracts intact.
- Preserve existing customer lookup, storage, clone, and uniqueness behavior for customer identifiers, emails, confirmation tokens, and profile references.
- Add customer validation state needed by SI1.5 so later `MembershipManager` work can determine whether a customer may register equipment without reaching across resource boundaries.
- Add tests covering SQLite-backed customer persistence and customer eligibility validation.

## Capabilities

### New Capabilities

None.

### Modified Capabilities

- `customer-access`: Customer persistence becomes SQLite-backed and exposes customer registration eligibility state for SI1.5 validation.

## Impact

- Affected code: `Component/Access/Customer/Interface`, `Component/Access/Customer/Service`, and customer access tests.
- Dependencies: add a SQLite provider dependency to the customer access service project if the repository does not already provide one.
- API, manager, equipment access, and other resource components are out of scope except for build/test references required by `CustomerAccess`.
