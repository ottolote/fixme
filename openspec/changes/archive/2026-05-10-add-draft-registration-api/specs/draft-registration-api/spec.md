## ADDED Requirements

### Requirement: Create draft equipment registration endpoint
The FixMe API SHALL expose a versioned REST endpoint for SI1.5 draft equipment registration creation. The endpoint SHALL accept JSON with `CustomerId` and optional `EquipmentTypeId`, SHALL call `MembershipManager.CreatePendingRegistration(CreatePendingRegistrationRequest)`, SHALL return the created draft registration as JSON on success, and SHALL NOT directly create a backoffice task.

#### Scenario: Draft registration is created
- **WHEN** a client posts a valid draft registration request for a customer permitted to register equipment
- **THEN** the API calls the membership manager, returns HTTP 201, and includes the created draft registration JSON

#### Scenario: Customer is invalid
- **WHEN** the membership manager reports `invalid-customer`
- **THEN** the API returns a 4xx response with an error body and does not call access or resource components directly

#### Scenario: Equipment type is invalid
- **WHEN** the membership manager reports `invalid-equipment-type`
- **THEN** the API returns HTTP 400 with an error body

#### Scenario: Request JSON is invalid
- **WHEN** the request body cannot be parsed as a draft registration request
- **THEN** the API returns HTTP 400 without calling the membership manager
