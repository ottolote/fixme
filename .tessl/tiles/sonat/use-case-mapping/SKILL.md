---
name: use-case-mapping
description: >
  Extract and categorize use cases from codebases or requirements/interview notes.
  Use this skill whenever the user asks to "map use cases", "extract use cases",
  "what does this system do", "analyze codebase for use cases", "use case inventory",
  "distill use cases", "legacy system analysis", "find all use cases",
  "codebase behavior analysis", "map out legacy behavior", or "what are the use cases".
  Also trigger when reviewing a legacy system before migration, onboarding to an
  unfamiliar codebase, or preparing input for volatility-based decomposition.
  Produces categorized use case tables and call chain traces that feed directly
  into the volatility-analysis skill.
license: MIT
compatibility: General methodology skill — no external tooling required.
metadata:
  author: Ole Rabanal, Sonat Consulting Trondheim
  version: "1.0"
---

# Use Case Mapping

Extract, categorize, and document use cases from codebases or requirements.

- **Part 1** (this file): Core methodology — extraction workflows, categorization, output format
- **Code patterns**: [Extraction Guide](EXTRACTION_GUIDE_REFERENCE.md) — framework-specific entry points, hidden use cases
- **Examples**: [Worked Examples](WORKED_EXAMPLES.md) — five diverse systems across both extraction modes

---

## Core Directives

> **A use case is a unit of required behavior** — a sequence of activities that produces
> an observable result for an actor. Extract use cases by tracing entry points to their
> terminal effects, not by reading class names or entity headings.

> **Name by verb, not by noun.** "Look up product by identifier" — not "ProductController".
> "Import data relations from external source" — not "DataImport entity".

> **Scheduled jobs, event handlers, and background processes are use cases.**
> They are often the most critical ones — data pipelines, cache warming, batch exports,
> nightly syncs, cleanup jobs. A system with 9 REST endpoints and 2 scheduled jobs has
> 11 entry points, not 9. Never treat non-HTTP entry points as secondary.

> **This skill produces input for decomposition, not the decomposition itself.**
> The output is a categorized use case inventory. Architecture decisions belong to
> the volatility-analysis skill.

---

## Two Extraction Modes

### Mode A: From Code

Use when the input is a codebase (legacy system, unfamiliar service, microservice).

#### Step 0: Analyze System Type and Stack

Before extracting use cases, determine **what kind of system this is** and **what
technology stack it uses**. This tells you where to look for use cases and what kinds
of use cases to anticipate.

**System Identification:**

| Question | What to look for |
|---|---|
| What kind of system is this? | README, project structure, build files, entry points |
| What tech stack? | Build dependencies, frameworks, language version, runtime |
| What are its boundaries? | Inbound interfaces (APIs, UIs, consumers), outbound interfaces (clients, publishers) |
| What external systems does it talk to? | HTTP clients, database connections, message brokers, file systems |
| What data stores does it use? | Databases, caches, blob storage, queues, search indexes |

**System Archetype Detection:**

The archetype determines *where use cases live* and *what kinds of entry points to expect*.
Most systems match one primary archetype. Some are hybrids — when hybrid, apply entry
point patterns from all matching archetypes.

| Archetype | Use Cases Live In... |
|---|---|
| **Backend API service** | Controllers, message consumers, scheduled jobs |
| **Stored procedure system** | The stored procedures — procs ARE the use cases |
| **Frontend application** | Page routes, form submissions, button handlers, API calls out |
| **Monolith with UI** | Controller actions, background jobs, page flows |
| **Batch / ETL system** | Job definitions, step configurations, pipeline stages |
| **Event-driven system** | Message handlers, event processors, saga orchestrators |
| **CLI tool** | Commands and subcommands — each is a use case |
| **Mobile application** | Screens, navigation flows, gestures, background sync |
| **Serverless / FaaS** | Each function is a use case; triggers define the actor |
| **Desktop application** | Menu actions, toolbar buttons, shortcuts, background tasks |
| **Scheduler proxy** | Scheduled triggers that proxy calls to other services |

**Consumer-type dimension:** The same archetype behaves differently by consumer type.
**Machine-to-machine** services have tight contracts, few endpoints, Serve/Acquisition-heavy.
**UI backends** have 3-5x more endpoints (CRUD per entity, search variants, file handling),
Administration-heavy, and more variant explosion. **Mixed** services should be partitioned
by consumer during analysis. Adjust time estimates accordingly.

**Stack-based anticipation:** Use dependencies to prompt your search — message broker →
Acquisition/event use cases, scheduler → Operational/batch, email/SMS library → Notification.
Always verify against actual code.

