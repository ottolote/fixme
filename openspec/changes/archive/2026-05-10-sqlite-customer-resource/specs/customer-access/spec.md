## ADDED Requirements

### Requirement: SQLite-backed customer persistence
`CustomerAccess` SHALL persist and retrieve customers through a SQLite-backed `CustomerResource` while preserving customer lookup and storage behavior.

#### Scenario: Customer persists across access instances
- **WHEN** one `CustomerAccess` instance stores a customer through a SQLite-backed `CustomerResource` and another `CustomerAccess` instance uses the same resource database
- **THEN** the second instance can retrieve the stored customer by customer identifier, email, confirmation token, or profile reference

#### Scenario: Customer uniqueness remains enforced in SQLite
- **WHEN** `Store(Customer)` is called with a customer whose customer identifier, email, confirmation token, or profile reference conflicts with another stored customer
- **THEN** `CustomerAccess` rejects the store operation and does not persist a partial `Customer`

### Requirement: Customer registration eligibility
`CustomerAccess` SHALL expose customer state sufficient for membership workflows to validate whether a customer may start an equipment registration draft. A returned customer SHALL indicate registration permission when the customer's email is confirmed.

#### Scenario: Confirmed customer may register equipment
- **WHEN** `Filter(CustomerCriteria)` returns an existing customer with confirmed email
- **THEN** the returned customer indicates that the customer may register equipment

#### Scenario: Unconfirmed customer may not register equipment
- **WHEN** `Filter(CustomerCriteria)` returns a missing customer or an unconfirmed customer
- **THEN** the returned result does not indicate permission to register equipment

## MODIFIED Requirements

### Requirement: Customer filtering
`CustomerAccess` SHALL expose `Filter(CustomerCriteria): Customer` for customer lookup operations used by membership workflows. The access component SHALL delegate reads to SQLite-backed `CustomerResource` and SHALL return only customer records matching the supplied `CustomerCriteria`.

#### Scenario: Matching customer is returned
- **WHEN** `Filter(CustomerCriteria)` is called with criteria matching an existing customer by email, customer identifier, confirmation token, or profile reference
- **THEN** `CustomerAccess` returns the matching `Customer` from `CustomerResource` without modifying stored customer state

#### Scenario: Customer is not found
- **WHEN** `Filter(CustomerCriteria)` is called with criteria that match no customer
- **THEN** `CustomerAccess` returns no `Customer` result and does not synthesize placeholder customer data

#### Scenario: Customer criteria is invalid
- **WHEN** `Filter(CustomerCriteria)` is called with missing or contradictory lookup fields
- **THEN** `CustomerAccess` rejects the lookup as invalid without querying unrelated customer records

### Requirement: Customer storage
`CustomerAccess` SHALL expose `Store(Customer): Customer` for creating and updating customer records used by account registration, email confirmation, password update, preference update, and customer state used for equipment registration eligibility workflows. The access component SHALL delegate writes to SQLite-backed `CustomerResource` and SHALL return the persisted `Customer` state.

#### Scenario: New customer is stored
- **WHEN** `Store(Customer)` is called with a valid new unconfirmed customer, confirmation token data, or profile data
- **THEN** `CustomerAccess` persists the `Customer` through `CustomerResource` and returns the stored `Customer`

#### Scenario: Existing customer is updated
- **WHEN** `Store(Customer)` is called with a valid existing customer containing confirmation, password, token, or preference changes
- **THEN** `CustomerAccess` persists the updated `Customer` through `CustomerResource` without changing unrelated customer fields

#### Scenario: Customer cannot be stored
- **WHEN** `Store(Customer)` is called with missing required identity fields, invalid persisted state, or a conflicting unique customer identity
- **THEN** `CustomerAccess` rejects the store operation and does not persist a partial `Customer`
