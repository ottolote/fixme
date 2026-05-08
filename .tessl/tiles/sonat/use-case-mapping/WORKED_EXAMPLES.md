# Worked Examples

Reference file for the `use-case-mapping` skill. Shows extraction and distillation
across different system types and input sources.

---

## Example 1: DIS — Device Information System (From Requirements)

**Input source:** Interview notes and requirements, already verb-organized.
**Mode:** B (Requirements Distillation)

### Raw Use Cases (already well-structured)

| Raw Use Cases | Distilled Family | Core Verbs |
|---|---|---|
| Ingest IMEI (partner, manual), Ingest product info (partner, manual), Ingest CDR (internal), Ingest TAC+band (internal), Ingest migration, Ingest purchase orders | **Acquisition / Feed** | Ingest, Import, Receive |
| Digest IMEI, Digest product info, Digest TAC, Digest CDR, Digest migration | **Transform** | Digest, Normalize, Map |
| Match primary color with AI, Guess product info from TAC, Resolve missing attributes | **Match / Resolution** | Match, Guess, Resolve |
| Export product info to GCP, Export IMEI to GCP, Export to insurance partner | **Distribution / Feed** | Export, Share |
| Store device, Lookup device (by IMEI/MSISDN/serial), Lookup product (by IMEI/GTIN), Filter products (all/list/by TAC) | **Serve** | Store, Lookup, Filter |
| Cleanup staged CDR | **Operational** | Cleanup |
| Notify by email (acquisition failures, success) | **Notification** | Notify |

### Full Use Case Table

| # | Use Case | Family | Actor(s) | Terminal Effect | Notes |
|---|---|---|---|---|---|
| UC1 | Ingest IMEI from partner | Acquisition | Partner file, Manual entry | Raw IMEI data staged | Partner + manual correction variants |
| UC2 | Ingest product info from partner | Acquisition | Partner file, Manual entry | Raw product data staged | Partner + manual correction variants |
| UC3 | Ingest CDR | Acquisition | Internal system | Raw CDR data staged | Internal source only |
| UC4 | Ingest TAC + band info | Acquisition | Internal system | Raw TAC data staged | Internal source only |
| UC5 | Ingest migration data | Acquisition | Legacy Hermes DB | Raw IMEI/TAC/CDR/product staged | Bulk migration, multiple entity types |
| UC6 | Ingest purchase orders | Acquisition | Financial orderbook (Apple++) | Raw order data staged | Not prioritized, ingest only |
| UC7 | Cleanup staged CDR | Operational | Scheduler | Stale CDR data removed | Housekeeping |
| UC8 | Digest IMEI | Transform | Pipeline (after ingest) | Device records created/updated | IMEI raw -> device schema |
| UC9 | Digest product info | Transform | Pipeline (after ingest) | Product records created/updated | Product raw -> product schema |
| UC10 | Digest TAC | Transform | Pipeline (after ingest) | TAC records created/updated | TAC raw -> device schema |
| UC11 | Digest CDR | Transform | Pipeline (after ingest) | CDR records linked to devices | CDR raw -> device schema |
| UC12 | Digest migration data | Transform | Pipeline (after ingest) | All entity types digested | Bulk migration path |
| UC13 | Match primary color with AI | Match / Resolution | Pipeline (after digest) | Color attribute assigned | AI/ML inference |
| UC14 | Guess product info from TAC | Match / Resolution | Pipeline (after digest) | Product attributes inferred | Heuristic matching |
| UC15 | Resolve missing attributes | Match / Resolution | Pipeline (after digest) | Gaps filled in product records | Data quality improvement |
| UC16 | Export product info to GCP | Distribution | Scheduler / Trigger | Product data in BigQuery | GCP export pipeline |
| UC17 | Export IMEI to GCP | Distribution | Scheduler / Trigger | IMEI data in BigQuery | GCP export pipeline |
| UC18 | Export to insurance partner | Distribution | Scheduler / Trigger | Data sent to partner | May fall out of scope |
| UC19 | Store device | Serve | API consumer | Device record persisted | Write path |
| UC20 | Lookup device | Serve | API consumer | Device identifiers + attributes + dates | By IMEI, MSISDN, serial number; multiple response shapes |
| UC21 | Lookup product | Serve | API consumer | Product attributes | By IMEI, GTIN |
| UC22 | Filter products | Serve | API consumer | Product list | All, list of possible, by TAC |
| UC23 | Notify on acquisition outcome | Notification | Pipeline (on success/failure) | Email sent | Success and failure variants |

