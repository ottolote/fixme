# SI5.1 Initiate eSigning for maintenance plan

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn` and `../references/use-cases/uc2.1-request-signatures.pu`.

After a maintenance plan request is approved, the system initiates eSigning. It validates that the pending maintenance plan can be signed, loads the plan, generates the maintenance plan agreement, requests a single customer signature through the eSigning capability, stores the returned signature order reference, and returns the signature request. The BPMN then sends a signature request to the customer.

## Questions

- Does the eSigning reference model map directly to `../references/use-cases/uc2.1-request-signatures.pu`?
- Is the signed agreement document stored by this interaction, or only later by `SI3.3 Resolve pending maintenance plan` after the customer signs?
