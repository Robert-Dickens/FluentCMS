---
name: cms-unit-tests
description: Use when adding or changing focused unit tests in the CMS projects, especially mapper behavior that does not require HTTP or database persistence assertions.
---

# CMS Unit Tests

Use this skill for focused behavior tests in `Modules/Cms/test`, especially mapping tests such as `Application.Tests/Mappers/Content/BlocksMappersTests.cs`.

## Existing pattern

- Inherit from the layer base needed to resolve the behavior; mapper tests inherit from `FluentCMSApplicationTestBase`.
- Resolve the mapper and random-data builders from dependency injection in the test class constructor.
- Build source and destination model instances with the existing `IRandomData<T>` builder rather than hand-constructing large objects.
- Call the mapping operation directly and assert the mapped result and the specific mapped properties relevant to the behavior.
- Use xUnit `[Fact]` and the existing Shouldly assertions. Keep each test focused on one mapping direction or behavior.

## Guidance

1. Keep the test independent of HTTP and database behavior when verifying a mapping contract.
2. Assert important contract fields explicitly; a non-null result alone does not verify the mapping.
3. Follow existing class/collection parallelization attributes only when the test shares that collection or its fixture requirements.
4. Avoid duplicating random-data registration or adding a new mocking library for behavior already available through the test module.
5. Name tests to describe the source, destination, and expected mapping behavior, following the surrounding test naming style.

## Verify

Run `Cms.Application.Tests` after modifying application mapper tests. If the test uses another layer's base or module, also run that owning test project.
