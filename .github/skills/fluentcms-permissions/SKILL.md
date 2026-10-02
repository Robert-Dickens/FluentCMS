---
name: fluentcms-permissions
description: Add or change FluentCMS permission groups, permissions, and child permissions. Use when asked to define permissions, CRUD permission trees, tenant/host scope, value-provider scope, enabled state, custom permission properties, or state-checker gating.
---

# FluentCMS Permissions Skill

Use `FluentCMSPermissionDefinitionProvider` as the local pattern. Keep permission names, authorization policies, value-provider restrictions, tenant scope, state checks, and localized display names consistent across the solution.

## Inspect conventions first

Before editing:

- Read `FluentCMSPermissions` constants and `FluentCMSPermissionDefinitionProvider` to understand existing group and child names.
- Check app services/controllers for policy names using the permission, plus related tests and other providers.
- Confirm the installed authorization package and provider classes available to the target project. The signatures below match the repository's ByteLabs authorization abstractions; verify them if the package version differs.
- Use named constants for permission names and provider names where available. Avoid duplicating permission-name strings across code.
- Follow the existing `L(...)` helper and the `FluentCMSResource` localization resource for display names. When adding localization text, follow `.github/skills/fluentcms-localization/SKILL.md` and update every supported locale.

The local pattern creates a permission group with a localizable display name, adds definitions to it, and registers CRUD children with `AddChild`. Reuse CRUD helpers where they remain clear; define bespoke children for capabilities that are not ordinary create/edit/delete operations, such as reset-password.

## Prompt for permission decisions

When the request does not specify the following, prompt the user before committing to a security-affecting choice. Ask per permission or child when choices differ; do not assume children inherit the parent's configuration.

1. **Tenant side:** Is the permission for `Host`, `Tenant`, or `Both`? The installed `MultiTenancySides` values are `Host`, `Tenant`, and `Both`; the permission defaults to `Both`. `AddPermission` and `AddChild` each accept their own `multiTenancySide` argument. Treat side applicability independently from who may hold a grant.
2. **Value provider:** Which provider may store or grant this permission? Use the provider's `ProviderName` constant, not a copied string. Available provider examples from the FluentCMS platform include:
   - `GlobalSettingValueProvider.ProviderName`: global-level value provider; this is the global/default choice described by the platform conventions.
   - `TenantSettingValueProvider.ProviderName`: tenant-bound.
   - `UserPermissionValueProvider.ProviderName`: user-bound.
   - `RolePermissionValueProvider.ProviderName`: role-bound.
   - `ClientPermissionValueProvider.ProviderName`: OAuth-client-bound.
   - `OrganizationPermissionValueProvider.ProviderName`: organization-bound.
   - `WorkspacePermissionValueProvider.ProviderName`: workspace-bound.

   Verify that the chosen provider is registered and referenced by the target project. Provider availability can depend on installed platform packages. `PermissionDefinition.Providers` is a list of allowed provider names; `WithProviders(...)` adds provider names. Per the installed abstractions, an empty list means all providers are allowed, not global-only. If the permission must be global-only, explicitly allow the global provider rather than relying on an empty list.
3. **Enabled state:** Should the definition be enabled? `AddPermission` and `AddChild` default `isEnabled` to `true`. A disabled permission cannot be granted and evaluates false, but remains defined for checking/hiding related functionality. Use a disabled definition only when that behavior is intended. `TryDisablePermission` can disable an existing named permission when extending definitions.
4. **Runtime state conditions:** Should access to the capability depend on a feature being installed/enabled, a module being available, or another runtime condition? Get the condition and its authoritative source from the user. If there is no additional condition, do not add a state checker.
5. **Custom metadata:** Does the permission or group need extra data for other components? Use `WithProperty(key, value)` or the `Properties` dictionary/indexer only when a consuming component needs the metadata. These properties do not themselves enforce access.
6. **Hierarchy and display:** Confirm the group, permission, and child names, parent-child relationships, and user-facing display names. Do not confuse the stable authorization name with its localizable display text.

Do not conflate tenant side and value provider: a permission can be host-only or tenant-only and also be restricted to a particular provider. Ask both questions independently where necessary.

## Define permissions and children

The permission API supports these relevant operations:

- `context.AddGroup(name, displayName)` creates a named group. Groups have a localizable `DisplayName`, a `Permissions` collection, and a `Properties` dictionary/indexer.
- `group.AddPermission(name, displayName, multiTenancySide, isEnabled)` defines a top-level permission.
- `permission.AddChild(name, displayName, multiTenancySide, isEnabled)` defines a child permission. A child has its own side and enabled arguments; explicitly set them when they differ from defaults or parent intent.
- Permission definitions expose `Name`, `Parent`, `DisplayName`, `Children`, `MultiTenancySide`, `Providers`, `StateCheckers`, `Properties`, and `IsEnabled`.
- `permission.WithProviders(providerNames...)` appends allowed provider names. Do not set a provider list just to restate an existing default.
- `permission.WithProperty(key, value)` attaches metadata; `permission[key]` and `permission.Properties` expose that metadata.

Preserve the existing permission naming hierarchy, localization-key construction, and CRUD helper style. When adding a permission, check that the corresponding service/controller authorization attributes and grants use the exact defined constant. Do not weaken authorization by applying a broad policy when a narrower permission exists.

## Add a custom state checker

`PermissionDefinition.StateCheckers` contains `ISimpleStateChecker<PermissionDefinition>` instances in the installed framework. A checker can make a permission conditionally available; for example, it can verify that a required feature or module is installed and enabled.

When a state condition is requested:

1. Ask exactly what state must be true, what happens if it is unknown/unavailable, and which feature/module system is authoritative.
2. Implement a focused checker for `PermissionDefinition` using the installed simple-state-checker context and asynchronous check contract. Keep feature discovery and feature enablement checks distinct if the system distinguishes them.
3. Attach the checker to the affected permission's `StateCheckers` collection using the project/framework's supported construction and dependency-injection lifecycle. Do not manually instantiate a checker with dependencies unless the definition-provider lifecycle makes that safe.
4. Add tests for enabled, disabled, and unavailable feature states when the checker introduces behavior. A false check must deny/disable the permission according to the framework's checker semantics.

Do not use a custom state checker as a replacement for `MultiTenancySide`, an allowed provider, or ordinary authorization policies. Each solves a different problem.

## Localization and authorization wiring

Permission display names are localizable, not ordinary displayed strings. Follow the permission provider's pattern of constructing a `LocalizableString` from a key in `FluentCMSResource`. Ensure the base English key and translations exist in all supported culture JSON files. Do not translate permission identifiers or use display text as a permission name.

After defining the permission:

- Wire the exact permission constant into the service/controller policy that protects the operation.
- Preserve both class-level access checks and operation-specific child checks when that is the established pattern.
- Check that child grants behave as intended relative to the parent in the installed authorization system.
- Update tests or seed/configuration behavior if permissions are persisted or granted explicitly.

## Validate

1. Build the project containing the permission definition and all affected service/API projects.
2. Verify permission names and parent-child relationships against constants and every policy reference.
3. Verify the selected `MultiTenancySide`, providers, and `IsEnabled` value for each changed definition.
4. Verify custom properties are consumed and state checkers are registered/attached and behave as specified.
5. Validate localized keys and locale coverage using `.github/skills/fluentcms-localization/SKILL.md`.
6. Report defined permissions, tenant side, allowed providers, enabled/state-checker choices, localization changes, and build/test results.
