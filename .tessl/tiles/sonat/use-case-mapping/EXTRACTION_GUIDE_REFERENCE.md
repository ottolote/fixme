# Extraction Guide Reference

Detailed patterns for extracting use cases from code. Companion to the `use-case-mapping` skill.

Organized by system archetype — find your archetype, then use the entry point tables
and hidden use case checklist for that archetype.

---

## Archetype: Backend API Service

The most common archetype. REST/GraphQL controllers, no UI, serves other systems or frontends.

### Entry Point Patterns by Framework

**Spring / Spring Boot (Java, Kotlin):**

| Entry Point Type | Search Pattern | Notes |
|---|---|---|
| REST endpoints | `@RestController`, `@Controller`, `@GetMapping`, `@PostMapping`, `@PutMapping`, `@DeleteMapping`, `@RequestMapping` | Check `@RequestMapping` at class level for base path |
| Scheduled jobs | `@Scheduled(cron=...)`, `@Scheduled(fixedRate=...)`, `@Scheduled(fixedDelay=...)` | Often in `@Component` or `@Service` classes |
| Startup hooks | `@EventListener(ApplicationReadyEvent.class)`, `CommandLineRunner`, `ApplicationRunner`, `@PostConstruct` | Cache warming, migration scripts, initial data loads |
| Event listeners | `@EventListener`, `@TransactionalEventListener`, `ApplicationListener<T>` | Domain events, Spring events |
| Message consumers | `@KafkaListener`, `@RabbitListener`, `@JmsListener`, `@StreamListener` | Check topic/queue names for clues about purpose |
| Async handlers | `@Async`, `CompletableFuture` return types | Background processing triggered by sync entry points |
| WebSocket | `@MessageMapping`, `SimpMessagingTemplate` | Real-time communication channels |
| GraphQL | `@QueryMapping`, `@MutationMapping`, `@SchemaMapping` | Spring for GraphQL |

**Ktor (Kotlin):**

| Entry Point Type | Search Pattern |
|---|---|
| REST endpoints | `routing { }`, `get()`, `post()`, `put()`, `delete()`, `route()` |
| Scheduled jobs | `launch { }` with `delay()` or `ticker()` in application module |
| Startup hooks | `environment.monitor.subscribe(ApplicationStarted)` |
| WebSocket | `webSocket()` route handler |

**Express / Fastify / Koa / NestJS (Node.js):**

| Entry Point Type | Search Pattern |
|---|---|
| REST endpoints | `app.get()`, `app.post()`, `router.get()`, `router.post()`, route files, `@Get()`, `@Post()` (NestJS) |
| Middleware | `app.use()` — may contain business logic (auth, validation) |
| Scheduled jobs | `node-cron`, `bull`, `agenda`, `bree`, `setInterval` |
| Message consumers | `kafkajs consumer.run()`, `amqplib channel.consume()`, `@MessagePattern()` (NestJS) |
| Event handlers | `EventEmitter.on()`, custom event bus patterns, `@EventPattern()` (NestJS) |

**Django / Flask / FastAPI (Python):**

| Entry Point Type | Search Pattern |
|---|---|
| REST endpoints | `urlpatterns`, `@app.route()`, `@app.get()`, `ViewSet`, `APIView` |
| Scheduled jobs | Celery `@task`, `django-crontab`, `APScheduler`, `@repeat_every` (FastAPI) |
| Management commands | `BaseCommand` subclasses in `management/commands/` |
| Signal handlers | `@receiver(post_save)`, `Signal.connect()` |

**Go (net/http, Gin, Echo, Fiber):**

| Entry Point Type | Search Pattern |
|---|---|
| REST endpoints | `http.HandleFunc()`, `r.GET()`, `e.GET()`, `app.Get()`, route registrations |
| Scheduled jobs | `time.Ticker`, `cron` library, goroutines with `time.Sleep` loops |
| Message consumers | Sarama consumer, `nats.Subscribe()`, SQS polling goroutines |

**Rust (Actix-web, Axum, Rocket):**

| Entry Point Type | Search Pattern |
|---|---|
| REST endpoints | `#[get()]`, `#[post()]`, `Router::new().route()`, `web::resource()` |
| Scheduled jobs | `tokio::spawn` with interval loops, `cron` crate |
| Message consumers | `rdkafka` consumer, `lapin` (RabbitMQ), `async-nats` |

