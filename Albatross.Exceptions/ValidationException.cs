using System;
using System.Collections.Generic;
using System.Linq;

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
	/// This exception carries a single message. When the failure is a list of per-field errors,
	/// throw <see cref="FieldValidationException"/> instead. The two types are unrelated, so a
	/// handler that needs both must catch both.
	/// </para>
	/// <para>
	/// In HTTP terms this maps to <c>422 Unprocessable Entity</c>.
	/// </para>
	/// </remarks>
	public class ValidationException : Exception {
		public const int StatusCode = 422;
		/// <summary>The message used when no message, or an empty one, is supplied.</summary>
		public const string StandardMessage = "Validation failed";

		public ValidationException() : base(CreateMessageFromString(StandardMessage)) { }
		/// <param name="message">Falls back to <see cref="StandardMessage"/> when null or empty.</param>
		public ValidationException(string? message) : this(CreateMessageFromString(message), null) { }
		/// <param name="message">Falls back to <see cref="StandardMessage"/> when null or empty.</param>
		/// <param name="inner">The exception that caused the validation failure.</param>
		public ValidationException(string? message, Exception? inner) : base(CreateMessageFromString(message), inner) { }

		static string CreateMessageFromString(string? error) {
			if (string.IsNullOrEmpty(error)) {
				return StandardMessage;
			} else {
				return error;
			}
		}
	}

	/// <summary>
	/// Thrown when input fails validation and the failure is reported as a list of per-field errors.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Use it when the caller needs to know which fields are wrong, such as when a request object is
	/// validated as a whole. For a failure described by a single message, throw
	/// <see cref="ValidationException"/> instead. The two types are unrelated, so catching
	/// <see cref="ValidationException"/> does not catch this one.
	/// </para>
	/// <para>
	/// The message joins the errors with <c>"; "</c>, each formatted by <see cref="ValidationError.ToString"/>.
	/// With no errors, the message is <see cref="ValidationException.StandardMessage"/>.
	/// </para>
	/// <code>
	/// throw new FieldValidationException(
	///     new ValidationError("cenCode", "Cen code 'ABCDE' is invalid"),
	///     new ValidationError("Base and quote currency are the same"));
	/// // Message: [cenCode] Cen code 'ABCDE' is invalid; Base and quote currency are the same
	/// </code>
	/// <para>
	/// In HTTP terms this maps to <c>422 Unprocessable Entity</c>.
	/// </para>
	/// </remarks>
	public class FieldValidationException : Exception {
		public const int StatusCode = 422;

		/// <param name="errors">Enumerated once; the exception keeps them in the order given.</param>
		public FieldValidationException(params IEnumerable<ValidationError> errors) : this(errors, null) { }
		/// <param name="errors">Enumerated once; the exception keeps them in the order given.</param>
		/// <param name="inner">The exception that caused the validation failure.</param>
		public FieldValidationException(IEnumerable<ValidationError> errors, Exception? inner) : this(errors.ToArray(), inner) { }

		FieldValidationException(ValidationError[] errors, Exception? inner) : base(CreateMessage(errors), inner) {
			this.Errors = errors;
		}

		public IReadOnlyList<ValidationError> Errors { get; } = Array.Empty<ValidationError>();

		// [cenCode] Cen code 'ABCDE' is invalid; [marketId] Market id 0 is invalid
		static string CreateMessage(ValidationError[] errors) {
			if (errors.Length == 0) {
				return ValidationException.StandardMessage;
			}
			return string.Join<ValidationError>("; ", errors);
		}
	}
}