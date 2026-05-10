## Purpose

The `MembershipManager` owns customer membership and equipment registration workflows documented by P2 SI1 interactions.
## Requirements
### Requirement: SI1.1 Register account
The `MembershipManager` SHALL expose `RegisterAccount(RegisterAccountRequest): RegisterAccountResponse` for preliminary customer account registration. The manager SHALL validate email format, look up existing customers with `CustomerAccess.Filter(CustomerCriteria)`, persist customer/token changes with `CustomerAccess.Store(Customer)`, and avoid account enumeration by returning the same accepted response for valid submissions whether or not a confirmed account already exists.

#### Scenario: New preliminary account is created
- **WHEN** a `RegisterAccountRequest` contains a valid email that is not associated with a confirmed customer account
- **THEN** the manager creates an unconfirmed customer account, creates a 24-hour confirmation token, persists it through customer access, returns `RegisterAccountResponse` as accepted, and emits or queues `preliminaryUserCreated` notification work through `NotifyUser(NotifyUserRequest): NotifyUserResponse`

#### Scenario: Existing unconfirmed account refreshes token
- **WHEN** a `RegisterAccountRequest` contains a valid email for an existing unconfirmed customer account
- **THEN** the manager invalidates the previous unused confirmation token, creates a replacement 24-hour token, persists the customer update, returns `RegisterAccountResponse` as accepted, and emits or queues `preliminaryUserCreated` notification work

#### Scenario: Confirmed account already exists
- **WHEN** a `RegisterAccountRequest` contains a valid email for an existing confirmed customer account
- **THEN** the manager returns the same accepted `RegisterAccountResponse` without creating a new account, refreshing tokens, or emitting preliminary-account notification work

#### Scenario: Email format is invalid
- **WHEN** a `RegisterAccountRequest` contains an invalid email address
- **THEN** the manager returns an invalid-email failure and does not store a customer or request notification work

### Requirement: SI1.2 Confirm user email
The `MembershipManager` SHALL expose `ConfirmUserEmail(ConfirmUserEmailRequest): ConfirmUserEmailResponse` for activation-link confirmation. The manager SHALL use `CustomerAccess.Filter(CustomerCriteria)` to resolve the token/customer, persist accepted confirmation with `CustomerAccess.Store(Customer)`, and return generic confirmation failures that do not reveal token or account existence.

#### Scenario: Valid token confirms email
- **WHEN** a `ConfirmUserEmailRequest` contains an existing, unexpired, unused token for an unconfirmed customer account
- **THEN** the manager marks the email confirmed, invalidates the confirmation token, persists the customer update, returns `ConfirmUserEmailResponse` as successful, and records `User email confirmed`

#### Scenario: Account is already confirmed
- **WHEN** a `ConfirmUserEmailRequest` references a token associated with an already-confirmed customer account
- **THEN** the manager returns `ConfirmUserEmailResponse` as successful without creating a new event or changing customer state

#### Scenario: Token cannot be accepted
- **WHEN** a `ConfirmUserEmailRequest` contains an unknown, expired, used, or unrelated token
- **THEN** the manager returns a generic confirmation failure without revealing which token condition failed

### Requirement: SI1.3 Update user password
The `MembershipManager` SHALL expose `UpdateUserPassword(UpdateUserPasswordRequest): UpdateUserPasswordResponse` for password setup and update. The manager SHALL validate that the request context is authorized for the target account, enforce password policy, retrieve the customer with `CustomerAccess.Filter(CustomerCriteria)`, hash the password with `SecurityUtility.HashPassword(HashPasswordRequest): HashResponse`, and persist the credential with `CustomerAccess.Store(Customer)`.

#### Scenario: Password is updated
- **WHEN** an authorized `UpdateUserPasswordRequest` contains a password with at least 12 characters that is not equal to the customer email
- **THEN** the manager hashes the password, stores the salted password hash, returns `UpdateUserPasswordResponse` as successful, records `User password updated`, invalidates other active sessions for password changes, and keeps the current session active

#### Scenario: Request is invalid or unauthorized
- **WHEN** an `UpdateUserPasswordRequest` fails request-context authorization or references no customer account
- **THEN** the manager returns an `invalid-request` or `account-not-found` failure without hashing or storing a password

#### Scenario: Password violates policy
- **WHEN** an `UpdateUserPasswordRequest` contains a password shorter than 12 characters or equal to the user email
- **THEN** the manager returns a `password-policy` failure without storing a password change

### Requirement: SI1.4 Set user preferences
The `MembershipManager` SHALL expose `SetUserPreferences(SetUserPreferencesRequest): SetUserPreferencesResponse` for customer profile preference updates. The manager SHALL validate name, email, phone number, and location fields, load the profile with `CustomerAccess.Filter(CustomerCriteria)`, and persist preferences with `CustomerAccess.Store(Customer)`.

#### Scenario: Preferences are stored
- **WHEN** a `SetUserPreferencesRequest` contains valid name, email, phone number, and location values for an existing customer profile
- **THEN** the manager stores the profile preferences, returns `SetUserPreferencesResponse` as successful, and records `User preferences set`

#### Scenario: Preference validation fails
- **WHEN** a `SetUserPreferencesRequest` contains an invalid name, email, phone number, or location
- **THEN** the manager returns the corresponding `invalid-name`, `invalid-email`, `invalid-phone-number`, or `invalid-location` failure without storing profile changes

