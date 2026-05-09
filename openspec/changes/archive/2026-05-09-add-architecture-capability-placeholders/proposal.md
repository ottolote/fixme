## Why

The architecture sketch identifies the system components, but OpenSpec has no capability placeholders to organize future requirements around those component boundaries. Creating placeholder capabilities now gives future changes a stable spec location for each client, manager, engine, access, resource, and utility.

## What Changes

- Add placeholder OpenSpec capabilities for every component listed in `docs/methodsketch.txt`.
- Establish component-oriented spec directories without defining behavioral requirements yet.
- Omit implementation tasks because this change only introduces empty specification placeholders.

## Capabilities

### New Capabilities
- `customer-web-client`: Customer-facing web client capability placeholder.
- `backoffice-client`: Backoffice user client capability placeholder.
- `membership-manager`: Membership workflow orchestration capability placeholder.
- `maintenance-manager`: Maintenance workflow orchestration capability placeholder.
- `notification-manager`: Notification orchestration capability placeholder.
- `tasking-manager`: Backoffice task orchestration capability placeholder.
- `matching-engine`: Maintenance plan matching decision capability placeholder.
- `scheduling-engine`: Maintenance scheduling decision capability placeholder.
- `policy-engine`: Policy decision capability placeholder.
- `customer-access`: Customer persistence access capability placeholder.
- `equipment-access`: Equipment persistence access capability placeholder.
- `maintenance-access`: Maintenance persistence access capability placeholder.
- `task-access`: Task persistence access capability placeholder.
- `notification-access`: Notification persistence access capability placeholder.
- `agreement-access`: Agreement persistence access capability placeholder.
- `e-signing-access`: E-signing integration access capability placeholder.
- `customer-resource`: Customer resource capability placeholder.
- `equipment-resource`: Equipment resource capability placeholder.
- `maintenance-resource`: Maintenance resource capability placeholder.
- `task-resource`: Task resource capability placeholder.
- `notification-resource`: Notification resource capability placeholder.
- `agreement-resource`: Agreement resource capability placeholder.
- `external-e-signing-resource`: External e-signing service resource capability placeholder.
- `communication-utility`: Communication utility capability placeholder.
- `security-utility`: Security utility capability placeholder.
- `logging-utility`: Logging utility capability placeholder.
- `message-bus-utility`: Message bus utility capability placeholder.

### Modified Capabilities

None.

## Impact

- Adds OpenSpec change artifacts only.
- No code, APIs, dependencies, runtime behavior, or data contracts are changed.