### Key Lessons

- DIS requirements were already verb-organized — rare. Most stakeholder notes are entity-organized.
- 6 Ingest variants are one Acquisition family with source as variability.
- 5 Digest variants are one Transform family with entity type as variability.
- Serve contains both read (Lookup, Filter) and write (Store) — watch for this splitting
  in volatility analysis; read and write paths may have different volatility profiles.
- 23 use cases collapse to 7 families.

---

## Example 2: Dog Patrol — Hundepatruljen (From Interview Notes)

**Input source:** Interview notes, entity-organized.
**Mode:** B (Requirements Distillation)

### Raw (Entity-Organized) to Distilled (Verb-Organized)

The raw notes were organized under entity headings: User, Customer, Pet profile, Products,
Employee courses, Contracts, Billing, Checklists, Task, Assets, Messaging, Reporting.

| Entity Heading | Raw Use Cases | Distilled Family | Why |
|---|---|---|---|
| User | Onboard, Offboard, Assign | **Administration** | User lifecycle management |
| Customer | Add pickup routines, Apply for membership, Register insurance, Waiting list, Communicate | **Administration** | Customer lifecycle; communicate → may also be Notification |
| Pet profile | Search, Add, Update, Remove | **Serve** (Search) + **Administration** (CRUD) | Split: Search is Serve, CRUD is Admin |
| Pet profile | Schedule notification to update (every 2 years) | **Notification** | Scheduled reminder — not Admin |
| Products/services | Search, Add, Update, Archive | **Serve** (Search) + **Administration** (CRUD) | Same split as Pet profile |
| Employee courses | Match, Search, Add, Update, Archive | **Match / Resolution** (Match) + **Administration** (CRUD) | Match certs to requirements is distinct behavior |
| Contracts | Create PDF, Create, Search, Update, Sign | **Administration** + potential **Signing** sub-concern | PDF generation and signing are distinct from CRUD |
| Billing | Invoice, Remove invoice, Price, CPI, Salary | **Finance** | Monetary operations |
| Checklists | Prospect, Delivery, Pickup, Pet Health, Routine, Feeding, Dietary | **Tasking / Workflow** | All are structured task sequences |
| Task | Triage, Assign, Aggregate, Create, Update, Remove, Notes, Location, Release order, Weather, Services, Progress, Activities, Templates, History | **Tasking / Workflow** | Core operational workflow — largest family |
| Pickup/delivery | Add point, Calculate routes, Search list | **Tasking / Workflow** | Sub-flow of the core task workflow |
| Assets | Add, Search, Aggregate, Remove, Update | **Administration** | Asset lifecycle CRUD |
| Messaging | Between users | **Notification** or **Tasking** | Could be task assignment instead of messaging |
| Reporting | Service insights, Ratings, Trends, Customer activities, Pet history | **Analysis** | Aggregation and reporting |
| Notify | Users (tasks, signing, reports), Clients | **Notification** | Who/when/what for notifications |

### Full Use Case Table

