# Albatross.Exceptions

status: active
created: 2026-06-21T10:00:00-04:00
updated: 2026-06-21T10:30:00-04:00
----

## Business Requirements

Provide a shared vocabulary of common semantic exception types for use across Albatross libraries and applications. This assembly defines vocabulary, not behavior — it names the error conditions that recur across projects so that producers and consumers share a common language. It exists so that multiple projects can throw and catch the same well-known exception types without duplicating definitions or taking on heavier dependencies.

The assembly is deliberately scoped to **common semantic exceptions** — exceptions whose meaning is widely understood and reused across domains (e.g. "not found", "conflict", "unauthorized"). It is not a dumping ground for every custom exception a project might need; project-specific exceptions belong in their own assemblies.

## Technical Design

- **Declarations only — no implementations**: The assembly contains exception classes and interface contracts. No extension methods, no middleware, no mapping logic, no implementations. An interface like `ISemanticExceptionConverter` is a declaration (it defines a contract), not behavior — implementations live in consuming assemblies.
- **Target framework**: `netstandard2.1` — maximizes compatibility across .NET consumers.
- **NuGet package**: Packaged with icon, license, and per-project README for distribution as a private NuGet package.
- **Naming convention**: Each exception is named for the semantic condition it represents (e.g. `NotFoundException`), not for the HTTP status code or infrastructure concern it might map to. Mapping exceptions to HTTP responses is the responsibility of the hosting layer, not this assembly.

## Key Design Decisions

- **Vocabulary, not behavior**: The assembly defines what error conditions are called, not what happens when they occur. Utilities, mapping logic, middleware, or any runtime behavior belong in other assemblies. This keeps the dependency lightweight and its purpose unambiguous. Interface contracts (declarations without implementations) are permitted but each addition should be scrutinized against the same standard — every new type must justify its presence.
- **Inclusion criteria**: An exception belongs in this assembly only if it meets all five conditions:
  1. It is meaningful across multiple transports/frameworks.
  2. It maps clearly to a stable semantic category.
  3. It is not tied to a product/domain/entity.
  4. It can be reasonably mapped to HTTP/API/CLI/logging behavior.
  5. It is unlikely to change.

  If an exception fails any of these, it belongs in the project that needs it, not here.
- **netstandard2.1 target**: Chosen for maximum compatibility rather than targeting a specific .NET version. This allows the package to be consumed by any modern .NET project.

## Open Questions

- Should `StatusCode` remain as a `const int` on each exception class, or should the HTTP mapping live only in documentation? The current `const` approach assumes every exception in this assembly has exactly one HTTP status code. If a future exception has no clear HTTP mapping, the contract becomes inconsistent.

## Dependencies & Constraints

- No runtime dependencies beyond the .NET base class library.
- Must remain `netstandard2.1` unless a compelling reason forces a target change.
