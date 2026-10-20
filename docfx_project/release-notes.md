# Release Notes

## 1.0.6

Changes since 1.0.1. Versions released in between are unlisted; upgrade from 1.0.1 directly to 1.0.6.

### Breaking changes
- Validation failures that carry field errors now use the new `FieldValidationException`. `ValidationException` no longer has the `ValidationException(IEnumerable<ValidationError>)` constructor or the `Errors` property.
  - Replace `new ValidationException(errors)` with `new FieldValidationException(errors)`.
  - Code that catches `ValidationException` to read `Errors` must catch `FieldValidationException` instead. The two types are unrelated, so a handler that maps validation failures must check for both.
- The message built from field errors no longer starts with `Validation failed: `. It is now just the errors joined with `; `, for example `[cenCode] Cen code 'ABCDE' is invalid; Base and quote currency are the same`. With no errors, the message is still `Validation failed`.
- `ValidationException` uses `Validation failed` as its message when none is given: for the parameterless constructor, and when the message passed in is null or empty. Previously these produced the default .NET exception message or an empty string.

### Added
- `FieldValidationException` (422): input is invalid, reported as a list of `ValidationError` items in `Errors`, kept in the order given. Its constructors take the errors as `params`, or with an inner exception.
- `ValidationError(string error)` creates an error about the object as a whole, with no field.
- `ValidationError.ToString()` returns `[field] error`, or just the error when there is no field.
- `ValidationException.StandardMessage` holds the `Validation failed` default message.
- `TryGetSemanticError` maps `FieldValidationException` to `SemanticError.Validation`.

## 1.0.0

Initial release of the Albatross.Exceptions vocabulary.

### Albatross Exceptions
- `ValidationException` (422) - Input is structurally or semantically invalid.
- `NotAuthenticatedException` (401) - The caller's identity has not been established.
- `ForbiddenException` (403) - The caller is authenticated but lacks permission.
- `NotFoundException` (404) - A required entity or resource cannot be found.
- `ConflictException` (409) - The operation conflicts with the current state of a resource.
- `PreconditionFailedException` (412) - The caller's stated precondition about resource state is not met.
- `MissingRequiredValueException` - A value required by an operation or provider contract was null or absent.

### Documented Framework Exceptions
The following `System` exceptions are documented as part of the vocabulary for consistent usage and hosting-layer mapping:
- `ArgumentException` (400)
- `NotSupportedException` (501)
- `TimeoutException` (408)
- `OperationCanceledException` (-)

### Classification
- `SemanticError` enum - names every condition above as a value, so owned and framework exceptions share one vocabulary.
- `SemanticErrorExtensions.TryGetSemanticError` - the default classifier that maps any known exception to its `SemanticError`.
- `ISemanticExceptionConverter` - contract the hosting layer can implement to recognize additional exception types before falling back to the default classifier.
