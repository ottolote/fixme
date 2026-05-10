## Why

SI1.5 draft equipment registration is already specified at the membership-manager boundary, but the external API does not yet expose the customer web entry point. Customers need a REST endpoint that starts a draft registration through `MembershipManager` while preserving the existing manager/access/resource layering.

## What Changes

- Add a customer-facing REST endpoint for creating draft equipment registrations.
- Accept request JSON containing `CustomerId` and optional `EquipmentTypeId`.
- Route API requests through `MembershipManager.CreatePendingRegistration` and return the created draft registration on success.
- Map invalid customer and validation failures to appropriate 4xx responses.
- Keep backoffice task creation out of this endpoint.
- Add API tests for successful draft creation and invalid customer handling.
- Update the root API capability description to advertise draft registration creation.

## Capabilities

### New Capabilities
- `draft-registration-api`: Customer web REST API behavior for creating SI1.5 draft equipment registrations.

### Modified Capabilities
- `customer-web-client`: Exposes the SI1.5 customer web interaction through the existing API surface.

## Impact

- Affects `Host/Api/FixMeApi.cs` routing, JSON request/response handling, and API capability metadata.
- Affects `tests/FixMe.Api.Tests/FixMeApiTests.cs` with TestApiHost-based coverage.
- Uses existing membership-manager behavior and access seams; no access/resource internals should change unless wiring requires it.
- No new external dependencies, container changes, or CI command changes are expected.
