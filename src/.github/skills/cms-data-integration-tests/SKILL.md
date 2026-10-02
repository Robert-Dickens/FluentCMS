---
name: cms-data-integration-tests
description: Use when adding or changing CMS repository, EF Core persistence, tenant-scoping, or application-service tests that use the shared SQLite-backed test data.
---

# CMS Data Integration Tests

Use this skill for database-backed CMS tests in `Domain.Context.Tests`, `Domain.Tests`, and `Application.Tests`.

## Existing patterns

- The domain-context test module uses an in-memory SQLite connection and creates the EF Core schema for the test application.
- `Domain.Context.Tests/Content/BlockModelRepositoryTests.cs` and `Domain.Tests/Content/BlockModelRepositoryTests.cs` inherit the shared `BaseBlockModelRepositoryTestFixture<TStartupModule>` from `TestBase`. Shared repository behavior is therefore exercised with layer-specific startup modules.
- Application tests inherit `FluentCMSApplicationTestBase`, resolve services from the test service provider, and use `UsingDbContext` when they need to inspect persisted state.
- Test data is seeded centrally. The sample seeder creates 100 blocks and 100 content records for each configured tenant context; use the actual database count or `FluentCMSTestData` rather than treating generated IDs or ordering as fixed.
- Existing tests use xUnit `[Fact]` and Shouldly assertions.

## Guidance

1. Choose the narrowest useful layer: repository/provider behavior belongs in a context/domain test; application-service behavior belongs in `Application.Tests`.
2. For behavior shared by the two repository test projects, add the test to `BaseBlockModelRepositoryTestFixture<TStartupModule>` so both derived test classes execute it.
3. For tenant-scoped behavior, obtain `ICurrentTenant`, change to `KnownTenants.TenantId` within a disposable tenant scope, and assert the returned records' `TenantId` values.
4. For application behavior, resolve the application service from DI. Use `UsingDbContext` for database verification rather than reaching into internal implementation details.
5. Use `WithUnitOfWorkAsync` when a test requires an explicit unit of work or persisted writes.
6. Assert behavior and relevant values, not just that a collection is non-null. When checking paging, compare returned item count and total count with database state or a demonstrated seeded-data precondition.
7. Avoid introducing per-test database configuration when the shared SQLite module already provides the required setup.

## Verify

Run the directly affected test project. If changing a shared repository fixture, run both `Cms.Domain.Context.Tests` and `Cms.Domain.Tests`; if changing application services or shared data, include `Cms.Application.Tests`.
