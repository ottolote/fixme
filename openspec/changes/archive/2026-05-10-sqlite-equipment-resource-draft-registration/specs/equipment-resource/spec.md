## ADDED Requirements

### Requirement: SQLite equipment persistence
`EquipmentResource` SHALL persist equipment types, registered equipment, and pending registrations using SQLite-owned storage. The resource SHALL NOT call other resources and SHALL NOT declare foreign keys to other resources; cross-resource identifiers such as `CustomerId` SHALL be stored as weak references.

#### Scenario: Equipment data survives access instance replacement
- **WHEN** equipment, equipment type, or pending registration data is stored through one `EquipmentAccess` instance backed by an `EquipmentResource` SQLite database
- **THEN** another `EquipmentAccess` instance using the same `EquipmentResource` SQLite database can retrieve the stored data

#### Scenario: Resource schema remains independent
- **WHEN** `EquipmentResource` initializes its SQLite schema
- **THEN** the schema stores weak reference IDs without declaring foreign keys to customer or other resource-owned tables
