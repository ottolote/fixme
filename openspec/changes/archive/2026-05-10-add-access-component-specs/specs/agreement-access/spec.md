## ADDED Requirements

### Requirement: Agreement filtering
`AgreementAccess` SHALL expose `Filter(AgreementCriteria): Agreement` for retrieving agreement evidence used by pending maintenance plan resolution. The access component SHALL delegate reads to `AgreementResource` and SHALL return agreements matching the supplied `AgreementCriteria`.

#### Scenario: Matching agreement is returned
- **WHEN** `Filter(AgreementCriteria)` is called with criteria matching an existing agreement by maintenance plan, customer, signature order reference, agreement identifier, or signed-evidence status
- **THEN** `AgreementAccess` returns the matching `Agreement` from `AgreementResource`

#### Scenario: Agreement is not found
- **WHEN** `Filter(AgreementCriteria)` is called with criteria matching no agreement
- **THEN** `AgreementAccess` returns no `Agreement` result

### Requirement: Agreement storage
`AgreementAccess` SHALL expose `Store(Agreement): Agreement` for creating maintenance plan agreements before eSigning and storing or linking signed agreement evidence after signature completion. The access component SHALL delegate writes to `AgreementResource` and SHALL return the persisted `Agreement` state.

#### Scenario: Agreement is stored
- **WHEN** `Store(Agreement)` is called with a valid maintenance plan agreement, generated document payload or reference, customer association, and signature state
- **THEN** `AgreementAccess` persists the `Agreement` through `AgreementResource` and returns the stored `Agreement`

#### Scenario: Agreement cannot be stored
- **WHEN** `Store(Agreement)` is called with missing plan, customer, document, signature state, or signed-evidence data required for its state
- **THEN** `AgreementAccess` rejects the store operation and does not persist a partial `Agreement`