Output: System boundary summary + identified archetype(s) + anticipated use case families.

#### Step 1: Find All Entry Points (Archetype-Aware)

Every way work enters the system is a potential use case. The archetype determines
where to look. **Always check all categories that apply to your archetype — not just
the obvious ones.**

**Count before you trace.** Before deep-diving into call chains (Step 2), do a quick
count of all entry points. This determines your analysis strategy:

| Entry Point Count | Strategy |
|---|---|
| **1–10** | Trace every entry point individually. Full call chain analysis is feasible. |
| **11–25** | Trace individually but group by controller/handler first. Look for variant patterns early — if one controller has 8 endpoints, check if they're variants before tracing all 8. |
| **26+** | Scan for variant clusters first. Identify which controllers/handlers have many endpoints, trace one representative per cluster, then confirm the pattern holds for variants. Budget extra time — large systems often have cross-cutting patterns (staging imports, N+1 calls, architecture violations) worth documenting as patterns rather than repeating per use case. |

The count also validates your archetype detection — a "Backend API service" with 40+
endpoints is likely a UI backend or monolith hybrid, not a focused microservice.

Use the [Extraction Guide](EXTRACTION_GUIDE_REFERENCE.md) for framework-specific entry
point patterns matching your archetype. Every archetype has detailed tables covering all
relevant entry point categories with search patterns.

**Key principles by archetype:**
- **Stored procedure systems:** The procs *are* the use cases — don't extract from the thin app layer
- **Frontend apps:** Group by user journey, not by component
- **Batch/ETL:** Each job/pipeline is a use case; steps are the call chain
- **Event-driven:** Message handlers are primary entry points, not REST controllers
- **CLI:** Each command/subcommand is a use case; behavior-changing flags are variants

**Transparent cache-miss loaders** (e.g., Hazelcast `MapLoader`, Caffeine `CacheLoader`,
read-through patterns) are **not separate use cases** but ARE data access paths for data
lineage (Step 7). Note them during discovery; trace in Step 7b. See
[Extraction Guide](EXTRACTION_GUIDE_REFERENCE.md) for ecosystem-specific examples.

#### Step 2: Trace Each Entry Point to Its Terminal Effect

For each entry point, follow the call chain down through layers:

```
Entry Point → Service/Business Layer → Data Access → Resource
```

Record:
- The **full call chain** (class.method at each layer)
- The **terminal effect** (what observable result does this produce?)
- **Variants** (conditional branches, optional parameters that change behavior)
- **Fallback paths** (error handling, degraded mode, cache miss paths)
- **External calls** (outbound HTTP, message publishing, file writes)
- **Unobservable terminal effects** (fire-and-forget calls via `@Async`, `CompletableFuture`,
  `Task.Run`, goroutines — the caller never sees errors or results. Flag these: the caller's
  use case has no feedback path, and the async work itself may need its own error handling)

**Recognize repeated cross-system patterns.** When the same operation sequence appears 3+
times, extract it as a **named pattern** and reference it in individual traces. Examples:
Clear-Insert-Complete (3-phase staging import), cache-aside with scheduled refresh,
enrich-on-read. Describe each pattern once in Cross-Cutting Concerns (Step 5) — the
replacement system can implement the pattern once.

#### Step 3: Extract the Behavioral Verb

Apply the "name by verb" directive from Core Directives.

| Bad (code-named) | Good (behavior-named) |
|---|---|
| `ProductSearchController.getById()` | Search product by identifier |
| `CacheLoader.refreshCache()` | Refresh product cache from database |

#### Step 4: Categorize by Behavioral Family

Apply the verb cluster table (see Categorization section below).

#### Step 5: Detect Cross-Cutting Concerns

Identify patterns spanning multiple use cases: caching strategies and lifecycle gaps,
fail-open/fail-closed decisions, resilience patterns, observability, security model,
consistency gaps, architecture violations (controllers calling repositories directly,
business logic in wrong layer), and dead code.

#### Step 6: Assess Use Case Activity Status

For each use case, determine status: **Active** (consumers calling it, production traffic),
**Dormant/Dead** (no callers, no tests), **Deprecated** (`@Deprecated`, removal TODOs),
**Disabled** (feature-flagged off, commented-out), **Dead chain** (only called by dead code).
Evidence: production config, traffic data, test coverage, deprecation markers, monitoring.

#### Step 7 (Optional): Data Lineage — When Designing a Replacement

> Include when the inventory is used to design a replacement system. Skip for
> behavioral understanding or onboarding only.

