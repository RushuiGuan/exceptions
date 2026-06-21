using System;

namespace Albatross.Exceptions {
	/// <summary>
	/// Thrown when input fails validation — the request is structurally or semantically invalid
	/// and cannot be processed as-is.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Use this exception when the caller's input does not meet the requirements for the operation.
	/// The problem is with the request itself — not with the state of the system — and the caller
	/// must fix the input before retrying.
	/// </para>
	/// <para><strong>Examples:</strong></para>
	/// <list type="bullet">
	///   <item>A required field is missing or empty.</item>
	///   <item>A value is out of range (e.g. negative quantity, date in the past).</item>
	///   <item>A string does not match the expected format (e.g. invalid email, malformed CUSIP).</item>
	///   <item>A combination of fields is logically inconsistent (e.g. start date after end date).</item>
	/// </list>
	/// <para><strong>How it differs from <see cref="ConflictException"/>:</strong></para>
	/// <list type="bullet">
	///   <item>
	///     <see cref="ValidationException"/>: the request is invalid on its own merits, regardless
	///     of the current state of any resource. The same input would fail every time.
	///   </item>
	///   <item>
	///     <see cref="ConflictException"/>: the request is well-formed but clashes with the current
	///     state of the system. The same input might succeed at a different time.
	///   </item>
	/// </list>
	/// <para><strong>How it differs from <see cref="System.ArgumentException"/>:</strong></para>
	/// <list type="bullet">
	///   <item>
	///     <see cref="ValidationException"/> (<c>422</c>): the server parsed the request successfully
	///     but the content is semantically invalid. The syntax is fine; the values are wrong.
	///   </item>
	///   <item>
	///     <see cref="System.ArgumentException"/> (<c>400</c>): the request itself is malformed or
	///     violates the method contract at the API boundary — a structural problem, not a content problem.
	///   </item>
	/// </list>
	/// <para>
	/// In HTTP terms this maps to <c>422 Unprocessable Entity</c>.
	/// </para>
	/// </remarks>
	public class ValidationException : Exception {
		public const int StatusCode = 422;
		public ValidationException() { }
		public ValidationException(string msg) : base(msg) { }
	}
}