| # | Use Case | Family | Actor(s) | Terminal Effect |
|---|---|---|---|---|
| UC1 | Onboard/offboard user | Administration | Staff, Admin | User account created/deactivated |
| UC2 | Manage customer lifecycle | Administration | Staff | Customer registered, membership applied, insurance linked |
| UC3 | Manage pet profile | Administration | Staff, Customer | Pet record created/updated/removed |
| UC4 | Manage products and services | Administration | Admin | Service catalog updated |
| UC5 | Manage assets | Administration | Staff | Asset inventory updated |
| UC6 | Manage contracts | Administration | Staff, Customer | Contract created, signed, updated |
| UC7 | Search pets/services/history | Serve | Staff, Customer | Results returned |
| UC8 | Triage and assign tasks | Tasking / Workflow | Staff | Work item assigned to employee |
| UC9 | Execute pet service task | Tasking / Workflow | Employee | Task completed, checklist filled |
| UC10 | Manage pickup and delivery | Tasking / Workflow | Staff | Route calculated, schedule set |
| UC11 | Calculate billing and CPI | Finance | System, Admin | Invoice generated, prices adjusted |
| UC12 | Match employee certifications | Match / Resolution | System | Certified employees matched to required services |
| UC13 | Generate reports and insights | Analysis | Manager, Admin | Dashboards, trends, ratings aggregated |
| UC14 | Send notifications | Notification | System (triggered) | Email/push sent to users or clients |
| UC15 | Schedule pet profile reminders | Notification | Scheduler (every 2 years) | Reminder notification dispatched |

### Key Lessons

- 50+ raw use cases collapsed to 15 use cases in 7 families.
- **"Task" heading was the largest** — this is the core business workflow. Checklists,
  pickup/delivery, pet services are all sub-flows of Tasking.
- **Entity headings hid behavioral families.** "Pet profile" contained Serve + Admin + Notification.
- **Messaging is ambiguous** — could be Notification or Tasking. Needs stakeholder clarification.
  The interviewer noted: "Must dig more into that one."

---

## Example 3: Apex — Construction & Property Management (From Interview Notes)

**Input source:** Interview notes, mixed entity and verb organization.
**Mode:** B (Requirements Distillation)

### The "Properties" Heading Problem

The "Properties" section alone contained 5 different behavioral families:

| Raw under "Properties" | Actual Family | Why |
|---|---|---|
| Import (for finishing, for rental), Upload, Floor plans | **Acquisition / Feed** | Bringing data into the system |
| Load multiple, Load single and aggregate | **Serve** | Reading data out |
| Update, Add attributes, Add property owner/tenant | **Administration** | Managing property records |
| Create lease, Create deposit account | **Finance** | Monetary/contractual operations |
| Design property, Add upgrades, Load from NOBB, Pick upgrades, Approve order | **Tasking / Workflow** | Multi-step design workflow with approvals |

### Full Use Case Table

| # | Use Case | Family | Actor(s) | Terminal Effect | Notes |
|---|---|---|---|---|---|
| UC1 | Import properties and floor plans | Acquisition | Customer, Staff | Property data ingested | For finishing and rental |
| UC2 | Load external data from NOBB | Acquisition | System | House articles loaded from public register | External integration |
| UC3 | Manage users and customers | Administration | Apex staff, Customer | User onboarded, settings configured | Includes pricing, profile, logo |
| UC4 | Manage property records | Administration | Customer, Staff | Attributes updated, owners/tenants assigned | CRUD for property lifecycle |
| UC5 | Manage projects | Administration | Manager | Project created, properties mapped | RUH reporting included |
| UC6 | Manage construction site | Administration | Manager | Personnel assigned, check-in/out tracked | Site lifecycle management |
| UC7 | Look up properties | Serve | Customer, Staff | Property details returned | Single, multiple, aggregated |
| UC8 | List and manage property sales | Serve + Administration | Broker, Staff | Listings created, sales registered | Read + write combined |
| UC9 | Design property upgrades | Tasking / Workflow | Homeowner, Manager, Subcontractor | Upgrades selected, orders approved/declined | Multi-step: pick -> order -> approve -> verify |
| UC10 | Manage cases and checklists | Tasking / Workflow | Staff, Subcontractor | Case created, assigned, progressed, closed | Core case management workflow |
| UC11 | Execute move-in/move-out protocols | Tasking / Workflow | Staff | Protocol completed | Structured workflow with steps |
| UC12 | Execute eviction workflow | Tasking / Workflow | Staff | Eviction processed | Legal workflow |
| UC13 | Run inspections | Tasking / Workflow | Staff | Inspection completed, findings recorded | Befaring |
| UC14 | Generate and manage invoices | Finance | System, Staff | Invoice created, payment tracked | Scheduled generation, manual corrections |
| UC15 | Process CPI adjustments | Finance | System, Admin | Prices adjusted | Scheduled automatic + manual override |
| UC16 | Process payouts | Finance | Admin, System | Payments to landlord/broker prepared | Approval required |
| UC17 | Run credit check | Finance | System | Creditworthiness assessed | External integration |
| UC18 | Share transactions to accounting | Distribution | Scheduler (nightly) | Transactions sent to external systems | SOAP, REST, GraphQL targets |
| UC19 | Export statistics | Distribution | Staff, Admin | Data sent to SSB, reports generated | External regulatory |
| UC20 | Generate reports and dashboards | Analysis | Manager, Staff | Sales/project metrics, rental dashboard | Multiple report types |
| UC21 | Sign documents | Administration | Staff, Customer | Document signed | Single/multiple signing; potential sub-concern |
| UC22 | Upload files | Acquisition | Staff, Customer | File stored | PDF, CSV, DOC, Images |
| UC23 | Send notifications | Notification | System | Email, SMS, Push sent | Triggered by workflows |

