# Release Notes

## 1.0.6

Changes since 1.0.0. Versions released in between are unlisted; upgrade from 1.0.0 directly to 1.0.6.

### Added
- `FieldValidationException` (422): input is invalid, reported as a list of `ValidationError` items in `Errors`, kept in the order given. Its constructors take the errors as `params`, or with an inner exception. Its message joins the errors with `; `, for example `[cenCode] Cen code 'ABCDE' is invalid; Base and quote currency are the same`, or is `Validation failed` when there are none.
  - `ValidationException` and `FieldValidationException` are unrelated types, so a handler that maps validation failures must check for both.
- `ValidationError`: one validation failure, with an optional field.
  - `ValidationError(string field, string error)` ties the error to a field, such as `legs[0].cenCode`.
  - `ValidationError(string error)` creates an error about the object as a whole, with no field.
  - `ToString()` returns `[field] error`, or just the error when there is no field.
- `ValidationException.StandardMessage` holds the `Validation failed` default message.
- `TryGetSemanticError` maps `FieldValidationException` to `SemanticError.Validation`.

### Changed
- `ValidationException` uses `Validation failed` as its message when none is given: for the parameterless constructor, and when the message passed in is null or empty. Previously these produced the default .NET exception message or an empty string.

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
