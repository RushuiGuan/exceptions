# Albatross.Exceptions

A shared vocabulary of common semantic exception types. This assembly defines what error conditions are called, not what happens when they occur. Mapping exceptions to HTTP responses, CLI exit codes, or log levels is the responsibility of the hosting layer.

## Albatross Exceptions

| Exception | Status Code | Meaning |
|---|---|---|
| `ValidationException` | 422 | Input is structurally or semantically invalid and cannot be processed as-is. |
| `NotAuthenticatedException` | 401 | The caller's identity has not been established. |
| `ForbiddenException` | 403 | The caller is authenticated but lacks permission. |
| `NotFoundException` | 404 | A required entity or resource cannot be found. |
| `ConflictException` | 409 | The operation was attempted but conflicts with the current state of a resource. |
| `PreconditionFailedException` | 412 | The caller's explicitly stated precondition about resource state is not met. |

## Framework Exceptions

The following `System` exceptions are part of the same semantic vocabulary. They are not redefined in this assembly because the framework already provides them, but they should be used consistently and mapped by the hosting layer alongside the Albatross exceptions.

| Exception | Status Code | Meaning |
|---|---|---|
| `ArgumentException` | 400 | An argument passed to a method is invalid. Use for method-contract violations at the API boundary. Prefer `ValidationException` for request-level input validation. |
| `NotSupportedException` | 501 | The operation is recognized but deliberately not supported. The system understands what was asked but will not do it. |
| `TimeoutException` | 408 | The operation exceeded its allotted time. Use when application code enforces a deadline, not for infrastructure-level timeouts handled by the framework. |
| `OperationCanceledException` | - | The operation was cancelled, typically via a `CancellationToken`. No HTTP response is expected — the request was abandoned by the caller. |

## Classifying exceptions

The `SemanticError` enum names every condition in both tables above as a *value* rather than a type. This is what lets a single category describe both the Albatross exceptions and the framework exceptions — a framework type can never implement an interface this assembly owns, but it can be mapped to an enum value.

`TryGetSemanticError` is the default classifier. It maps every exception in both tables to its `SemanticError` and returns `false` for anything it does not recognize:

```csharp
if (exception.TryGetSemanticError(out var error)) {
    // map `error` to an HTTP status code, CLI exit code, or log level in the hosting layer
}
```

The hosting layer remains the owner of behavior. It may run its own `ISemanticExceptionConverter` chain first — to recognize transport- or library-specific exceptions — and fall back to `TryGetSemanticError` for the shared vocabulary.

## Composing the converter and the classifier

`ISemanticExceptionConverter` and `TryGetSemanticError` form a two-stage pipeline:

```
arbitrary Exception ──ISemanticExceptionConverter──▶ semantic Exception ──TryGetSemanticError──▶ SemanticError
   (normalize)                                          (classify)
```

- **`ISemanticExceptionConverter`** is the *normalize* stage. It turns exceptions outside the vocabulary — a library's `DbException`, a third-party client error, a domain-specific exception — into one of the known types.
- **`TryGetSemanticError`** is the *classify* stage. It has no opinion about where an exception came from; it maps a known type to its `SemanticError` value.

The `Resolve` extension method wires them together — classify directly first, and only normalize when the exception is not already in the vocabulary:

```csharp
public static SemanticError? Resolve(this Exception ex, params IEnumerable<ISemanticExceptionConverter> converters) {
    if (ex.TryGetSemanticError(out var error))          // already in the vocabulary?
        return error;
    foreach (var converter in converters) {             // otherwise try to normalize it
        if (converter.TryConvert(ex, out var converted) && converted.TryGetSemanticError(out error))
            return error;
    }
    return null;                                         // unknown — let it bubble as a 500
}
```

`TryGetSemanticError` already recognizes the four framework exceptions directly, so converters never need to handle those. Their job is strictly the exceptions that are not already in the shared vocabulary: the enum classifier owns the known set, and converters extend it.