**.NET (C#):**

| Entry Point Type | Search Pattern |
|---|---|
| REST endpoints | `[ApiController]`, `[HttpGet]`, `[HttpPost]`, `ControllerBase` |
| Background services | `IHostedService`, `BackgroundService`, `Timer` callbacks |
| Message consumers | `IConsumer<T>` (MassTransit), Azure Service Bus handlers |
| Minimal API | `app.MapGet()`, `app.MapPost()` |
| gRPC | `.proto` files, `Grpc.AspNetCore` service implementations |

---

## Archetype: Stored Procedure System

Systems where business logic lives primarily in the database — Sybase, Oracle, SQL Server,
PostgreSQL. The application layer is often a thin pass-through (JDBC calls, ODBC, thin
REST wrapper). **The stored procedures are the use cases.**

### Entry Point Patterns

| Entry Point Type | Search Pattern | Notes |
|---|---|---|
| Stored procedures | `CREATE PROCEDURE`, `CREATE OR REPLACE PROCEDURE`, `sp_`, proc catalog queries | Each externally-called proc is a potential use case |
| DB functions | `CREATE FUNCTION`, `CREATE OR REPLACE FUNCTION` | Especially those called by application layer or other procs |
| DB-scheduled jobs | SQL Server Agent jobs, Oracle `DBMS_SCHEDULER`, `pg_cron`, Sybase Job Scheduler | These are scheduled use cases living entirely in the DB |
| Triggers | `CREATE TRIGGER` | Reactive use cases — fired on data change |
| Complex views | `CREATE VIEW` with business logic (calculated columns, multi-table joins) | May represent Serve use cases |
| Packages (Oracle) | `CREATE PACKAGE`, `CREATE PACKAGE BODY` | Groups of related procs — each public proc is an entry point |

### Where to Find Procs

| Location | What to look for |
|---|---|
| SQL source files in repo | `.sql` files, `migrations/`, `scripts/`, `stored-procedures/` directories |
| Application layer calls | `CallableStatement`, `EXEC`, `CALL`, proc name strings in code |
| ORM mappings | `@NamedStoredProcedureQuery`, `@Procedure`, Dapper `.Execute` with proc names |
| Config/properties | Connection strings, data source names, schema references |
| Database catalog | `INFORMATION_SCHEMA.ROUTINES`, `USER_PROCEDURES`, `sysobjects WHERE type = 'P'` |

### Tracing Proc Use Cases

```
Application call (or DB job trigger)
  → Stored procedure
      → Reads from / writes to tables
      → Calls other procs (trace these as sub-steps)
      → Fires triggers (trace as side effects)
  → Terminal effect: data changed, result set returned
```

Key insight: In these systems, the proc name often *is* the behavioral verb.
`GetProductByID` → "Search product by ID". `ImportOrderBatch` → "Import order batch".

### Hidden Use Cases in Stored Procedure Systems

- **DB jobs** — SQL Server Agent, Oracle Scheduler, pg_cron: these are scheduled use cases
  that never touch the application layer
- **Triggers** — reactive use cases that fire on INSERT/UPDATE/DELETE
- **Cross-database calls** — linked servers, database links, foreign data wrappers
- **Procs called only by other procs** — internal, not entry points; trace as call chain steps

---

## Archetype: Frontend Application

React, Angular, Vue, Svelte, or any browser-based SPA/MPA. Use cases are **user journeys**
— what the user can accomplish, not what components exist.

### Entry Point Patterns

**React / Next.js:**

| Entry Point Type | Search Pattern |
|---|---|
| Page routes | `pages/` directory (Next.js), `<Route path=...>`, `createBrowserRouter` |
| Form submissions | `onSubmit`, `handleSubmit`, form `action` handlers |
| Button handlers | `onClick`, dispatch actions, mutation triggers |
| API calls out | `fetch()`, `axios`, `useSWR`, `useQuery` (React Query/TanStack), `trpc` |
| State actions | Redux `createAsyncThunk`, Zustand actions, `useReducer` dispatch |
| Effects with side effects | `useEffect` that calls APIs, sets up WebSocket, starts polling |
| Server actions (Next.js) | `"use server"` functions, form actions |

**Angular:**

| Entry Point Type | Search Pattern |
|---|---|
| Page routes | `RouterModule.forRoot()`, `Routes` array, lazy-loaded modules |
| Form submissions | `(ngSubmit)`, reactive forms `FormGroup.value` |
| Button handlers | `(click)`, template event bindings |
| API calls out | `HttpClient`, service classes with `.get()`, `.post()` |
| State actions | NgRx `createAction`, `createEffect`, `@Effect()` |
| Resolvers/guards | `CanActivate`, `Resolve` — may load data before route |

**Vue / Nuxt:**

| Entry Point Type | Search Pattern |
|---|---|
| Page routes | `pages/` directory (Nuxt), `vue-router` route definitions |
| Form submissions | `@submit`, `v-on:submit`, method handlers |
| Button handlers | `@click`, `v-on:click`, method and inline handlers |
| API calls out | `axios`, `$fetch` (Nuxt), `useFetch`, composable API calls |
| State actions | Vuex actions, Pinia actions, `store.dispatch()` |

### Tracing Frontend Use Cases

```
User action (click, submit, navigate)
  → Event handler / action creator
      → API call(s) to backend
      → State mutation
      → UI update / navigation
  → Terminal effect: what the user sees or what data changed
```

Group by **user journey**, not by component. "Submit registration form" is one use case
even if it touches a form component, a validation hook, an API service, and a toast notification.

### Hidden Use Cases in Frontends

- **Background polling** — `setInterval`, WebSocket listeners, Server-Sent Events
- **Service workers** — offline sync, push notification handling, cache strategies
- **Route guards** — auth checks that redirect (not use cases themselves, but cross-cutting)
- **Deep links** — URL-driven state that constitutes a use case entry point
- **Keyboard shortcuts** — may trigger significant actions not visible in UI scan

---

## Archetype: Monolith with UI

Server-rendered applications: JSP, Razor Pages, Thymeleaf, Rails, Django templates,
PHP (Laravel/Symfony). Combines backend logic with server-rendered pages.

### Entry Point Patterns

**Rails:**

| Entry Point Type | Search Pattern |
|---|---|
| Page/API routes | `config/routes.rb`, `resources`, `get`, `post` |
| Controller actions | Methods in `app/controllers/` — each action is a potential use case |
| Background jobs | Sidekiq `perform_async`, ActiveJob `perform_later`, `Delayed::Job` |
| Scheduled tasks | `whenever` gem (cron), Sidekiq-cron, `clockwork` |
| Callbacks | `after_create`, `after_save` — model-level reactive behavior |

**Laravel (PHP):**

| Entry Point Type | Search Pattern |
|---|---|
| Routes | `routes/web.php`, `routes/api.php`, `Route::get()`, `Route::post()` |
| Controller actions | Methods in `app/Http/Controllers/` |
| Scheduled jobs | `app/Console/Kernel.php` `schedule()`, `artisan` commands |
| Queue workers | `ShouldQueue` interface, `dispatch()` calls |
| Event listeners | `EventServiceProvider`, `$listen` array |

**Django with templates:**

| Entry Point Type | Search Pattern |
|---|---|
| URL routes | `urlpatterns` in `urls.py` |
| View functions/classes | `View`, `TemplateView`, function-based views |
| Management commands | `management/commands/` |
| Celery tasks | `@shared_task`, `@task` |
| Signals | `post_save`, `pre_delete`, `Signal.connect()` |

**Spring MVC (Thymeleaf, JSP):**

| Entry Point Type | Search Pattern |
|---|---|
| Page controllers | `@Controller` (not `@RestController`), methods returning view names |
| Form handlers | `@PostMapping` that accept form data, `@ModelAttribute` |
| REST endpoints | `@RestController` — may coexist with page controllers |
| All backend patterns | Same as Backend API Service archetype |

### Hidden Use Cases in Monoliths

- **Admin panels** — often a separate set of controllers with CRUD for managing the system
- **Background workers** — the monolith often has scheduled tasks running alongside the web server
- **Report generators** — triggered by cron or admin action, produce files/emails
- **Data migration scripts** — rake tasks, artisan commands, management commands

---

## Archetype: Batch / ETL System

Airflow, SSIS, Spring Batch, dbt, shell scripts, cron-driven pipelines.
**Each job or pipeline is a use case.** Steps within a job are the call chain.

### Entry Point Patterns

| Entry Point Type | Search Pattern | Notes |
|---|---|---|
| Airflow DAGs | `DAG()`, `@dag`, `with DAG(...)` in `dags/` directory | Each DAG is a use case; tasks are the call chain |
| Airflow tasks | `PythonOperator`, `BashOperator`, `SQLExecuteQueryOperator`, `@task` | Steps in the call chain |
| Spring Batch | `@Bean Job`, `StepBuilder`, `ItemReader/Processor/Writer` | Job = use case, Steps = call chain |
| SSIS packages | `.dtsx` files, Data Flow tasks, Control Flow | Each package is a use case |
| dbt models | `models/` directory, `dbt_project.yml`, `sources.yml` | Each model/mart is a Transform use case |
| Shell scripts | `.sh` files called by cron, orchestrator scripts | Each script invocation is a use case |
| Crontab entries | `crontab -l`, `/etc/cron.d/`, systemd timers | Each cron entry is a trigger for a use case |
| Kubernetes CronJobs | `CronJob` manifests, `batch/v1` API | Scheduled container-based jobs |

### Tracing Batch Use Cases

```
Trigger (schedule, file arrival, API call, manual)
  → Job/DAG start
      → Step 1: Extract (from source)
      → Step 2: Transform (clean, enrich, validate)
      → Step 3: Load (to target)
      → On failure: retry / dead letter / alert
  → Terminal effect: data in target system, report generated, file produced
```

### Hidden Use Cases in Batch Systems

- **Retry/recovery jobs** — re-process failed batches
- **Monitoring/alerting** — jobs that check data quality or pipeline health
- **Cleanup jobs** — archive old data, rotate logs, delete temp files
- **Dependency chains** — DAG A triggers DAG B; trace the full chain

---

## Archetype: Event-Driven System

Systems where message handlers are the primary entry points. Kafka, RabbitMQ, SQS,
EventBridge, NATS, Pulsar.

### Entry Point Patterns

| Entry Point Type | Search Pattern | Notes |
|---|---|---|
| Kafka consumers | `@KafkaListener`, `kafkajs consumer.run()`, consumer group configs | Topic name reveals purpose |
| RabbitMQ consumers | `@RabbitListener`, `amqplib channel.consume()`, queue bindings | Exchange/routing key reveals purpose |
| SQS consumers | `@SqsListener`, AWS SDK `receiveMessage`, Lambda SQS trigger | Queue name reveals purpose |
| Event handlers | `@EventListener`, MediatR `INotificationHandler`, domain event subscribers | Internal event reactions |
| Saga/process managers | Saga orchestrators, state machines, choreography patterns | Multi-step coordinated workflows |
| DLQ consumers | Dead letter queue handlers, retry consumers | Error recovery use cases |
| REST endpoints | May exist for queries, health, or manual triggers | Secondary to event flow |

### Tracing Event-Driven Use Cases

```
Event arrives (message, domain event, webhook)
  → Consumer / handler
      → Business logic
      → Optionally publish new events
      → Store state changes
  → Terminal effect: state changed, downstream event published, notification sent
```

### Hidden Use Cases in Event-Driven Systems

- **DLQ processing** — handling failed messages is a distinct use case
- **Event replay** — rebuilding state from event log
- **Saga compensation** — rollback steps when multi-step workflows fail
- **Ordering guarantees** — partition-key-based ordering may mask separate use cases in the same topic

---

## Archetype: CLI Tool

Command-line tools where each command or subcommand is a use case.

### Entry Point Patterns

| Framework | Search Pattern |
|---|---|
| Click (Python) | `@click.command()`, `@click.group()`, command decorators |
| Typer (Python) | `@app.command()`, `Typer()` |
| argparse (Python) | `add_subparsers()`, `add_parser()` |
| Cobra (Go) | `cobra.Command{}`, `cmd.AddCommand()` |
| Clap (Rust) | `#[derive(Parser)]`, `#[command()]`, `Subcommand` enum |
| picocli (Java) | `@Command`, `@Parameters`, `@Option` |
| Commander (Node.js) | `.command()`, `.action()` |
| yargs (Node.js) | `.command()`, command modules |
| System.CommandLine (.NET) | `Command()`, `RootCommand`, handlers |

### Tracing CLI Use Cases

```
Command invocation (with args/flags)
  → Argument parsing and validation
      → Business logic (may call APIs, read/write files, query DB)
  → Terminal effect: file produced, data changed, output printed, exit code
```

### Hidden Use Cases in CLI Tools

- **Flags that fundamentally change behavior** — these are variants, not separate use cases
- **Interactive modes** — prompts, wizards, REPL loops
- **Config file processing** — `init` or `config` commands that set up state
- **Piped input/output** — stdin/stdout processing is a different use case than file-based

---

## Archetype: Mobile Application

iOS, Android, React Native, Flutter, Kotlin Multiplatform.

### Entry Point Patterns

**Android (Kotlin/Java):**

| Entry Point Type | Search Pattern |
|---|---|
| Screens | `Activity`, `Fragment`, Jetpack Compose `@Composable` screens |
| User actions | `setOnClickListener`, `onClick`, button bindings |
| API calls | Retrofit interfaces, `@GET`, `@POST`, OkHttp calls |
| Background work | `WorkManager`, `CoroutineWorker`, `JobScheduler`, `AlarmManager` |
| Push handlers | `FirebaseMessagingService.onMessageReceived()` |
| Deep links | `<intent-filter>` with `<data>`, Navigation deep links |
| Broadcast receivers | `BroadcastReceiver`, `onReceive()` |

**iOS (Swift):**

| Entry Point Type | Search Pattern |
|---|---|
| Screens | `UIViewController`, SwiftUI `View`, `NavigationView` |
| User actions | `@IBAction`, `Button(action:)`, gesture recognizers |
| API calls | `URLSession`, Alamofire, `async/await` network calls |
| Background work | `BGTaskScheduler`, `BGAppRefreshTask`, background fetch |
| Push handlers | `UNUserNotificationCenter` delegate methods |
| Deep links | `application(_:open:)`, Universal Links, `onOpenURL` (SwiftUI) |

**Flutter:**

| Entry Point Type | Search Pattern |
|---|---|
| Screens | `StatefulWidget`, `StatelessWidget`, route definitions in `MaterialApp` |
| User actions | `onPressed`, `onTap`, `GestureDetector` |
| API calls | `http` package, `dio`, repository classes |
| Background work | `WorkManager` (via plugin), `Isolate.spawn()` |
| Push handlers | `FirebaseMessaging.onMessage`, notification callbacks |

### Tracing Mobile Use Cases

```
User action (tap, swipe, navigate) or system event (push, background trigger)
  → Handler / ViewModel / BLoC
      → API call(s) to backend
      → Local storage update
      → UI state change
  → Terminal effect: screen updated, data synced, notification shown
```

### Hidden Use Cases in Mobile Apps

- **Background sync** — data upload/download when app is not in foreground
- **Push notification handling** — received push may trigger navigation or silent data update
- **Offline mode** — queuing actions for later sync is a distinct use case
- **App lifecycle events** — `onResume`, `applicationDidBecomeActive` may trigger refresh
- **Widget/extension actions** — iOS widgets, Android widgets, watch complications

---

## Archetype: Serverless / FaaS

AWS Lambda, Google Cloud Functions, Azure Functions, Cloudflare Workers.
**Each function is a use case.** The trigger type defines the actor.

### Entry Point Patterns

| Trigger Type | Search Pattern | Actor |
|---|---|---|
| HTTP trigger | API Gateway config, `@HttpTrigger`, route definitions | API consumer |
| Schedule trigger | CloudWatch Events, `@TimerTrigger`, Cloud Scheduler | Scheduler |
| Queue trigger | SQS trigger, `@QueueTrigger`, Pub/Sub push | Message producer |
| Storage trigger | S3 event, `@BlobTrigger`, GCS notification | File upload |
| Stream trigger | DynamoDB Streams, Kinesis, Change Streams | Data change |
| Event trigger | EventBridge, SNS, custom events | Event publisher |

### Tracing Serverless Use Cases

```
Trigger event (HTTP, schedule, queue, file)
  → Function handler
      → Business logic (may call other services, DB, storage)
  → Terminal effect: response returned, data changed, downstream triggered
```

---

## Hidden Use Case Detection (All Archetypes)

Regardless of archetype, these categories of use cases are commonly missed.

### Scheduled and Background Use Cases

Scheduled jobs are often the most critical use cases in a system. They represent
core data pipelines, cache management, batch processing, nightly syncs, and
automated business operations.

**What they represent — map to behavioral families:**

| Scheduled behavior | Family | Example |
|---|---|---|
| Cache warming/refresh | **Operational** | Daily full cache reload from database |
| Data import/poll from external source | **Acquisition / Feed** | Hourly poll for partner files |
| Data export/sync to external system | **Distribution / Feed** | Nightly data share to downstream system |
| Batch processing / transformation | **Transform** | Process raw data into structured form |
| Cleanup / archival / retention | **Administration** | Remove stale data older than retention period |
| Report generation | **Analysis** | Generate daily metrics report |
| Billing / financial operations | **Finance** | Monthly price adjustment, invoice generation |
| Notification dispatch | **Notification** | Weekly digest email, overdue reminders |

### Event-Driven Use Cases

| Event-driven behavior | Family | Example |
|---|---|---|
| React to data change from upstream | **Acquisition / Feed** | Consume topic with new registrations |
| Trigger downstream processing | **Transform** or **Match** | On data ingested, start processing pipeline |
| Notify on state change | **Notification** | On case closed, send email to stakeholders |
| Sync state to external system | **Distribution / Feed** | On record updated, push to partner API |
| Update read model / projection | **Serve** | On write event, update search index |

### Startup and Initialization

Code that runs on application start — often missed:

- Startup hooks → cache loading, connection warming, health registration
- Initialization methods → setup that may have business meaning
- Database migration scripts → schema changes, data migration

### Cross-Cutting Concerns (Not Use Cases)

These wrap multiple use cases transparently — document as cross-cutting, not as use cases:

- Timing/logging aspects → observability
- Security enforcement → authorization
- Transaction management → consistency
- Audit logging → compliance

### Dead Code Indicators

| Signal | What it means |
|---|---|
| Deprecation annotations or comments | Marked for removal but possibly still called |
| TODO comments mentioning removal | Developer acknowledged it should go |
| No test coverage | Possibly abandoned |
| No auth/permission assigned | Endpoint may be unreachable |
| Feature flag disabled, commented-out route | Not active in production |
| Called only by other dead code | Dead chain — the whole call tree is dead |

---

## Common Architecture Patterns to Trace

These patterns appear across all archetypes. Recognizing them helps trace call chains
and identify cross-cutting concerns.

### Cache-First with Fallback

```
Request → Check Cache → (hit) → Return cached
                      → (miss) → Query DB → Update Cache → Return
                      → (error) → Query DB directly → Return (degraded)
```

Document: What is cached? What triggers invalidation? What is the staleness window?

### Pipeline / ETL

```
Source → Ingest (raw) → Transform → Validate → Store (processed) → Notify
```

Document: What triggers the pipeline? What are the stages? Where can it fail?

### Saga / Distributed Transaction

```
Step 1 → Step 2 → Step 3
              ↓ (failure)
         Compensate Step 1
```

Document: What are the steps? What compensations exist? What if compensation fails?

### CQRS (Command Query Responsibility Segregation)

```
Commands → Write Model → Event → Read Model (projection)
Queries → Read Model
```

Document: Are reads and writes separated? Different data stores? Event-driven sync?

### Event-Driven Fan-Out

```
Producer → Event Bus → Consumer A
                     → Consumer B
                     → Consumer C
```

Document: What events? Who publishes? Who subscribes? Is ordering guaranteed?

### Offline-First (Mobile/Desktop)

```
User action → Local store → Queue for sync
                           → On connectivity → Push to server → Reconcile
```

Document: What is stored locally? How are conflicts resolved? What syncs immediately vs. queued?

---

## Call Chain Notation

Standard format for documenting call chains. Adapt layer labels to match the
system's archetype — not all systems have "controllers" and "services".

```
UC<N>: <Behavioral name>
  Actor: <who/what triggers this>
  Entry: <HTTP method + path | cron expression | topic | command | user action>
  Chain:
    ClassName.methodName()                    [layer:line_or_annotation]
    -> ClassName.methodName()                 [layer:line_or_annotation]
        -> ClassName.methodName()             [resource: db_proc | table | cache | API]
        -> ClassName.methodName()             [resource: description]  (conditional)
  Terminal Effect: <response | state change | file produced | UI update>
  Variants:
    - <condition> -> <different behavior>
  Fallback:
    - <On error> -> <fallback path>
  Status: <Active | Dormant | Deprecated | Dead>
  Evidence: <what confirms the status>
```

Layer annotations (adapt to archetype):
- `[controller:line]` or `[handler:line]` — entry/presentation layer
- `[service:line]` or `[usecase:line]` — business/orchestration layer
- `[db: procedure_or_query]` — database access
- `[cache: system_name]` — cache access
- `[external: service_name]` — outbound HTTP/messaging
- `[event: topic_or_type]` — event publishing/consuming
- `[file: path_or_pattern]` — file system access
- `[ui: component]` — UI layer (frontend/mobile)
- `[proc: procedure_name]` — stored procedure (database archetype)
- `[step: job_step]` — batch/ETL step

Indentation indicates call depth. `(conditional)` marks optional branches.
`-> ` (arrow) indicates a synchronous call. `~> ` (tilde arrow) indicates async/queued.
