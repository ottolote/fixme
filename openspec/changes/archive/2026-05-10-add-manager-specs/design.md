## Context

The P2 documentation maps system interactions to the owning managers: SI1 to `MembershipManager`, SI2 to `MaintenanceManager`, SI3 to `NotificationManager`, and SI4 to `TaskingManager`. The methodsketch DSL provides the corresponding call chains, access dependencies, engines, utilities, and queued manager interactions for these use cases.

Current OpenSpec manager spec files exist but contain no requirements. This change translates the documented manager responsibilities into OpenSpec delta specs without changing code.

## Goals / Non-Goals

**Goals:**

- Add testable requirements for all manager-owned system interactions documented in P2.
- Preserve the manager boundaries shown in `docs/project/P2/business/business-processes/system-interactions.md` and `docs/methodsketch.txt`.
- Keep requirements focused on observable manager responsibilities and collaborator interactions.

**Non-Goals:**

- Implement application code or tests.
- Define resource, access, engine, utility, or client behavior beyond what is needed as manager collaborators.
- Resolve product details not present in the P2 documentation.

## Decisions

- Use `ADDED Requirements` for each manager spec because the existing manager specs are empty and no current requirement text needs modification.
- Group requirements by system interaction rather than by lower-level data operation so each requirement can be traced back to the P2 documentation and methodsketch use case names.
- Represent queued interactions such as task creation and notification requests as manager obligations to enqueue or handle documented requests, without prescribing a transport implementation.
- Treat this as a docs-only change by making `tasks.md` explicitly state that no implementation tasks are required.

## Risks / Trade-offs

- [Risk] Some business details in BPMN or interaction markdown may be more detailed than the methodsketch call chains. -> Mitigation: keep requirements at the manager responsibility level and avoid inventing unstated field-level rules.
- [Risk] Requirements could over-specify collaborator internals. -> Mitigation: state collaborator expectations only as manager-observable calls or outcomes.
- [Risk] Documentation-only changes still require a tasks artifact for OpenSpec readiness. -> Mitigation: provide a no-op tasks file that records the absence of implementation work.
