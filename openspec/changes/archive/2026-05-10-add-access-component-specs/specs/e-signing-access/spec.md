## ADDED Requirements

### Requirement: Signature request storage
`ESigningAccess` SHALL expose `SignatureRequestStore(SignatureRequest): SignatureRequest` for initiating external customer signature requests. The access component SHALL delegate the request to `ExternalESigningResource` and SHALL return the accepted provider-backed `SignatureRequest` state, including the external signature order reference when one is issued.

#### Scenario: Signature request is accepted
- **WHEN** `SignatureRequestStore(SignatureRequest)` is called with a valid customer signer, agreement document reference or payload, callback/correlation data, and maintenance plan context
- **THEN** `ESigningAccess` sends the request to `ExternalESigningResource` and returns the stored `SignatureRequest` with the external signature order reference and accepted state

#### Scenario: Signature request is rejected
- **WHEN** `SignatureRequestStore(SignatureRequest)` is called with missing signer data, missing agreement document data, invalid callback/correlation data, or data rejected by `ExternalESigningResource`
- **THEN** `ESigningAccess` returns a rejected or failed `SignatureRequest` outcome without reporting a successful signature order reference

#### Scenario: External eSigning resource is unavailable
- **WHEN** `SignatureRequestStore(SignatureRequest)` cannot complete because `ExternalESigningResource` is unavailable or times out
- **THEN** `ESigningAccess` reports the request as not accepted so the calling manager can nack or retry according to its workflow
