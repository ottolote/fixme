## Context

`CustomerAccess` already exposes the membership-facing customer `Filter` and `Store` contract and delegates to `CustomerResource`. `CustomerResource` is currently in-memory, which preserves behavior within one process but cannot persist customer state for SI1.5 customer eligibility checks.

The resource architecture treats `CustomerResource` as its own defined resource. This change keeps `CustomerResource` isolated: it stores only customer-owned fields and weak reference values such as profile references, with no calls to other resources and no foreign keys to other resources.

## Goals / Non-Goals

**Goals:**

- Back customer storage with SQLite.
- Preserve existing `CustomerAccess` public methods and existing customer lookup/update semantics.
- Add minimal customer eligibility state for equipment registration draft validation.
- Keep customer persistence independent from equipment, membership, API, and other resources.

**Non-Goals:**

- Implement SI1.5 `MembershipManager`, `EquipmentAccess`, or API orchestration.
- Add relational links or foreign keys from customers to other resource tables.
- Introduce a broader persistence abstraction beyond the customer resource.

## Decisions

- Use SQLite directly inside `CustomerResource` because this component needs a small durable store and the repository does not yet require a shared ORM or database access layer for this scope. Alternative considered: keep the in-memory list and serialize to disk, but that would not exercise SQLite persistence and would not match the SI1.5 requirement.
- Keep `CustomerAccess` constructors compatible by defaulting to a shared SQLite-backed `CustomerResource`. Tests can construct isolated resources with explicit database paths or in-memory SQLite connections through internal test-visible constructors.
- Expose customer eligibility as derived customer state: a customer may register equipment when their email is confirmed. Alternative considered: add a separate boolean permission flag, but there is no current business rule requiring registration permission to vary independently from confirmed membership.
- Enforce uniqueness in `CustomerResource` using SQLite unique indexes and the existing resource-level conflict checks so errors stay coherent with current behavior.

## Risks / Trade-offs

- SQLite file location for the shared resource may need production configuration later -> use a minimal default now and keep explicit resource construction available for tests and future composition.
- Adding eligibility state to `Customer` extends the public model -> keep it derived from existing confirmation state and aligned with customer validation rather than SI1.5 orchestration.
- Direct SQLite access adds a package/runtime dependency -> limit it to the customer service project and verify restore/build/tests locally.
