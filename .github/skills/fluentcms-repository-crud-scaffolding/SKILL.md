---
name: fluentcms-repository-crud-scaffolding
description: Scaffold or complete FluentCMS CRUD features from a supplied repository interface or IRepository<TEntity, TKey>, including domain and API models, application-service contracts and implementations, HTTP API controllers, permissions, localization, and Mapperly integration.
---

# FluentCMS Repository-Driven CRUD Scaffolding

Use this orchestration skill when the user supplies a repository interface or `IRepository<TEntity, TKey>` and asks to scaffold the associated FluentCMS CRUD feature. Treat the supplied repository as the starting contract, discover existing layers before creating files, and implement only missing pieces. Coordinate authorization, localization, and mapping using the specialized skills listed below.

## Inputs and ambiguity

Resolve these from the repository and existing feature patterns where possible; ask the user only for decisions that cannot safely be inferred:

- Feature/entity name, namespace/module, primary key type, and requested operations.
- Domain model properties and whether the repository/model already exists.
- Response DTO and create/update/list request shapes; validation, nullability, and extra-property requirements.
- Supported custom repository queries or service operations and their return/streaming behavior.
- Storage providers that must implement a new repository contract. Do not create database/provider implementations unless requested or required by the project architecture.
- Authorization group, permission hierarchy, host/tenant scope, provider restrictions, and optional feature/state gating.
- User-facing display names and localization meaning for the supported cultures.
- Whether lifecycle events/messages are required and which consumers/actions they must serve.

Do not invent business fields, permissions, route behavior, tenant ownership, persistence support, or event semantics. Reuse the repository's conventions and existing types when these decisions are already encoded.

## Inspect before scaffolding

1. Resolve the interface's actual namespace, model type, key type, methods, and base interfaces. A feature repository may inherit `IRepository<TModel, TKey>` and add custom operations.
2. Search for its Domain model, persistence DbModel, repository implementation(s), registrations, tests, API clients, and any existing app-service/controller. Avoid duplicate types and endpoints.
3. Distinguish `FluentCMS.Domain.*` models from `FluentCMS.Repositories.*.DbModels` persistence classes and Application DTOs. `FluentCMSCrudAppService` uses the exact model in its generic declaration; do not replace it with a similarly named EF Core type.
4. Read `FluentCMSCrudAppService<TEntity, TEntityDto, TKey, TGetListInput, TCreateInput, TUpdateInput>`, a simple neighboring app service/controller, and `FluentCMS.Content.ContentAppService` plus `.github/skills/fluentcms-content-service-pattern/SKILL.md` when custom queries, async streams, or lifecycle events are involved.
5. Inventory existing permissions, localization keys, mapper conventions, and build/test commands for the affected projects.

## Specialized skills to follow

Use these repository-specific skills as part of this workflow; do not duplicate or override their security/mapping/localization rules:

- `.github/skills/fluentcms-permissions/SKILL.md`: permission constants/provider definitions, CRUD child permissions, host/tenant side, allowed value providers, enabled state, and state checkers.
- `.github/skills/fluentcms-localization/SKILL.md`: localized permission/resource display names and user-facing strings across all locale files.
- `.github/skills/mapperly-mapping/SKILL.md`: Mapperly maps, constructor/property conversions, ignored members, and `IHasExtraProperties`.
- `.github/skills/fluentcms-content-service-pattern/SKILL.md`: use when the feature needs custom repository operations, streamed results, message publishing, or service lifecycle hooks similar to Content.

If the environment cannot invoke a skill directly, read the referenced `SKILL.md` and follow its checklist in the current task.

## Scaffold the feature by layer

### 1. Domain model and repository contract

- Keep or create the Domain model in the feature's Domain area, using the established base class for key, audit, soft-delete, site, tenant, and extra-property behavior. Select the base only after inspecting comparable models and the requested semantics.
- Keep the supplied repository interface authoritative. Add only required custom operations and include cancellation tokens where neighboring asynchronous APIs do.
- If a persistence model/adapter is missing and in scope, implement the relevant DbModel, mapping, repository adapter, and DI registration for the requested storage provider(s). Preserve supported-provider behavior; do not assume one provider is the only implementation.
- Add domain invariants/validation only when required by the requested behavior and existing architecture.

### 2. Application.Abstractions contract and DTOs

