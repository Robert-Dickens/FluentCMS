---
name: mapperly-mapping
description: Create or update Mapperly mappings between caller-specified source and destination classes or records. Use when the caller asks for one-way or two-way mappings, custom property mappings, constructor mappings, ignored members, extra properties, enum settings, or nested collection conversions.
---

# Mapperly Mapping Skill

Create mappings for the exact source and destination types requested. Follow the project’s installed Mapperly and foundation-package conventions; do not assume another project uses the same base types or version.

## Gather mapping requirements

Before editing, establish:

- Source and destination type names, namespaces, and projects.
- Direction: one-way source-to-destination, or two-way.
- Whether the mapper must be registered/discovered by the project’s object-mapping setup.
- Whether either type or a base type implements `IHasExtraProperties`.
- Whether properties differ in name, shape, type, or nesting; whether constructor parameters must be mapped; and whether collections need custom child mapping.
- Any desired `MapperAttribute` options, especially strictness for unmapped members, constructor selection, enum behavior, null handling, cloning, and reference handling.

Inspect both types, their base types, relevant constructors, and existing mapper conventions before writing the declarations. Do not substitute persistence/database types for the source or destination types named by the caller.

## Choose the base type and mapping methods

Use the project's Mapperly base classes, when available:

- One-way mapping: `MapperBase<TSource, TDestination>`.
- Two-way mapping: `TwoWayMapperBase<TSource, TDestination>`.

For a one-way mapper, implement the base-required source-to-destination `Map` methods following the local signature pattern. For a two-way mapper, add both `Map` and `ReverseMap` methods, including both create-new and map-into-existing-destination overloads if required by the base type. Preserve exact source/destination order in each method; verify signatures against the installed base class.

Apply `[Mapper]` from `Riok.Mapperly.Abstractions` to the partial mapper class. Keep generated partial methods abstract declarations as required by the base; let the generator implement them. Use a descriptive mapper class name and the project’s normal namespace and registration/discovery pattern.

## Configure MapperAttribute options

Prompt for or select `[Mapper(...)]` options only when useful for the requested map. The installed Mapperly version is authoritative. Common `MapperAttribute` options in Mapperly 4.3.0 are:

- `RequiredMappingStrategy`: `None`, `Source`, `Target`, or `Both`; controls warnings for unmapped source and/or target members. Prefer strict target coverage when the project expects every destination member to be explicitly mapped. Do not silence member warnings globally to avoid investigating them.
- `RequiredEnumMappingStrategy`: unmapped enum-member warning policy.
- `PropertyNameMappingStrategy`: case-sensitive or case-insensitive name matching.
- `EnumMappingStrategy`, `EnumNamingStrategy`, and `EnumMappingIgnoreCase`: enum value and name mapping behavior.
- `IncludedMembers` and `IncludedConstructors`: access levels considered for members and constructors.
- `PreferParameterlessConstructors`: whether a parameterless constructor should take priority over parameterized constructors.
- `ThrowOnMappingNullMismatch`, `ThrowOnPropertyMappingNullMismatch`, and `AllowNullPropertyAssignment`: null mismatch and assignment behavior.
- `UseDeepCloning` and `UseReferenceHandling`: object-copy and reference-cycle behavior.
- `EnabledConversions`: conversions Mapperly may generate.
- `IgnoreObsoleteMembersStrategy`: handling of obsolete members.
- `AutoUserMappings`: discovery of user-written mapping methods.

Do not set unrelated options just to populate the attribute. Explain choices that affect behavior, and ask the caller when the desired policy cannot be inferred safely.

## Map members with different names or shapes

Use `[MapProperty(source, target)]` on the individual mapping method to explicitly map renamed members. Prefer `nameof()` for simple member names:

`[MapProperty(nameof(Source.SomeValue), nameof(Destination.OtherValue))]`

Mapperly also accepts member paths. C# `nameof(Source.Nested.Value)` evaluates to only `"Value"`, not the dotted path. For a nested path, use a string literal such as `[MapProperty("Nested.Value", nameof(Destination.Value))]` (and an explicit dotted target string when the target is nested). Add the corresponding attributes to every direction and overload that needs the mapping; mappings do not automatically imply reverse mappings.

`MapPropertyAttribute.Use` names a unique custom mapping method to convert the selected member. Use `nameof(CustomMapMethod)` for that method reference. `StringFormat`, `FormatProvider`, and `SuppressNullMismatchDiagnostic` are available where their specific conversion behavior is required; do not use diagnostic suppression instead of addressing a real null mismatch.

For constructor-bound destination parameters, map to the constructor parameter/member name using a string literal as the target, for example `[MapProperty(nameof(Source.Value), "value")]`. Do not use `nameof()` for a constructor parameter target. Inspect the actual constructor and verify the generated mapping selects the intended overload; `IncludedConstructors` and `PreferParameterlessConstructors` can affect selection.

## Ignore intentionally unmapped members

Resolve Mapperly unmapped-member warnings one member at a time; do not use broad suppression as a substitute for deciding how each value should be handled.

- `[MapperIgnoreSource(nameof(Source.Member))]` declares a source member intentionally unused by that mapping method.
- `[MapperIgnoreTarget(nameof(Destination.Member))]` declares a target member intentionally not populated by that mapping method.

Put the attributes on each affected method, not just one overload or direction. For reverse maps, reassess source and target roles: a member ignored as a forward source may be a reverse target, so use the appropriate attribute for that method. Use a fully qualified type or an unambiguous alias in `nameof()` where needed. Ignore only members that are intentionally not part of that mapping; if a value should transfer, map it rather than ignoring it.

## Preserve `IHasExtraProperties`

If either mapped type implements `IHasExtraProperties` directly or through a base class, add the project's `[MapExtraProperties]` attribute to the Mapperly partial class. In this codebase it comes from `ByteLabs.Foundations.Mapperly`. Apply it at class level and keep it when adding `[Mapper]`; verify the correct namespace and package in other projects. Do not ignore `ExtraProperties` or assume the other side has the interface without checking both type hierarchies.

## Convert nested or collection members

When a child member or collection cannot be mapped correctly by convention, provide a custom conversion method and select it with `MapProperty(..., Use = nameof(CustomMethod))`. For bidirectional mapping, write and select the appropriate converter in each direction. Keep converters explicit about null and empty behavior, collection element types, and whether an empty input should yield `null` or an empty collection.

If a converter delegates to child maps, use the project’s object mapper for the correct context (for example `IObjectMapper<TContext>`) and ensure it is injected/registered consistently with existing mappers. Follow the local child-mapping pattern rather than duplicating element mapping logic.

## Validate

After implementation:

1. Build the mapper’s project to run source generation and inspect all Mapperly diagnostics.
2. Resolve warnings by fixing mappings or adding precise ignores where omission is intentional; specifically verify RMG012 target members and source-member warnings under the selected `RequiredMappingStrategy`.
3. Inspect generated code or compiler output for constructor selection, custom `Use` methods, extra properties, and both directions.
4. Build the containing solution or run relevant tests when practical.
5. Report the exact types and directions mapped, significant configuration choices, remaining diagnostics, and validation result.