### Key Lessons

- **"Properties" was the worst offender** — 5 families hiding under one entity noun.
- **Finance is substantial** — 4 distinct use cases with different triggers (scheduled, manual, external).
  This is a candidate for its own volatility in decomposition.
- **Distribution has two distinct patterns** — nightly batch to accounting systems (SOAP/REST/GraphQL)
  vs. regulatory export to SSB. Different volatility profiles.
- **Signing** appeared as a sub-concern under Administration. In this domain, signing mechanisms
  (BankID, manual, bulk) could warrant separate consideration during volatility analysis.
- 23 use cases in 8 families. Apex is the most complex of the examples.

---

## Example 4: Simple Accounting System (From Flat Requirements)

**Input source:** Flat numbered use case list in Norwegian, minimal structure.
**Mode:** B (Requirements Distillation)

### Raw (Numbered) to Distilled

The raw list used numbered groups (1.x, 2.x, 3.x, etc.) that mapped to domain processes,
not behavioral families.

| Raw Group | Raw Use Cases | Distilled Family | Why |
|---|---|---|---|
| 0 | Payment (Vipps integrations) | **Finance** | Payment processing |
| 1.x | Register/edit expense/income, Reconcile, Get status | **Finance** (Register/Reconcile) + **Serve** (Get status) | Split: mutation vs. query |
| 2.x | Get shareholder overview | **Serve** | Read-only query |
| 3.x | Register flag for delivery, Generate/get/edit/send shareholder register report | **Compliance / Regulatory** | Regulatory submission to Altinn |
| 4.x | Generate/get/edit/send tax return | **Compliance / Regulatory** | Regulatory submission |
| 5.x | Generate/get/edit/send annual accounts, Get company info from Brreg | **Compliance / Regulatory** + **Acquisition** (Brreg) | Regulatory submission + external data fetch |
| 7.x | Send notification via SMS, email | **Notification** | Notifications |
| 8.x | Register/remove user, Change subscription | **Administration** | User lifecycle |

### Full Use Case Table

| # | Use Case | Family | Actor(s) | Terminal Effect | Notes |
|---|---|---|---|---|---|
| UC1 | Register and reconcile transactions | Finance | Accountant | Expense/income recorded, reconciled | File upload for receipts; share purchases/sales |
| UC2 | Process payments | Finance | System (Vipps) | Payment executed | External integration |
| UC3 | Look up accounting status | Serve | Accountant | Reconciliation status returned | What is reconciled vs. not |
| UC4 | Look up shareholder overview | Serve | Accountant | Shareholder list returned | Read-only |
| UC5 | Manage shareholder register report | Compliance | Accountant | Report generated, edited, submitted to Altinn | Generate -> Edit -> Sign -> Submit |
| UC6 | Manage tax return | Compliance | Accountant | Tax return generated, edited, submitted | Same lifecycle as UC5 |
| UC7 | Manage annual accounts | Compliance | Accountant | Annual accounts generated, submitted | Includes company info fetch from Brreg |
| UC8 | Fetch company info from Brreg | Acquisition | System | Company data loaded from public register | External integration, requires approval |
| UC9 | Manage users and subscriptions | Administration | Admin | User registered/removed, subscription changed | Basic user lifecycle |
| UC10 | Send notifications | Notification | System | SMS/email sent | Triggered on submission completion |

