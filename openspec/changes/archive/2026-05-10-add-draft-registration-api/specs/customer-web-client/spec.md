## ADDED Requirements

### Requirement: Customer web client can start draft equipment registration
The customer web client API surface SHALL provide a versioned operation for starting SI1.5 draft equipment registration. The operation SHALL accept a customer identifier and optional equipment type identifier and SHALL return the draft registration created by membership management.

#### Scenario: Customer starts draft registration
- **WHEN** a customer clicks register equipment before submitting equipment details
- **THEN** the client-facing API accepts the request and returns the created draft registration

#### Scenario: Invalid customer cannot start draft registration
- **WHEN** the customer cannot be validated or is not permitted to register equipment
- **THEN** the client-facing API returns a 4xx failure response instead of creating a draft registration
