# Release Notes

## 1.0.0

Initial release of the Albatross.Exceptions vocabulary.

### Albatross Exceptions
- `ValidationException` (422) - Input is structurally or semantically invalid.
- `NotAuthenticatedException` (401) - The caller's identity has not been established.
- `ForbiddenException` (403) - The caller is authenticated but lacks permission.
- `NotFoundException` (404) - A required entity or resource cannot be found.
- `ConflictException` (409) - The operation conflicts with the current state of a resource.
- `PreconditionFailedException` (412) - The caller's stated precondition about resource state is not met.

### Documented Framework Exceptions
The following `System` exceptions are documented as part of the vocabulary for consistent usage and hosting-layer mapping:
- `ArgumentException` (400)
- `NotSupportedException` (501)
- `TimeoutException` (408)
- `OperationCanceledException` (-)
