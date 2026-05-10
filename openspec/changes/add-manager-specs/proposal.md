## Why

The manager capabilities currently exist as empty OpenSpec specs, while the P2 business documentation and methodsketch already define the relevant manager responsibilities and interactions. Adding manager requirements to OpenSpec makes those documented behaviors reviewable and reusable for future design and implementation work.

## What Changes

- Add requirement deltas for each manager capability represented in the P2 documentation and methodsketch.
- Capture documented manager responsibilities for membership, maintenance, notification, and backoffice tasking workflows.
- Keep the change documentation-only, with no implementation work or runtime behavior changes.

## Capabilities

### New Capabilities

- None.

### Modified Capabilities

- `membership-manager`: Add requirements for account registration, email confirmation, password update, user preferences, pending registration creation, and pending registration resolution.
- `maintenance-manager`: Add requirements for maintenance plan matching, pending maintenance plan creation and resolution, e-signing initiation, maintenance job slot proposal creation and confirmation, slot proposal acceptance, and maintenance job cancellation.
- `notification-manager`: Add requirements for user notification creation and dispatch through configured templates and communication utilities.
- `tasking-manager`: Add requirements for creating backoffice tasks from queued requests.

## Impact

- Affects OpenSpec documentation under `openspec/changes/add-manager-specs/` only.
- Does not change application code, APIs, dependencies, persistence, or runtime behavior.
