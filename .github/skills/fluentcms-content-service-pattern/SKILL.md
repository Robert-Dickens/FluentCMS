---
name: fluentcms-content-service-pattern
description: Follow or extend the FluentCMS Content feature's app-service contract, DTO, custom repository query, service lifecycle hooks, and HTTP API controller pattern. Use when adding Content-like behavior or custom CRUD service operations.
---

# FluentCMS Content Service Pattern

Use the Content feature as the reference for CRUD app services that need a custom repository-backed query and post-persistence events. Reuse the generic CRUD base for ordinary CRUD behavior; add only feature-specific logic. Do not copy Content-specific field names, permissions, event names, or query routes into another feature without adapting them.

## Layered contract and model

The Content feature separates these types and projects:

- `IContentModelRepository` and `ContentModel` are in the Domain project. The model inherits `SiteAssociatedEntityModel`, and the repository extends `IRepository<ContentModel, Guid>` with the feature-specific `GetAllByContentType` operation.
- `IContentAppService`, its CRUD inputs, the response `Content`, and `GetContentRequest` are in Application.Abstractions. The response derives from `ExtensibleFullAuditedEntityDto<Guid>`; the create/update inputs derive from `ExtensibleEntityDto`; the list input derives from `FilteredAndPagedAndSortedResultRequestDto`.
- `ContentAppService` is in Application and inherits `FluentCMSCrudAppService<DomainModel, ResponseDto, Key, GetListInput, CreateInput, UpdateInput>`. Preserve that generic parameter order and use the exact model type imported by the app service. Do not substitute an EF Core `DbModels.*` type for a Domain model just because a storage class has a similar name.
- `ContentController` is in HttpApi, implements `IContentAppService`, and forwards calls to the application service.

When creating a different feature, inspect the requested repository interface and the actual service generic arguments first. The domain model, persistence DbModel, and Application DTO are distinct layers and are not interchangeable.

## CRUD contract and DTO pattern

Keep the service interface and related request/response types together in the appropriate Application.Abstractions feature file, following neighboring feature naming and namespaces. The interface extends the CRUD service abstraction using response DTO, key, list request, create request, and update request. Add only custom operations required by the repository or use case.

Shape DTO bases and fields from neighboring contracts and the actual domain model. Decide whether the DTO needs extra properties, audit/tenant fields, or associated IDs by inspecting the model and security requirements. Do not expose persistence navigation properties, secrets, or internal data merely because they exist on the model.

## Application service pattern

For standard CRUD, derive from `FluentCMSCrudAppService<...>` and inject the corresponding repository through the constructor. Set the class-level authorization policy and assign the create, update, and delete policy names from the feature's permission constants, matching the existing service pattern.

For a custom repository query:

- Call it through the feature repository interface, not a concrete storage implementation.
- Pass the `CancellationToken` through to the repository.
- Return the contract's declared type. Content exposes its filtered results as `IAsyncEnumerable<Content>`; its implementation maps each domain model through `ObjectMapper` as it yields results.
- Apply `[EnumeratorCancellation]` to a cancellation token accepted by an async iterator. Preserve streaming semantics where the contract is a stream; do not materialize results into a list without a requirement.
- Keep the model-to-DTO mapping in the configured object-mapping context. Check the actual source and destination properties for nontrivial conversions.
- The Content response DTO derives from `ExtensibleFullAuditedEntityDto<Guid>`. Check both mapping types and their base classes for `IHasExtraProperties`, and apply `[MapExtraProperties]` to the Mapperly partial class when required.

For side effects after CRUD, use the base class lifecycle hooks rather than reimplementing the CRUD method. Content publishes create/update/delete messages from `OnAfterCreateAsync`, `OnAfterUpdateAsync`, and `OnAfterDeleteAsync` using `IMessagePublisher`, the matching action name, and the cancellation token. Preserve the hook's return/deletion semantics and publish the correct domain event only when the use case requires it. Follow `ActionNames` and message-handler conventions; do not invent a new event naming system.

## HTTP API controller pattern

Use a thin controller that implements the same app-service interface and forwards each contract method to an injected interface instance. Follow the neighboring controller attributes for `[Controller]`, `[RemoteService]`, `[Area]`, module route prefix, and class-level `[Authorize]`.

- Add a route and operation-level authorization attribute for each write operation, using the corresponding feature permission constant.
- Keep reads under the class-level/default policy unless the permission model defines a distinct read permission.
- Expose custom contract methods explicitly with a route that is unambiguous with CRUD routes; forward all arguments and cancellation tokens unchanged.
- Return the declared service task or stream directly; do not duplicate application logic in the controller.
- When a DTO name collides with a namespace such as `Content`, use an explicit type alias to keep signatures bound to the DTO type.

## Related specialized skills

When implementing this pattern in a new feature, also follow the applicable repository skills:

- `.github/skills/fluentcms-permissions/SKILL.md` for permission constants, provider definitions, tenancy, and custom permission checks.
- `.github/skills/fluentcms-localization/SKILL.md` for permission display names and user-facing strings.
- `.github/skills/mapperly-mapping/SKILL.md` for model/DTO mappings and custom conversions.

## Validation

- Confirm the repository method, model type, service interface, service implementation, and controller signatures agree exactly.
- Confirm authorization is present at both service and API boundaries and write operations use the intended child permissions.
- Verify cancellation is forwarded through custom operations and event publishers.
- Build the affected Application.Abstractions, Domain, Application, and HttpApi projects; run relevant tests.
- Verify any newly added mapper and localization resource changes through their specialized skills.