- Add or complete `I<Entity>AppService` in the appropriate feature namespace, extending the project's CRUD service abstraction with response DTO, key, list input, create input, and update input in its established order.
- Define the response DTO, get-list request, create request, and update request following adjacent feature patterns. Avoid returning database models or exposing internal navigation objects.
- Add custom operations to the interface only when required by the repository or stated use case. Match return types exactly, including `IAsyncEnumerable<T>` when streaming is part of the contract.
- Add validation attributes/rules consistent with the project; preserve nullability and prevent over-posting of protected or server-owned properties.

### 3. Application service implementation

- Implement `I<Entity>AppService` in the Application project by deriving from `FluentCMSCrudAppService<DomainModel, ResponseDto, TKey, GetListInput, CreateInput, UpdateInput>` when it fits the existing CRUD base pattern.
- Inject the repository interface and delegate normal CRUD handling to the base. Set the class-level permission and create/update/delete policy names from the defined permission constants.
- Implement custom repository queries through the interface, pass cancellation end-to-end, and map domain models to response DTOs through the configured object mapper.
- For async iterators, follow the Content cancellation/streaming conventions. Avoid materializing large result sets unless required.
- Put side effects in the base lifecycle hooks (`OnBefore/OnAfterCreateAsync`, `OnBefore/OnAfterUpdateAsync`, `OnBefore/OnAfterDeleteAsync`) when those hooks match the use case. Publish messages only when requested; use stable action names, correct post-persistence ordering, and cancellation tokens.

### 4. Mapperly integration

- Add only maps required by the service implementation or CRUD base. Map the actual service generic Domain model to the Application DTO; do not map the EF Core DbModel instead unless that is the explicit request.
- Check both types and their base classes for `IHasExtraProperties`; apply `[MapExtraProperties]` when either side implements it.
- Resolve shape/name mismatches explicitly and ignore only intentionally unmapped members.
- Follow `.github/skills/mapperly-mapping/SKILL.md`, then build the Application project to run source generation and inspect diagnostics.

### 5. Permissions and localization

- Add stable permission constants and definitions for the requested CRUD/custom operations. Use class-level default permission and operation-level create/edit/delete checks consistent with the feature's existing pattern; do not add an unrequested read permission.
- Prompt separately for host/tenant applicability and allowed value-provider types when unclear. Apply decisions per definition and child; do not assume inheritance.
- Add localized permission display names and any requested user-facing text through the registered `FluentCMSResource`; keep keys aligned with permission names.
- Follow `.github/skills/fluentcms-permissions/SKILL.md` and `.github/skills/fluentcms-localization/SKILL.md` rather than hard-coding authorization decisions or display strings.

### 6. HTTP API controller

- Add or complete a thin controller in HttpApi that implements the same app-service interface and forwards every method to the injected app-service interface.
- Follow neighboring controllers for `[Controller]`, `[RemoteService]`, `[Area]`, route prefixes, class-level authorization, and CRUD routes. Select a feature route by examining existing routes; do not mechanically derive a route when that could collide.
- Add operation-level authorization to write methods using the exact child permission constants. Reads normally use the class-level permission unless a dedicated read permission exists.
- Expose every custom service operation with an unambiguous route and preserve arguments, result type, streaming behavior, and cancellation.
- Alias DTO names when a namespace shadows a type name. Keep business logic in the app service, never duplicate it in the controller.
- If the module has generated/maintained HTTP API clients, update or regenerate those using the existing client workflow.

### 7. Tests and integration

- Add or update tests for the new service behavior, custom operations, permission gates, and event/state behavior where those are part of the feature. Reuse existing test projects and naming conventions; do not create redundant test layers.
- Verify repository registrations resolve for each supported provider in scope. Verify the app service and controller registrations/conventions are discovered as expected.

## Validation checklist

1. Build all changed projects: Domain, Application.Abstractions, Application, HttpApi, and relevant persistence/client projects.
2. Confirm one-to-one signature compatibility between repository, app-service interface, implementation, and controller.
3. Inspect Mapperly diagnostics and generated mappings, including extra properties and ignored members.
4. Check every permission constant is defined and used at the correct service/controller boundary; verify tenant side and allowed providers match the requested decisions.
5. Parse all affected locale JSON files, compare key coverage to English, verify culture names/nonempty translations, and search changed code for remaining literals.
6. Run focused tests, then broader relevant tests; report skipped or unavailable checks explicitly.
7. Summarize the files created/modified, custom behavior, permissions/providers/tenant scope, localization keys, and build/test results.