When replacing a system, you need both behavioral inventory (*what it does*) and data
lineage (*what data flows through it*) — data contracts are what consumers depend on.

**7a: Inventory Domain Models** — Catalog every model/DTO/entity with: class name, layer
(API/service/cache/DB/external/import), serialization boundary, use case references,
relationship to other models. Surface **model multiplicity** (N classes for M concepts).

**7b: Attribute-Level Lineage** — For each domain attribute, trace: source (DB column /
external field) → internal model field → cache representation → API response field name.
Build a lineage table. Look for: fetched-but-discarded fields, derived attributes,
channel-specific projections, name/type mismatches across layers, and inconsistent
derivation across code paths (same attribute derived differently per path).

**7c: Data Flow Matrix** — Show which use cases read/write which entities through which
resources (DB, cache, external API).

**7d: Key Data Findings** — Summarize findings affecting replacement: model multiplicity,
fetched-but-discarded data, derived attributes, inconsistent derivation, fail-open/closed
policies, channel projections, import-without-invalidation, stale entry accumulation.

---

### Mode B: From Requirements / Interview Notes

Use when the input is stakeholder interviews, feature lists, or requirements documents.

#### Step 1: Re-sort by Verb, Ignore the Noun

Requirements are organized by domain entity ("Properties", "Customers", "Pets").
Re-sort by behavioral verb:

- "Import properties", "Import floor plans", "Load from external register" → one verb family: **Acquire**
- "Search customer", "Search services", "Search history" → one verb family: **Serve**

#### Step 2: Collapse Same-Verb-Different-DTO

If the only difference is the data type, it is one family with DTO as variability:

- `Ingest (type A, type B, type C, type D)` → one family, not four
- `Lookup (by ID, by code, by name)` → one family, not three

#### Step 3: Split Domain Headings Hiding Multiple Families

A single entity heading often contains multiple behavioral families:

- "Properties" → Feed (import), Serve (lookup), Admin (manage), Tasking (case), Finance (lease)
- "Customer profile" → Serve (search), Admin (add/update/remove), Notification (schedule reminders)

Split by verb, not by entity.

#### Step 4: Name Families by Behavioral Theme

Use the verb cluster table. Name by what changes together, not by the data it acts on.

#### Step 5: Check for Hidden Scheduled and Event-Driven Use Cases

Requirements focus on user-facing behavior. Probe for time/event-triggered use cases:
nightly batch jobs, scheduled pipelines, event-driven reactions, periodic maintenance.

**Always ask:** "What happens when nobody is using the system? What runs at night?
What triggers when data changes? What needs to happen on a schedule?"

---

## Behavioral Verb Cluster Table

Use this table to **categorize** extracted use cases into families — not to rename them.
If the code says "Search", the use case name should say "Search", even though the table
lists "Lookup" first in that cluster. The table determines which *family* a use case
belongs to, not what verb to use in its name. Families align with IDesign volatility
decomposition — each family hints at a candidate volatility area.

| Verb Cluster | Family Name | Signals in Code / Requirements |
|---|---|---|
| Lookup, Filter, Search, Get, Load, Read | **Serve** | GET endpoints, query methods, find/filter in service layer |
| Ingest, Import, Receive, Sync, Upload, Poll | **Acquisition / Feed** | POST endpoints accepting external data, scheduled importers, file receivers, message consumers |
| Transform, Normalize, Enrich, Digest, Map, Convert | **Transform** | Mapper classes, enrichment services, format converters, ETL steps |
| Match, Resolve, Classify, Validate, Guess, Assign (data) | **Match / Resolution** | Matching engines, classifiers, validation services, AI/ML inference |
| Export, Share, Distribute, Publish, Push (data out) | **Distribution / Feed** | Outbound HTTP calls, export jobs, event publishers, file writers |
| Triage, Assign (work), Complete, Inspect, Schedule, Approve | **Tasking / Workflow** | Task/case management, state machines, approval chains, checklists |
| Onboard, Register, Manage, Configure, Create/Update/Delete (admin) | **Administration** | CRUD endpoints for config/management, settings, user management |
| Bill, Invoice, Pay, Credit, Adjust, Calculate price | **Finance** | Billing services, payment integrations, price adjustments, payout logic |
| Notify, Alert, Email, SMS, Push (notification) | **Notification** | Notification services, email/SMS senders, push dispatchers |
| Analyze, Report, Aggregate, Dashboard, Track | **Analysis** | Report endpoints, aggregation queries, dashboard data, trend analysis |
| Refresh, Reload, Warm, Invalidate, Ping, Health | **Operational** | Cache management, health checks, readiness probes, infra endpoints |

