# SI5.1 Initiate eSigning for maintenance plan

## Flow

Source: `BP1.3-enroll-in-maintenance-plan.bpmn` and `../references/use-cases/uc2.1-request-signatures.pu`.

After a maintenance plan request is approved, the system initiates eSigning. It validates that the pending maintenance plan can be signed, loads the plan, generates the maintenance plan agreement, requests signatures through the eSigning capability, stores the returned signature order reference, and returns the signature request. The BPMN then sends a signature request to the customer.

## Questions

- The BPMN labels this activity as `SI 5.1` with a space, while `system-interactions.md` lists `SI5.1`. Should the BPMN label be corrected?
- Which party signs the agreement besides the customer, if any?
- Does the eSigning reference model map directly to `../references/use-cases/uc2.1-request-signatures.pu`?
