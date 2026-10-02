---
name: fluentcms-localization
description: Add or update FluentCMS localization keys across culture JSON files, or replace a user-facing hard-coded string with a localized resource lookup. Use when asked to register a localization property/key with a meaning or localize a string literal.
---

# FluentCMS Localization Skill

Use this skill to add or update a FluentCMS localization entry, or convert a user-facing literal in code to a localized lookup. Keep the change aligned with the registered resource and all supported cultures.

## Resource registration and file locations

The CMS resource is `FluentCMSResource`, declared in `FluentCMS.Localization` and named `[LocalizationResourceName("FluentCMS")]`. Its JSON resources are in:

`Modules/Cms/src/Domain.Abstractions/Localization/FluentCMS/<culture>.json`

`Cms.Domain.Abstractions.csproj` embeds JSON files under `Localization`. `FluentCMSDomainAbstractionsModule` registers the resource with base culture `en`, adds validation resource base types, and loads virtual JSON from `/Localization/` plus `FluentCMSConsts.ModuleName`. A normal new key in this resource does not require changing module registration or the project file. Only change registration when the user requests a new resource or the existing registration is demonstrably incorrect.

The current locale files are `ar.json`, `de-DE.json`, `en.json`, `es.json`, `fi.json`, `fr.json`, `it.json`, `pt-BR.json`, `ru.json`, `sk.json`, `sl.json`, and `tr.json`. Each uses this structure:

- `culture`: the file's culture name.
- `texts`: a JSON object mapping stable resource keys to translated text.

Always enumerate the current locale files before editing; do not assume this list remains exhaustive.

## Interpret the request

When the user asks to register a localization property/key `x` with meaning `y`:

1. Treat `x` as the requested resource key if it is clearly a stable identifier. If they say "property" without giving a key, choose a stable key from its feature and meaning, or ask only if the intended key cannot be inferred without ambiguity.
2. Treat `y` as the intended meaning, usually the English/base-culture text. Keep the key independent of the display text; never use a translated phrase as a resource key.
3. Search every locale file for the key first. If it already exists, preserve or update it only as the request requires; do not duplicate keys or overwrite a differing translation silently.
4. Add the key to `en.json` and every other supported culture JSON file. Translate the meaning naturally for each culture, preserving product names and technical terms where appropriate. If a translation cannot be supplied reliably, call that out rather than pretending it is verified.
5. Preserve each file's culture metadata, JSON formatting, neighboring key conventions, and unrelated entries.

Current permission entries use keys such as `Permission:Create` and `Permission:FluentCMS:SiteContent:Content`; the permission provider constructs resource keys with a `Permission:` prefix. For a new permission, inspect `FluentCMSPermissionDefinitionProvider` and permission constants to ensure the key exactly matches the code. For other UI text, choose a feature-scoped stable key consistent with nearby resources; do not force a `Permission:` prefix.

## Convert a user-facing code literal

When asked to localize an existing literal:

1. Read the containing method/component and identify where and when the text is shown. Keep the message's meaning, punctuation, capitalization, formatting, and parameter values intact.
2. Confirm the literal is genuinely user-facing. Do not localize protocol tokens, routes, identifiers, enum values, permission names, action names, SQL/config keys, or developer-only logs unless explicitly requested.
3. Reuse an existing key if it expresses the same text and context. Otherwise choose a stable feature-scoped key and add it to all locale files as above.
4. Replace the literal using the localization API appropriate to that layer. In permission definitions and APIs expecting a `LocalizableString`, follow the provider pattern: `LocalizableString.Create<FluentCMSResource>(key)` (often through the existing `L(...)` helper). For runtime UI text, use the localization abstraction already used or supported in that component/service; a standard .NET option is an injected `IStringLocalizer<FluentCMSResource>` when the project supports it. Do not return the raw resource key as display text or use `LocalizableString` where a resolved string is required.
5. For formatted messages, preserve dynamic values as localization arguments/placeholders rather than concatenating a localized fragment with hard-coded words. Follow the framework's formatting and HTML-safety conventions.
6. Keep localization key names out of user-visible output. If the code uses an exception code or another specialized localization mechanism, inspect and follow that mechanism rather than replacing it with an unrelated localizer.

## Validation

After editing:

1. Parse every JSON file in the resource directory and verify its `culture` matches its file name.
2. Compare all locale key sets with `en.json`; report missing and unexpected keys. Ensure newly added translations are nonempty and no duplicate keys were introduced.
3. Search the changed code for remaining occurrences of the literal when full localization was requested. Verify the new key is referenced through the correct resource.
4. Build the affected project(s), especially `Cms.Domain.Abstractions` when changing embedded resources and the code project's project when changing a literal.
5. Report the key, updated cultures, changed code location if applicable, and validation results. Distinguish successful JSON/build checks from human translation review.
