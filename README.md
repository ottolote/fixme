# FixMe

FixMe is a repair orchestration platform for onboarding customers and maintenance providers, scheduling repairs, handling agreements, and coordinating notifications and back-office tasks.

## Prerequisites

- .NET SDK 10.0.x
- Podman 5.x or Docker-compatible tooling

## Local Development

Run the API directly:

```sh
dotnet run --project Host/Api/FixMe.Api.csproj -- --url=http://127.0.0.1:8080
```

Run the local stack with Podman Compose:

```sh
podman compose up --build
```

Useful endpoints:

- `GET /health/live`
- `GET /health/ready`
- `GET /api/v1`

## Quality Gates

Run the same checks used by CI:

```sh
dotnet restore FixMe.slnx
dotnet format FixMe.slnx --verify-no-changes --no-restore
dotnet build FixMe.slnx --configuration Release --no-restore
dotnet test FixMe.slnx --configuration Release --no-build --collect:"XPlat Code Coverage"
```

Build the API image locally:

```sh
podman build --file Containerfile --tag fixme-api:local .
```

## Service Layout

The current repository contains the initial API host plus component/interface projects for the expected FixMe service areas:

- Membership
- Maintenance
- Notification
- Tasking

The API host is the external entry point. The existing manager, engine, and access projects are the internal component seams that future services can split around when the platform grows to 3-4 deployable services.