### Key Lessons

- Numbered groups (3.x, 4.x, 5.x) were **three instances of the same behavior**: Generate -> Edit -> Submit
  regulatory report. One Compliance family with report type as variability.
- Several items marked "gjores heller manuelt" (done manually instead) — these are
  **dormant use cases**. The system was designed for them but they are handled manually.
  Important to flag in activity assessment.
- The smallest system in the examples: 10 use cases in 6 families.
- **Compliance / Regulatory** is a clear sub-concern here — regulatory submission to Altinn
  has its own lifecycle (generate, edit, sign, submit) distinct from normal Finance operations.

---

## Example 5: Hermes GTIN Service (From Code Extraction)

**Input source:** Java/Spring Boot codebase.
**Mode:** A (Code Extraction)
**Purpose:** Replacement system design (data lineage included).

### System Boundary

| Aspect | Finding |
|---|---|
| Archetype | Backend API Service + Stored Procedure hybrid |
| Stack | Java 21 + Kotlin, Spring Boot 3.5, Jetty, Sybase ASE (via jconn4), Hazelcast 5.3 (embedded) |
| Entry points | 4 REST controllers (9 endpoints + 1 health), 2 scheduled cron jobs, 1 startup hook |
| Data stores | Sybase IMEI DB (9 stored procedures), Hazelcast (2 in-process caches) |
| External dependencies | TAC Cache Service (outbound REST for model/manufacturer enrichment) |
| Auth model | Spring Security + Basic Auth, 6 role constants |

### Raw Use Case Table

| # | Use Case | Family | Entry Point | Actor(s) | Terminal Effect | Status | Notes |
|---|---|---|---|---|---|---|---|
| UC1 | Get GTIN config by IMEI | Serve | `GET /gtin-configurations/imei/{imei}` | Channel consumers | GtinInfo returned from cache (or DB fallback) | Active | Optional TAC Cache enrichment; optional manufacturer/deviceType post-filter |
| UC2 | Get default GTIN config by IMEI or TAC | Serve | `GET /gtin-configurations/default/imei/{imei}` | Default consumers | List\<GtinInfo\> — GTIN match or priority-based TAC fallback | Active | Falls back to TAC-GTIN priority cache when no GTIN found |
| UC3 | Get GTIN config by IMEI for Moox | Serve | `GET /gtin-configurations/Moox/imei/{imei}` | Insurance channel | GtinInfoV2 (includes terminalValue) | Active | Uses Moox-specific stored proc; validates GTIN via `isGtinValid` field |
| UC4 | Get GTIN config by GTIN number | Serve | `GET /gtin-configurations/gtin-number/{gtinNumber}` | GTIN searchers | GtinInfo returned from cache (or DB fallback) | Active | Simplest search path — direct cache key lookup |
| UC5 | Get all GTIN configurations | Serve | `GET /gtin-configurations/all` | Report readers | Full List\<GtinInfo\> (entire catalog) | Active | Large dataset warning |
| UC6 | Update cache for specific GTINs | Operational | `POST /gtin-cache/config/gtin-numbers` | Cache operators | Specific cache entries refreshed from DB | Active | Selective cache update via admin UI |
| UC7 | Refresh GTIN config cache | Operational | `POST /gtin-cache/config/admin` | Admin | Entire GTIN cache rebuilt from DB | Active | Manual trigger via REST |
| UC8 | Refresh GTIN config cache (scheduled) | Operational | `@Scheduled cron` | Scheduler | Entire GTIN cache rebuilt from DB | Active | Same behavior as UC7, triggered by cron |
| UC9 | Refresh TAC-GTIN priority cache | Operational | `POST /gtin-cache/priority/admin` | Admin | Priority cache cleared and rebuilt from DB | Active | Manual trigger via REST |
| UC10 | Refresh TAC-GTIN priority cache (scheduled) | Operational | `@Scheduled cron` | Scheduler | Priority cache cleared and rebuilt from DB | Active | Same behavior as UC9, triggered by cron |
| UC11 | Warm both caches on startup | Operational | `@EventListener(ApplicationReadyEvent.class)` | Application lifecycle | Both caches loaded | Active | Runs once at boot; excluded in test profile |
| UC12 | Import GTIN-IMEI relations | Acquisition | `POST /gtin-imports` | External sources | GTIN-IMEI stored in Sybase | Active | No cache invalidation after import |
| UC13 | Ping (health check) | Operational | `GET /api/ping` | Monitoring | Returns "ack" | Active | Verifies authentication is working |

