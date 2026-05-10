## Context

The API host currently uses `HttpListener` with explicit routing in `Host/Api/FixMeApi.cs` and exposes only health and root API description endpoints. The SI1.5 business interaction and `MembershipManager.CreatePendingRegistration` behavior already exist at the manager boundary, so this change only needs to add the external REST entry point and keep the API routed through the manager.

## Goals / Non-Goals

**Goals:**
- Expose `POST /api/v1/equipment-registrations/drafts` for customers to create draft equipment registrations.
- Deserialize `CustomerId` and optional `EquipmentTypeId` from JSON and call `MembershipManager.CreatePendingRegistration`.
- Return created draft registration JSON on success and map known manager validation errors to 4xx responses.
- Allow API tests to inject a controlled membership manager without seeding production resources through the API.

**Non-Goals:**
- Do not change access/resource internals or have API code call access/resource components directly.
- Do not create or enqueue backoffice tasks from the API endpoint.
- Do not implement SI1.6 equipment detail submission or duplicate registration handling.

## Decisions

- Use `POST /api/v1/equipment-registrations/drafts` because it is explicit, versioned, and matches the existing `/api/v1` host surface.
- Inject `IMembershipManager` into `FixMeApi` with a production default built from existing `CustomerAccess` and `EquipmentAccess`; tests can pass a fake manager to keep setup minimal and avoid direct API-to-resource calls.
- Map manager `Error` values from `PendingRegistration` to HTTP status codes: `invalid-customer` to `404 NotFound`, `invalid-equipment-type` to `400 BadRequest`, and other validation errors to `400 BadRequest`.
- Treat malformed JSON or missing request body as `400 BadRequest` before calling the manager.

## Risks / Trade-offs

- Minimal `HttpListener` routing keeps the API simple but duplicates framework behavior that ASP.NET Core would normally provide. Mitigation: keep parsing and response helpers small and localized.
- The production default manager uses current access defaults, so a real customer must already exist and be confirmed before the endpoint succeeds. Mitigation: tests use manager injection, and customer creation flows remain separate.
