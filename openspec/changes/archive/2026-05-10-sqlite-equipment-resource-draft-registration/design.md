## Context

`EquipmentAccess` currently owns simple persistence behavior for equipment, equipment types, and pending registrations through `EquipmentResource`. SI1.5 requires draft registration creation before equipment details are submitted, so draft pending registrations cannot require an equipment type. The resource must remain independent from `CustomerResource` and other resources; customer IDs are weak references only.

## Goals / Non-Goals

**Goals:**

- Persist equipment types, equipment, and pending registrations through SQLite-backed `EquipmentResource` storage.
- Preserve existing `EquipmentAccess` method contracts and behavior where SI1.5 does not require change.
- Allow draft pending registrations to store only identity, customer, and draft status.
- Enforce stricter state-specific validation for non-draft registrations.
- Keep `EquipmentResource` free of cross-resource calls and foreign keys to other resources.

**Non-Goals:**

- Do not implement the MembershipManager SI1.5 orchestration in this change.
- Do not change CustomerAccess, CustomerResource, or API behavior.
- Do not add duplicate-registration detection or equipment type-specific attributes.

## Decisions

- Use SQLite directly inside `EquipmentResource` rather than adding a shared persistence abstraction. This is the smallest change that satisfies resource-owned persistence and avoids introducing broader architecture decisions.
- Use resource-local tables without foreign key constraints to external resources. `CustomerId`, `EquipmentTypeId`, and `RegistrationId` remain stored identifiers; no referential coupling is created to CustomerResource or other resources.
- Seed supported equipment types in the resource database if missing. This preserves existing equipment type filtering behavior while moving storage behind SQLite.
- Keep default constructors usable by backing them with isolated in-memory SQLite connections. Tests that need persistence across access/resource instances can pass a shared connection string or resource instance.

## Risks / Trade-offs

- SQLite connection lifetime matters for in-memory databases -> tests that need cross-instance persistence should use a file-backed temporary database or a shared connection.
- Direct SQL in the resource is less abstract than a repository layer -> acceptable because this component has a narrow data model and the goal is minimal change.
- No external foreign keys means weak references can point to missing customers or equipment types -> this intentionally preserves resource independence; higher-level managers own cross-resource validation.