### Distilled Use Case Table

> **Warning: Variant explosion detected.** 13 entry points collapse to **7 distinct use
> cases**. The variability axes are identifier type, channel, cache target, and trigger
> mechanism. This is important input for decomposition — the variants share volatility
> and likely belong in the same component.

| # | Use Case | Family | Variability Axis | Variants | Actor(s) | Terminal Effect |
|---|---|---|---|---|---|---|
| DUC1 | Get GTIN config by identifier | Serve | Identifier type (IMEI, GTIN number), channel (default, Moox), fallback strategy | 4 variants: UC1, UC2, UC3, UC4 | Channel consumers, default consumers, Moox | GtinInfo / GtinInfoV2 |
| DUC2 | Get all GTIN configurations | Serve | *(none)* | 1 entry point: UC5 | Report readers | Full catalog dump |
| DUC3 | Update cache for specific GTINs | Operational | *(none)* | 1 entry point: UC6 | Cache operators | Selective cache entries refreshed |
| DUC4 | Refresh GTIN config cache | Operational | Trigger mechanism (REST, cron, startup) | 3 variants: UC7, UC8, UC11 | Admin, Scheduler, App lifecycle | Entire GTIN cache rebuilt |
| DUC5 | Refresh TAC-GTIN priority cache | Operational | Trigger mechanism (REST, cron, startup) | 3 variants: UC9, UC10, UC11 | Admin, Scheduler, App lifecycle | Priority cache cleared and rebuilt |
| DUC6 | Import GTIN-IMEI relations | Acquisition | *(none)* | 1 entry point: UC12 | External sources | GTIN-IMEI stored in Sybase |
| DUC7 | Ping (health check) | Operational | *(none)* | 1 entry point: UC13 | Monitoring | "ack" returned |

### Call Chain Traces (abbreviated — 3 of 13 shown)

```
UC1: Get GTIN config by IMEI
  Actor: Channel consumer
  Entry: GET /gtin-configurations/imei/{imei}
  Chain:
    GtinSearchController.getGtinConfigByImei()              [controller:51]
    → GtinSearchServiceImpl.getGtinConfigByImei()           [service:47]
        → GtinRepository.getGtinByImei()                    [db: DVSg_GetGtinbyIMEI]
        → GtinCacheRepository.getGtinConfig()               [cache: Hazelcast gtin-cache]
        → TACCacheServiceAdapter.getModelFromTACCache()     [external: TAC Cache REST] (conditional)
  Terminal Effect: GtinInfo {gtinNumber, manufacturer, model, color, memorySize, memorySuffix, deviceType}
  Variants:
    - isPureGTINSearch=true  → cache only, no external call
    - isPureGTINSearch=false → if model null, calls TAC Cache for model/manufacturer
    - if model contains "watch" → deviceType = "Wearable" (derived attribute)
    - manufacturer/deviceType params → post-filter, 404 if mismatch
  Fallback: On HazelcastException → GtinRepository.getGtinConfigByGtinNumber()

UC7+UC8: Refresh GTIN config cache (manual + scheduled)
  Actor: Admin OR Scheduler (cron)
  Entry: POST /gtin-cache/config/admin | @Scheduled(cron)
  Chain:
    GtinCacheController.refreshGtinConfigCache() | HazelcastCacheLoader.refreshGTINCache()
    → GtinCacheServiceImpl.refreshGtinConfigCache()         [service:30]
        → GtinCacheRepository.loadGTINsIntoCache()          [cache+db]
            → GtinRepository.getAllGtins()                   [db: DVSg_GtinconfigLoadCache]
            → Hazelcast IMap.set() × N                      [cache: full overwrite]
  Terminal Effect: Entire GTIN cache rebuilt

UC12: Import GTIN-IMEI relations
  Actor: External source admin
  Entry: POST /gtin-imports
  Chain:
    GtinImportController.importGtin()                       [controller:36]
    → GtinRepository.importGtin() × N                      [db: DVSg_IMEI_GTIN_IMPORT_Stg per item]
  Terminal Effect: GTIN-IMEI associations stored in Sybase
  Note: No cache invalidation after import — data invisible until next cache refresh
```

