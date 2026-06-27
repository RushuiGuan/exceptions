# Albatross.Exceptions

status: active
created: 2026-06-21T10:00:00-04:00
updated: 2026-06-26T00:00:00-04:00
----

## Business Requirements

Provide a shared vocabulary of common semantic exception types for use across Albatross libraries and applications. This assembly defines vocabulary, not behavior — it names the error conditions that recur across projects so that producers and consumers share a common language. It exists so that multiple projects can throw and catch the same well-known exception types without duplicating definitions or taking on heavier dependencies.

The assembly is deliberately scoped to **common semantic exceptions** — exceptions whose meaning is widely understood and reused across domains (e.g. "not found", "conflict", "unauthorized"). It is not a dumping ground for every custom exception a project might need; project-specific exceptions belong in their own assemblies.

## Technical Design

- **Declarations and vocabulary mapping — no transport behavior**: The assembly contains exception classes, interface contracts, the `SemanticError` enum, and the default `TryGetSemanticError` classifier. An interface like `ISemanticExceptionConverter` is a declaration (it defines a contract), not behavior. `TryGetSemanticError` is permitted because it is pure vocabulary mapping — it classifies an exception into a `SemanticError` and carries no transport behavior. Mapping a `SemanticError` to an HTTP status code, CLI exit code, or log level remains the hosting layer's responsibility.
- **Target framework**: `netstandard2.1` — maximizes compatibility across .NET consumers.
- **NuGet package**: Packaged with icon, license, and per-project README for distribution as a private NuGet package.
- **Naming convention**: Each exception is named for the semantic condition it represents (e.g. `NotFoundException`), not for the HTTP status code or infrastructure concern it might map to. Mapping exceptions to HTTP responses is the responsibility of the hosting layer, not this assembly.

## Key Design Decisions

- **Vocabulary, not transport behavior**: The assembly defines what error conditions are called and how to classify an exception into the shared vocabulary (`SemanticError` + `TryGetSemanticError`). It does not define what happens when an exception occurs — middleware, transport mapping (HTTP/CLI/logging), and any runtime behavior belong in other assemblies. This keeps the dependency lightweight and its purpose unambiguous. Interface contracts and pure vocabulary mapping are permitted, but each addition should be scrutinized against the same standard — every new type must justify its presence.
- **Inclusion criteria**: An exception belongs in this assembly only if it meets all five conditions:
  1. It is meaningful across multiple transports/frameworks.
  2. It maps clearly to a stable semantic category.
  3. It is not tied to a product/domain/entity.
  4. It can be reasonably mapped to HTTP/API/CLI/logging behavior.
  5. It is unlikely to change.

  If an exception fails any of these, it belongs in the project that needs it, not here.
- **netstandard2.1 target**: Chosen for maximum compatibility rather than targeting a specific .NET version. This allows the package to be consumed by any modern .NET project.

## Key Design Decisions (cont.)

- **Marker interface replaced by a value-based classifier**: `ISemanticException` (a marker-with-metadata interface carrying a `Description`) was removed. A marker interface can only ever be implemented by types this assembly owns, so it could never cover the framework exceptions (`ArgumentException`, `NotSupportedException`, `TimeoutException`, `OperationCanceledException`) that belong to the same vocabulary. The `SemanticError` enum names each condition as a *value*, which can describe owned and framework exceptions equally, and `TryGetSemanticError` is the single classifier over both groups.

## Open Questions

- Should `StatusCode` remain as a `const int` on each exception class, or should the HTTP mapping live only in documentation / the hosting layer alongside the `SemanticError` mapping? The current `const` approach assumes every exception in this assembly has exactly one HTTP status code, and it does not cover the framework exceptions at all — their status codes already live only in documentation. Consolidating all status-code mapping onto `SemanticError` in the hosting layer would make owned and framework exceptions consistent.

## Dependencies & Constraints

- No runtime dependencies beyond the .NET base class library.
- Must remain `netstandard2.1` unless a compelling reason forces a target change.
