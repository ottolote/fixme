# Documentation Consistency Tasks

This task list captures the documentation cleanup needed after reviewing `./docs/`, especially the rule that the first number in each system interaction ID should indicate the manager that owns the interaction.

## Context

- `docs/methodsketch.txt` defines the component model and is the best current source for manager ownership.
- Relevant managers in `methodsketch.txt` appear in this order: `MembershipManager`, `MaintenanceManager`, `NotificationManager`, `TaskingManager`.
- Current system interaction IDs are internally consistent across many markdown, PlantUML, and BPMN files, but several IDs do not appear to reflect the owning manager.
- Any renumbering must be applied consistently across interaction markdown files, PlantUML files, generated diagram exports, the system interaction index, and BPMN call activity names.

## Tasks

1. Document the manager-to-system-interaction numbering convention.
   - Add an explicit legend near `docs/business/business-processes/system-interactions.md` or an appropriate architecture/index doc.
   - Suggested mapping from `docs/methodsketch.txt`: `SI1 = MembershipManager`, `SI2 = MaintenanceManager`, `SI3 = NotificationManager`, `SI4 = TaskingManager`.
   - Confirm whether numbering should follow this exact manager order before applying bulk renames.

2. Renumber `Notify user` to match `NotificationManager` ownership.
   - Current: `SI2.1 Notify user`.
   - `methodsketch.txt` shows `Notify User` is owned by `NotificationManager`.
   - Suggested target: `SI3.1 Notify user`, assuming `SI3` maps to `NotificationManager`.
   - Update markdown filename/title, PlantUML filename/title, SVG/PNG exports, `system-interactions.md`, BPMN call activity names, and cross-references in other markdown files.

3. Renumber maintenance interactions to match `MaintenanceManager` ownership.
   - Current maintenance-owned interactions are mostly under `SI3.x`, but `MaintenanceManager` appears to be manager 2 in `methodsketch.txt`.
   - Suggested targets, pending numbering confirmation:
     - `SI3.1 Match maintenance plan offering` -> `SI2.x Match maintenance plan offering`.
     - `SI3.2 Create pending maintenance plan` -> `SI2.x Create pending maintenance plan`.
     - `SI3.3 Resolve pending maintenance plan` -> `SI2.x Resolve pending maintenance plan`.
     - `SI3.5 Cancel maintenance job` -> `SI2.x Cancel maintenance job`.
     - `SI3.6 Accept maintenance slots proposal` -> `SI2.x Accept maintenance slots proposal`.
     - `SI3.7 Create maintenance job slots proposal` -> `SI2.x Create maintenance job slots proposal`.
   - Preserve stable relative ordering if possible, but close gaps if the team wants a clean sequence.

4. Renumber `Initiate eSigning for maintenance plan` under `MaintenanceManager`.
   - Current: `SI5.1 Initiate eSigning for maintenance plan`.
   - `methodsketch.txt` shows the interaction is owned by `MaintenanceManager`, not by an `ESigningManager`.
   - There is no `ESigningManager`; only `ESigningAccess` and `ExternalESigningResource` exist.
   - Suggested target: a `SI2.x` ID in the MaintenanceManager sequence.
   - Update references in `BP1.3-enroll-in-maintenance-plan.bpmn`, markdown, PlantUML, generated exports, and any source references.

5. Keep `Create backoffice task` under `TaskingManager`, but review scope.
   - Current: `SI4.1 Create backoffice task`.
   - This appears consistent with `TaskingManager` ownership.
   - Verify whether all task creation should go through this interaction, including equipment registration review and provider cancellation notification.

