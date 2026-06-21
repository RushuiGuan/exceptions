using System;

namespace Albatross.Exceptions {
	/// <summary>
	/// Thrown when an operation was attempted but conflicts with the current state of a resource.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Use this exception when the system evaluates or attempts an operation and discovers that it
	/// clashes with existing state. Unlike <see cref="PreconditionFailedException"/>, where the caller
	/// explicitly states an expectation that is checked before the operation begins, a conflict is
	/// discovered during the operation itself.
	/// </para>
	/// <para><strong>Examples:</strong></para>
	/// <list type="bullet">
	///   <item>Inserting a record that violates a unique constraint (duplicate key, duplicate name).</item>
	///   <item>Creating a resource that already exists when the operation is "create only, not upsert".</item>
	///   <item>An optimistic concurrency check fails because another writer modified the row after it was read.</item>
	///   <item>A state transition is invalid — e.g. attempting to publish a record that is already published.</item>
	/// </list>
	/// <para><strong>How it differs from <see cref="PreconditionFailedException"/>:</strong></para>
	/// <list type="bullet">
	///   <item>
	///     <see cref="ConflictException"/>: the operation was attempted (or evaluated) and found to
	///     clash with current state. The conflict is discovered by the system, not asserted by the caller.
	///   </item>
	///   <item>
	///     <see cref="PreconditionFailedException"/>: the caller explicitly stated an expectation
	///     (version, ETag, status) and the system checked it before doing anything.
	///   </item>
	/// </list>
	/// <para>
	/// In HTTP terms this maps to <c>409 Conflict</c>.
	/// </para>
	/// </remarks>
	public class ConflictException : Exception {
		public const int StatusCode = 409;
		public ConflictException() { }
		public ConflictException(string msg) : base(msg) { }
	}
}