### Cross-Cutting Concerns

| Concern | Pattern | Where | Impact |
|---|---|---|---|
| Cache-first with DB fallback | Every Serve use case tries Hazelcast first; on `HazelcastException` falls back to stored procedure | UC1–UC5 | Resilience at cost of duplicated code paths per use case |
| Scheduled cache warming | Daily full reload from DB; no incremental invalidation | UC7/UC8, UC9/UC10 | Staleness window up to 24h; priority cache does `clear()` + rebuild (brief empty-cache window) |
| Import-cache gap | UC12 writes to DB but does not invalidate cache | Between import controller and cache service | Imported data invisible until next scheduled refresh |
| External enrichment on read path | UC1 conditionally calls TAC Cache REST API via deprecated `GenericRestClient` | `TACCacheServiceAdapter` | Adds latency + external dependency; swallows exceptions |
| AOP observability | Timing + logging for all REST controller and JDBC methods | Timing aspects | Good observability; potential PII exposure in JDBC logging |
| Dead code | MapLoader stubs return null; `GenericRestClient` deprecated but used | Hazelcast config, TAC adapter | Maintenance noise |

### Exhaustive Entry Point Audit

| Category | Count | Details |
|---|---|---|
| REST endpoints (business) | 9 | 5 search, 3 cache, 1 import |
| REST endpoints (health) | 1 | PingController |
| Scheduled cron jobs | 2 | GTIN cache refresh, TAC-GTIN priority cache refresh |
| Startup hooks | 1 | `@EventListener(ApplicationReadyEvent.class)` — loads both caches |
| `@PostConstruct` | 2 | Hazelcast map initialization (infrastructure, not use cases) |
| AOP aspects | 2 | REST timing + JDBC timing (cross-cutting, not use cases) |
| Dead MapLoader stubs | 2 | Implement interface but return null |
| Message consumers | 0 | None |
| **Total entry points** | **13** | 10 REST + 2 scheduled + 1 startup |
| **Total distinct use cases** | **7** | After variant collapse |

### Stored Procedure Inventory

| Stored Procedure | Called By | Purpose |
|---|---|---|
| `DVSg_GtinconfigLoadCache` | `GtinRepository.getAllGtins()` | Load all GTIN configs for cache warming |
| `DVSg_GetGtinbyIMEI` | `GtinRepository.getGtinByImei()` | Resolve IMEI → GTIN number |
| `DVSg_GetGtinConfigbyNumber` | `GtinRepository.getGtinConfigByGtinNumber()` | Get GTIN config by number (fallback) |
| `DVSg_MooXgetGtinInfoByIMEI` | `GtinRepository.getGtinByImeiMoox()` | Moox-specific IMEI → GTIN with validation |
| `DVSg_MooXGetGtinConfigbyNumber` | `GtinRepository.getGtinConfigTerminalValue()` | GTIN config with terminal value (Moox fallback) |
| `DVSg_TacGtinpriorityLoad` | `GtinRepository.getAllTacGtinPriorities()` | Load all TAC-GTIN priority mappings |
| `DVSg_TacGtinpriority` | `GtinRepository.getDefaultGtinFromDB()` | Resolve TAC → default GTIN (DB fallback) |
| `DVSg_UpdateGtinCacheOnDemand` | `GtinRepository.getAllGtins(List)` | Fetch specific GTIN configs for selective update |
| `DVSg_IMEI_GTIN_IMPORT_Stg` | `GtinRepository.importGtin()` | Import/stage GTIN-IMEI relationship |

