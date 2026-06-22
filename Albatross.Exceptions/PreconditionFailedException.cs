using System;

namespace Albatross.Exceptions {
	/// <summary>
	/// Thrown when a caller's explicitly stated precondition about the current state of a resource
	/// is not met, and the operation is therefore not attempted.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Use this exception when the caller provides an assertion about expected state — such as a
	/// version number, timestamp, or ETag — and that assertion does not match the actual state.
	/// The key distinction is that the caller explicitly declared what they expected, and reality
	/// disagrees, so the system refuses to proceed.
	/// </para>
	/// <para><strong>Examples:</strong></para>
	/// <list type="bullet">
	///   <item>The caller sends <c>If-Match: "v3"</c> but the resource is at version 5.</item>
	///   <item>An update request includes <c>expectedVersion: 7</c> but the row is at version 9.</item>
	///   <item>A CLI command passes <c>--if-exists</c> but the target resource does not exist.</item>
	///   <item>A conditional delete requires the record to be in "draft" status, but it is "published".</item>
	/// </list>
	/// <para><strong>How it differs from <see cref="ConflictException"/>:</strong></para>
	/// <list type="bullet">
	///   <item>
	///     <see cref="PreconditionFailedException"/>: the caller stated an expectation and the system
	///     checked it before doing anything. The operation was <em>never attempted</em>.
	///   </item>
	///   <item>
	///     <see cref="ConflictException"/>: the operation was attempted (or evaluated) and found to
	///     clash with current state — e.g. a duplicate key or uniqueness violation.
	///   </item>
	/// </list>
	/// <para>
	/// In HTTP terms this maps to <c>412 Precondition Failed</c>, whereas <see cref="ConflictException"/>
	/// maps to <c>409 Conflict</c>.
	/// </para>
	/// </remarks>
	public class PreconditionFailedException : Exception {
		public const int StatusCode = 412;
		public PreconditionFailedException() { }
		public PreconditionFailedException(string? message) : base(message) { }
		public PreconditionFailedException(string? msg, Exception? inner) : base(msg, inner) { }
	}
}