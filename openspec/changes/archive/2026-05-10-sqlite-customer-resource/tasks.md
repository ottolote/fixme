## 1. Customer Model and Resource Persistence

- [x] 1.1 Add derived customer registration eligibility state to the customer model without changing existing access method signatures.
- [x] 1.2 Replace or augment in-memory `CustomerResource` storage with SQLite-backed persistence and schema initialization.
- [x] 1.3 Preserve existing customer lookup, update, clone, and uniqueness behavior through the SQLite resource.

## 2. Tests and Verification

- [x] 2.1 Add or adjust tests proving customers persist through SQLite across access/resource instances.
- [x] 2.2 Add or adjust tests for confirmed customer validation state relevant to equipment registration drafts.
- [x] 2.3 Run relevant restore, format, build, and test commands for the worktree.
