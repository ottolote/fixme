## Context

`docs/methodsketch.txt` lists the architecture components and their use case interactions, but the OpenSpec workspace has no main specs or change specs for those component boundaries. This change creates placeholder capability directories so future requirements can be added incrementally to the appropriate component-owned capability.

## Goals / Non-Goals

**Goals:**
- Represent every architecture component from the sketch as an OpenSpec capability placeholder.
- Keep each placeholder empty so no behavior is specified prematurely.
- Avoid implementation planning because there is no code change in scope.

**Non-Goals:**
- Define component requirements, scenarios, APIs, persistence models, or runtime behavior.
- Implement any clients, managers, engines, accesses, resources, or utilities.
- Change existing business process or system interaction documentation.

## Decisions

- Use one capability per architecture component. This preserves the architecture vocabulary and gives future changes an obvious spec location.
- Use kebab-case capability names derived from component names. This follows OpenSpec naming conventions while keeping names traceable to the sketch.
- Leave spec files empty. The change is only an inventory scaffold; adding normative requirements would imply decisions that have not been made.
- Omit `tasks.md`. There are no implementation tasks because the requested output is specification placeholders only.

## Risks / Trade-offs

- Empty specs are not implementation-ready OpenSpec requirement deltas -> Future changes must add real requirements and scenarios before implementation.
- Component-level capabilities may be too granular for some future behavior -> Capabilities can later be merged or superseded once requirements clarify stable boundaries.