6. Resolve inconsistent backoffice task creation ownership.
   - `methodsketch.txt` models task creation by queueing to `TaskingManager` in `Create Pending Registration`, `Create Pending Maintenance Plan`, and `Create Maintenance Job Slots Proposal`.
   - `si4.1-create-backoffice-review-task.md` says equipment registration review is started directly by `SI1.5` and cancellation notification is created directly by `SI3.5`.
   - Decide whether these should be formalized as `SI4.x Create backoffice task` cases or explicitly documented as exceptions.
   - If formalized, update BPMNs and interaction docs to show event-driven task creation through `TaskingManager`.

7. Add or clarify the missing maintenance slots confirmation interaction.
   - `methodsketch.txt` includes `Confirm Maintenance Provider Slots` and `Confirm Maintenance Slots Proposal`.
   - Current docs do not have a dedicated system interaction for confirming the slots proposal and producing `Maintenance slots proposal confirmed`.
   - `Notify user` consumes `Maintenance slots proposal confirmed`, so the producer should be explicit.
   - Decide whether this should be a MaintenanceManager interaction, a TaskingManager interaction, or two separate interactions.

8. Align `Resolve pending maintenance plan` activation flow.
   - `methodsketch.txt` models `Activate Signed Maintenance Plan` as a customer/client request to `MaintenanceManager`.
   - `si3.3-resolve-pending-maintenance-plan.md` models activation as consuming a signed-agreement event with DLQ failure handling.
   - `BP1.3-enroll-in-maintenance-plan.bpmn` shows a user signing flow followed by system resolution and returning an active maintenance plan.
   - Pick one model: synchronous customer-driven activation or asynchronous event-driven activation.
   - Update `methodsketch.txt`, BPMN, markdown, and PlantUML so they describe the same trigger and failure behavior.

9. Normalize notification event naming.
   - `Notify user` step table says it produces generic `User notification sent`.
   - The produced events section lists specific events such as `Activation notification sent`, `Accepted equipment registration notification sent`, and `Maintenance slots proposal notification sent`.
   - Choose one convention: one generic event with a typed payload or distinct event names per notification type.
   - Update markdown and PlantUML text accordingly.

10. Remove or restore orphaned `SI1.6 Load registration` diagram exports.
    - Existing files: `docs/business/system-interactions/diagrams/si1.6-load-registration.png` and `.svg`.
    - Missing files: no `si1.6-load-registration.md`, no `.pu`, and no `SI1.6` entry in `system-interactions.md`.
    - Either add the missing interaction docs if it is still needed or remove the orphaned generated exports.

11. Fill gaps between `methodsketch.txt` and system interaction docs.
    - The docs include onboarding interactions and cancellation interaction that are absent from `methodsketch.txt`.
    - `methodsketch.txt` includes some use cases not represented as system interaction markdown files.
    - Decide whether `methodsketch.txt` should be complete and authoritative.
    - If yes, add missing use cases for onboarding and cancellation, and reconcile or document folded use cases such as `Activate Signed Maintenance Plan`.

12. Update all affected cross-references after renumbering.
    - Search for old IDs in all docs, including markdown, `.pu`, `.bpmn`, SVG text, PNG export names, and index tables.
    - Update BPMN call activity names such as `SI2.1 Notify user`, `SI3.x ...`, and `SI5.1 ...`.
    - Update flow descriptions that reference the old IDs, for example markdown references to `SI2.1 Notify user`, `SI4.1 Create backoffice task`, and `SI5.1 Initiate eSigning for maintenance plan`.

13. Regenerate diagram exports after source changes.
    - Regenerate PlantUML `.svg` and `.png` files from the updated `.pu` files.
    - Regenerate or update BPMN image exports if the BPMN source labels change.
    - Confirm generated file names match the new interaction IDs.

14. Validate final consistency.
    - Verify every entry in `system-interactions.md` has a matching markdown file and PlantUML source file.
    - Verify every PlantUML title matches its markdown title and filename.
    - Verify every BPMN call activity points to an existing system interaction ID.
    - Verify the first number of every system interaction ID matches the owning manager from `methodsketch.txt`.
    - Verify no orphaned generated diagrams remain.
