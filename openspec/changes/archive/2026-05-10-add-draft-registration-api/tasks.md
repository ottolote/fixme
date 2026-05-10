## 1. API Wiring

- [x] 1.1 Add membership-manager dependency wiring to `FixMeApi` with injectable manager support for tests.
- [x] 1.2 Add `POST /api/v1/equipment-registrations/drafts` request parsing and response handling.
- [x] 1.3 Map `invalid-customer`, `invalid-equipment-type`, and malformed request failures to appropriate 4xx JSON responses.
- [x] 1.4 Update root API capability description to include draft registration creation.

## 2. Tests

- [x] 2.1 Add TestApiHost-style success test for draft registration creation.
- [x] 2.2 Add TestApiHost-style invalid customer test.
- [x] 2.3 Verify the endpoint calls the manager seam and does not require API test code to seed production resources.

## 3. Verification

- [x] 3.1 Run relevant API tests.
- [x] 3.2 Run OpenSpec verification for the change and address significant shortcomings.