**Potential sub-concerns:** Signing/Document generation, Compliance/Regulatory reporting.
Whether these are distinct families depends on the volatility analysis.

---

## Output Format

### Raw Use Case Table

One row per entry point — no collapsing, no interpretation. This is the faithful
extraction of everything the system exposes.

| Column | Description |
|---|---|
| **#** | Sequential identifier (UC1, UC2, ...) |
| **Use Case** | Behavioral name (verb + object + qualifier) |
| **Family** | Behavioral family from verb cluster table |
| **Entry Point** | HTTP method + path, cron expression, message topic, etc. |
| **Actor(s)** | Who/what triggers this use case (role, system, scheduler) |
| **Terminal Effect** | What observable result is produced |
| **Status** | Active, Dormant, Deprecated, Disabled, Dead |
| **Notes** | Fallbacks, gaps, dependencies |

### Distilled Use Case Table

After the raw table, collapse entry points that are variants of the same use case —
they share the same controller/service, same behavioral pattern, and differ only in
identifier type, data source, or input shape.

| Column | Description |
|---|---|
| **#** | Distilled identifier (DUC1, DUC2, ...) |
| **Use Case** | Behavioral name (verb + object) — without per-variant qualifier |
| **Family** | Behavioral family from verb cluster table |
| **Variability Axis** | What differs between variants (e.g., identifier type, cache target, format) |
| **Variants** | Count and list of raw UC identifiers (e.g., "4 variants: UC1–UC4") |
| **Actor(s)** | Who/what triggers this use case |
| **Terminal Effect** | What observable result is produced |

> **Variant explosion warning:** If raw >> distilled (e.g., 10 raw → 6 distilled), flag it:
> *"Variant explosion: N entry points collapse to M use cases. Variability axis: [axis]."*

### Call Chain Trace (Mode A only)

For each use case, trace: Entry Point → Service/Business Layer → Data Access → Resource.
Record actor, entry point, call chain with class.method at each layer, terminal effect,
variants, and fallback paths. See [Extraction Guide](EXTRACTION_GUIDE_REFERENCE.md) for
notation format and layer annotations.

### Cross-Cutting Concerns Table

| Concern | Pattern | Where | Impact |
|---|---|---|---|
| Name | Description of the pattern | Which use cases / classes | Implications or risks |

### Family Summary

Summarize: system name, N entry points → M use cases in F families, family breakdown
with counts, external boundaries, notable patterns, gaps, and variant collapses.

### Data Lineage (when included — Step 7)

Add after Family Summary: **Domain Model Inventory**, **Attribute Lineage Table**,
**Data Flow Matrix**, and **Key Data Findings** (see Step 7 for column definitions).

---

## Distillation Traps — Checklist

Verify each extraction against these common mistakes:

- [ ] **Trap 1: Missing non-HTTP entry points.** Did you search for scheduled jobs, message consumers, event handlers, startup hooks, and background processes? A system with 9 REST endpoints and 3 scheduled jobs has 12 entry points.
- [ ] **Trap 2: Entity-as-family.** Are families named by verbs, not nouns? "Properties" hides Feed, Serve, Admin, Tasking, and Finance.
- [ ] **Trap 3: Variant explosion.** Did you collapse same-verb-different-DTO? `Lookup(by ID, by code, by name)` is one Serve family with identifier type as variability.
- [ ] **Trap 4: Missing canonical families.** Does the system notify? Generate reports? Have admin workflows? Import/export data? Check for Notification, Analysis, Admin, Feed.
- [ ] **Trap 5: Sub-flow as use case.** Does each use case produce an **observable result for an actor**? Validation, mapping, and logging are steps, not use cases.
- [ ] **Trap 6: Operational vs. business confusion.** Are cache refresh, health checks, and config reloads in the Operational family, separate from business families?
- [ ] **Trap 7: Analysis magnets.** Are you spending disproportionate time on large utility classes (file parsers, format converters)? They are *called by* use cases, not use cases themselves. Document as cross-cutting in Step 5. Exception: extract domain-specific business rules inside utilities as data lineage findings (Step 7).

---

## Relationship to Other Skills

**use-case-mapping** → **volatility-analysis** → **contract-factoring**

This skill produces categorized use case inventories (and optional data lineage).
The volatility-analysis skill consumes families to decompose into components.
The contract-factoring skill consumes components to design interfaces.

Family names intentionally align with volatility-analysis verb clusters, but the mapping
is not 1:1 — that determination belongs to the decomposition skill.
