## Context

The P2 documentation describes system interactions for membership, maintenance, tasking, and notifications. `docs/methodsketch.txt` identifies the access components used by those flows and names the DTOs/entities exchanged across manager-to-access boundaries. The corresponding OpenSpec access component capabilities already exist, but their `spec.md` files are empty.

This change is documentation-only. It adds requirements that define the access component contracts without changing runtime code, storage schemas, or generated artifacts.

## Goals / Non-Goals

**Goals:**

- Document each access component operation shown in `docs/methodsketch.txt` using the same DTO and entity names.
- Make the resource boundary explicit for each access component: `CustomerResource`, `EquipmentResource`, `MaintenanceResource`, `TaskResource`, `NotificationResource`, `AgreementResource`, and `ExternalESigningResource`.
- Specify testable filter/store behavior that manager specs can depend on, including empty filter results, validation, persistence, and returned persisted entities.
- Keep the OpenSpec change apply-ready even though it has no code implementation work.

**Non-Goals:**

- No application implementation, API endpoint, database migration, or dependency changes.
- No DTO renaming or domain model redesign.
- No changes to manager behavior beyond documenting the access operations they already call.
- No attempt to define physical storage schemas for resources.

## Decisions

- Use existing capability names rather than creating new capabilities. The access folders already exist under `openspec/specs/`, so this change adds requirements to `customer-access`, `equipment-access`, `maintenance-access`, `task-access`, `notification-access`, `agreement-access`, and `e-signing-access`.
- Use one delta spec file per access capability. This keeps each persistence boundary independently archivable and mirrors the component names in methodsketch.
- Use `## ADDED Requirements` for all delta specs. The current access component spec files are empty, so there are no existing requirement blocks to modify.
- Preserve methodsketch DTO/entity names exactly in requirement text. Names such as `MaintenanceJobSlotsProposalCriteria`, `NotificationTemplateCriteria`, `SignatureRequest`, and `CreateBackofficeTaskRequest` remain unchanged so specs can be traced directly to `docs/methodsketch.txt`.
- Specify access behavior at the logical contract level. The specs describe filtering, storing, resource delegation, and failure/empty-result handling, while leaving database tables, indexes, transactions, and provider API details out of scope.

## Risks / Trade-offs

- Spec names may expose methodsketch inconsistencies such as generic `Filter` versus specific `MaintenancePlanFilter` methods. Mitigation: keep the exact method names where methodsketch provides specific names and use the documented access component boundary as the source of truth.
- Empty-result semantics are documented as access-layer behavior, but manager specs may map them to different domain failures. Mitigation: access specs only require returning no matching entity or rejecting invalid store input; manager-specific failure names remain in manager specs.
- Documentation-only tasks can be misread as requiring code implementation. Mitigation: tasks explicitly state that implementation work is intentionally not applicable for this change.