### Data Lineage (abbreviated)

**Domain Model Inventory:** 7 model classes for 2 domain concepts. Key finding: `GtinInfoCache`,
`GtinInfo`, and `GtinInfoV2` carry the same core GTIN attributes — the only difference is
`terminalValue` visibility and `Serializable` support. Candidate for unification in replacement.

**Key Data Findings (top 5):**

1. **Model multiplicity (3 classes for 1 concept)** — unify to single domain entity with channel-based projection.
2. **Fetched but discarded: `eid`** — proc `DVSg_GetGtinbyIMEI` returns `eid` OUT param but code never reads it. Decision needed.
3. **Derived attribute: `deviceType`** — set to "Wearable" if model name contains "watch". Business rule embedded in service code, not in data.
4. **Channel-specific projection: `terminalValue`** — stored in cache for all GTINs but only exposed to Moox consumers. Access control concern, not data model concern.
5. **Import-without-invalidation** — UC12 writes to DB but doesn't update cache. Imported data invisible for up to 24h.

(Full data lineage with attribute-level tables and data flow matrix is in the complete report.)

### Key Lessons

- **13 raw → 7 distilled catches variant explosion.** The two-table format makes this
  visible: UC1–UC4 are all "get GTIN config" with identifier type and channel as variability.
  UC7–UC10 are "refresh cache" with cache target and trigger mechanism as variability.
  UC11 (startup) collapses into both DUC4 and DUC5.
- **Startup hooks are separate entry points.** `@EventListener(ApplicationReadyEvent.class)`
  loads both caches on boot — a distinct entry point from the scheduled and manual triggers,
  even though the behavior overlaps.
- **Stored procedure hybrid requires proc inventory.** The procs reveal the actual business
  operations. Without the proc inventory, you miss that 9 procs back 13 entry points and
  that naming conventions (`DVSg_GetGtinbyIMEI`, `DVSg_MooXgetGtinInfoByIMEI`) encode
  channel-specific logic.
- **Data lineage is essential for replacement design.** The behavioral inventory tells you
  *what* the system does (7 use cases in 3 families). The data lineage tells you *what
  data flows through it* — the 3:1 model multiplicity, the fetched-but-discarded `eid`,
  the derived `deviceType`, and the channel-specific `terminalValue` projection. You need
  both to design the replacement.
- **Cache management is Operational, not Administration.** Cache refresh/update is
  infrastructure lifecycle, not business entity management.
- **Import-cache gap is surfaced by call chain tracing.** UC12 writes to DB without
  invalidating cache — imported data invisible until next daily refresh. This kind of
  consistency gap only becomes visible when you trace the full call chain.

### Summary

```
Hermes GTIN Service
  13 entry points → 7 distinct use cases in 3 families (all active)
  |-- Serve (2)          -- get GTIN config by identifier (4 variants: IMEI, default/TAC, Moox, GTIN number)
  |                         get all GTIN configurations (catalog dump)
  |-- Operational (4)    -- update cache for specific GTINs
  |                         refresh GTIN config cache (3 variants: REST, cron, startup)
  |                         refresh TAC-GTIN priority cache (3 variants: REST, cron, startup)
  |                         health check (ping)
  |-- Acquisition (1)    -- import GTIN-IMEI relations from external source

  3 external boundaries: Sybase IMEI DB (9 stored procs), Hazelcast (2 caches), TAC Cache REST API
  1 key pattern: cache-first with DB fallback across all Serve use cases
  1 notable gap: import does not invalidate cache — imported data invisible until next refresh
  1 risk: priority cache does clear() before rebuild (brief empty-cache window)

  Variant collapses:
    - 4 search endpoints → 1 use case (variability: identifier type + channel)
    - 3 GTIN cache refresh triggers → 1 use case (variability: trigger mechanism)
    - 3 priority cache refresh triggers → 1 use case (variability: trigger mechanism)
```
