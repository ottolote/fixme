## Context

The main `membership-manager` spec already defines SI1.5 draft registration behavior: validate a customer, persist a draft registration, return it, and record `Pending equipment registration created` without directly creating backoffice work. Customer and equipment access components now provide the persistence seams required by this manager workflow.

## Goals / Non-Goals

**Goals:**

- Implement the MembershipManager SI1.5 method using `CustomerAccess.Filter(CustomerCriteria)` and `EquipmentAccess.Store(PendingRegistration)`.
- Keep draft creation valid before equipment details are known, including no equipment type.
- Represent the business event on the manager result so downstream event publication can be wired later without coupling to TaskingManager.
- Add focused tests for manager orchestration and failure behavior.

**Non-Goals:**

- Do not add or change the REST API endpoint.
- Do not directly create backoffice tasks or add a tasking-manager dependency for SI1.5.
- Do not add resource-to-resource calls or cross-resource foreign keys; customer references in equipment registration remain weak references.
- Do not implement SI1.6 submission policy or SI1.7 backoffice resolution behavior beyond preserving existing contracts.

## Decisions

- Use access interfaces from the manager only. This preserves the manager/access/resource layering and avoids direct resource calls from `MembershipManager`.
- Treat a confirmed customer as sufficient permission to start a draft registration. This matches the current SI1.5 documentation and avoids inventing additional authorization rules.
- Allow missing equipment type in the request. SI1.5 explicitly supports starting registration before equipment details, so equipment-type lookup is conditional and only applies when a type is supplied.
- Represent `Pending equipment registration created` as response data rather than an immediate tasking call. This keeps business-event visibility testable without introducing an event bus implementation or direct backoffice task creation in this component slice.

## Risks / Trade-offs

- Event representation is local to the manager response for now -> later event-bus wiring must map this response event into the platform event publication mechanism.
- Equipment-type validation depends on the available equipment access contract -> tests should assert the manager only invokes it when input includes equipment type.
- Existing public contracts may be minimal -> add only small request/response fields needed for SI1.5 and preserve current method names.
