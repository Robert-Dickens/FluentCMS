---
name: cms-test-infrastructure
description: Use when creating or changing shared test fixtures, test modules, seeded data, tenant setup, unit-of-work helpers, or SQLite test configuration for the CMS tests.
---

# CMS Test Infrastructure

Use this skill for shared setup used by the CMS test projects under `Modules/Cms/test`.

## Existing structure

- `TestBase/Cms.TestBase.csproj` is the shared foundation. Its common base is `FluentCMSTestBase<TStartupModule>` in `FluentCMSTestBase.cs`; it configures Autofac, loads test settings, and exposes unit-of-work helpers.
- `FluentCMSTestBaseModule` wires shared platform modules, debug logging, random-data support, and startup seeding.
- `DataSeeders/SiteContentDataSeeder.cs` seeds sites, content types, blocks, and content. `KnownTenants` supplies the non-tenant and tenant contexts; `FluentCMSTestData` exposes seeded identifiers.
- `Domain.Context.Tests/FluentCMSDomainContextTestModule.cs` configures EF Core SQLite with an in-memory connection, creates the schema, and disables unit-of-work transactions for these tests.
- Layer-specific test modules compose on these shared modules. Keep layer-specific configuration in its owning test project.

## When adding shared setup

1. Prefer extending the existing base class, module, seeder, or test data holder over introducing another framework or abstraction.
2. Put setup in the narrowest layer that needs it: shared behavior in `TestBase`, EF Core setup in `Domain.Context.Tests`, application authorization setup in `Application.Tests`, and host setup in `HttpApi.Tests`.
3. Resolve application dependencies through the test service provider. Reuse seeded records and `FluentCMSTestData` identifiers instead of relying on random generated values.
4. For data changes that require a unit of work, use `WithUnitOfWorkAsync` and complete it through the existing helper.
5. Preserve tenant context explicitly when a test depends on tenant-specific data; the shared seeder runs for the known tenant IDs.
6. Keep settings and secrets out of source-controlled test data. The base loads `appsettings.json` and optional `appsettings.secrets.json`.
7. Avoid changing shared seeding counts or initialization semantics for a single test; build the test around the existing data or make the required change narrowly and account for dependent suites.

## Verify

- Run the narrowest affected CMS test project first, then the dependent test project(s).
- Confirm tests can use the intended test module and fixture without leaking tenant, scope, or unit-of-work state.
- Do not edit generated files under `obj`.
