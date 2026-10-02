---
name: cms-http-api-integration-tests
description: Use when adding or changing CMS HTTP API integration tests that exercise controllers through the in-process ASP.NET Core test host.
---

# CMS HTTP API Integration Tests

Use this skill for tests in `Modules/Cms/test/HttpApi.Tests` that verify HTTP behavior through the configured API test host.

## Existing pattern

- Inherit from `FluentCMSHttpApiTestBase`, which provides `AspNetCoreApiIntegrationTestBase<FluentCMSHttpApiTestModule>` and the shared API test collection.
- Define the endpoint with the `RoutePath` override, following the existing `api/{FluentCMSRemoteServiceConsts.RoutePath}/...` form.
- Configure authorization in the test when needed with `EnableAuthorization()`; the API test module enables the application test setup and configures the in-process routes and remote-service client.
- Exercise the endpoint through the existing HTTP helpers (for example, `GetPagedAsync<T>`), not by calling controller methods directly.
- Use xUnit `[Fact]` and Shouldly to verify the response contract, total count, and returned item count.

## Guidance

1. Test externally observable HTTP behavior: route, request parameters, authorization, and response shape.
2. Keep endpoint setup and route path consistent with the existing API fixture. Reuse the module's configured test host and client.
3. For pagination tests, choose a page size and assert both the page item count and a meaningful total-count condition. Do not assume the dataset contains an exact count unless the test explicitly establishes it.
4. Enable authorization when the endpoint's behavior requires it; do not bypass the HTTP pipeline by resolving application services in an API test.
5. Use existing seeded data and avoid adding endpoint-specific seed state unless the scenario requires it.
6. Keep API tests in `HttpApi.Tests`; test service-layer rules in `Application.Tests` instead.

## Verify

Run `Cms.HttpApi.Tests` after changing API tests or API test-host configuration. Include dependent application tests if a shared application module or seeded-data behavior changes.