#### Scenario: Profile does not exist
- **WHEN** a `SetUserPreferencesRequest` references no existing customer profile
- **THEN** the manager returns `profile-not-found` without storing preferences

### Requirement: SI1.5 Create draft registration
The `MembershipManager` SHALL create draft equipment registrations for customers permitted to register equipment. The manager SHALL expose `CreatePendingRegistration(CreatePendingRegistrationRequest)` for draft creation, SHALL validate customer eligibility with `CustomerAccess.Filter(CustomerCriteria)`, SHALL store draft `PendingRegistration` records with `EquipmentAccess.Store(PendingRegistration)`, and SHALL NOT directly create a backoffice task. The manager SHALL support creating drafts without equipment details or an equipment type. If the request includes an equipment type, the manager SHALL validate it with `EquipmentAccess.Filter(EquipmentTypeCriteria)` before storing the draft.

#### Scenario: Draft registration is created without equipment type
- **WHEN** a confirmed customer starts equipment registration without supplying an equipment type
- **THEN** the manager validates the customer, does not look up an equipment type, stores a draft or pending registration record using the equipment-registration persistence path, returns the draft registration, and records `Pending equipment registration created`

#### Scenario: Draft registration is created with valid equipment type
- **WHEN** a confirmed customer starts equipment registration with an equipment type that exists
- **THEN** the manager validates the customer, validates the equipment type, stores a draft or pending registration record using the equipment-registration persistence path, returns the draft registration, and records `Pending equipment registration created`

#### Scenario: Customer is invalid
- **WHEN** the draft registration request cannot be associated with a customer permitted to register equipment
- **THEN** the manager returns `invalid-customer` without storing a draft registration or validating equipment type

#### Scenario: Equipment type is invalid
- **WHEN** the draft registration request includes an equipment type that cannot be resolved
- **THEN** the manager returns `invalid-equipment-type` without storing a draft registration

#### Scenario: Draft creation does not create backoffice work
- **WHEN** a draft equipment registration is created
- **THEN** the manager does not call or enqueue `CreateBackofficeTask(CreateBackofficeTaskRequest): CreateBackofficeTaskResponse`

### Requirement: SI1.6 Submit registration
The `MembershipManager` SHALL submit an existing draft equipment registration for policy evaluation. The manager SHALL validate request shape, load the draft registration, invoke `PolicyEngine.ValidateEquipmentRegistration()`, and apply policy outcomes `valid`, `needs review`, or `invalid`.

#### Scenario: Registration is accepted by policy
- **WHEN** a valid submit-registration request references an existing draft and the policy engine returns `valid`
- **THEN** the manager creates registered equipment, marks the registration accepted, returns an accepted registration response, and records `Equipment registration accepted` for downstream user notification

#### Scenario: Registration requires manual review
- **WHEN** a valid submit-registration request references an existing draft and the policy engine returns `needs review`
- **THEN** the manager marks the draft registration pending review, returns a pending registration response, and records `Equipment registration review requested` for downstream `CreateBackofficeTask(CreateBackofficeTaskRequest): CreateBackofficeTaskResponse`

#### Scenario: Registration is rejected by policy
- **WHEN** a valid submit-registration request references an existing draft and the policy engine returns `invalid`
- **THEN** the manager returns the policy failure reason without creating registered equipment or requesting backoffice work

#### Scenario: Draft registration is missing
- **WHEN** a submit-registration request references no existing draft registration
- **THEN** the manager returns `draft-registration-not-found` without invoking policy or changing registration state

#### Scenario: Required equipment policy cannot be satisfied
- **WHEN** the submitted equipment type has no service provider or requires official registration/license validation that fails
- **THEN** the manager treats the policy result as invalid or review-required according to the policy engine response and does not directly bypass policy

### Requirement: SI1.7 Resolve pending registration
The `MembershipManager` SHALL expose `ResolvePendingRegistration(ResolvePendingRegistrationRequest): ResolvePendingRegistrationResponse` for backoffice decisions on pending equipment registrations. The manager SHALL load pending registrations with `EquipmentAccess.Filter(PendingRegistrationCriteria)` and persist accepted equipment with `EquipmentAccess.Store(Equipment)`.

#### Scenario: Pending registration is accepted
- **WHEN** a `ResolvePendingRegistrationRequest` contains an accept decision for an existing pending registration
- **THEN** the manager creates registered equipment, marks the registration accepted, returns `ResolvePendingRegistrationResponse` as successful, and emits or queues `Equipment registration accepted` notification work through `NotifyUser(NotifyUserRequest): NotifyUserResponse`

#### Scenario: Pending registration is rejected
- **WHEN** a `ResolvePendingRegistrationRequest` contains a reject decision for an existing pending registration with an optional reason
- **THEN** the manager stores the rejection reason when supplied, marks the registration rejected, returns `ResolvePendingRegistrationResponse` as successful, and records `Equipment registration rejected`

#### Scenario: Decision is invalid
- **WHEN** a `ResolvePendingRegistrationRequest` contains neither accept nor reject as the decision
- **THEN** the manager returns `invalid-decision` without changing registration state

#### Scenario: Registration is missing or already resolved
- **WHEN** a `ResolvePendingRegistrationRequest` references no pending registration or a registration that is already accepted or rejected
- **THEN** the manager returns `registration-not-found` or `already-resolved` without creating equipment

