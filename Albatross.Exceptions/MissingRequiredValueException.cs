using System;

namespace Albatross.Exceptions {
	/// <summary>
	/// Thrown when a required value is null or absent where a non-null result was expected.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Use this exception when an operation is expected to produce a value but returns null instead.
	/// The absence is not because the entity doesn't exist in the system (use <see cref="NotFoundException"/>
	/// for that), but because the provider failed to return data it was required to return.
	/// </para>
	/// <para><strong>Examples:</strong></para>
	/// <list type="bullet">
	///   <item>A <c>GetRequired</c> method wraps a nullable lookup: <c>Get() ?? throw new MissingRequiredValueException(...)</c>.</item>
	///   <item>An HTTP client receives a 2xx response with no body from an endpoint that is expected to return content.</item>
	///   <item>A service method returns null for a result that the caller's contract requires to be non-null.</item>
	///   <item>A deserialized response evaluates to null when a value was required.</item>
	/// </list>
	/// <para><strong>Differs from <see cref="NotFoundException"/>:</strong></para>
	/// <para>
	/// <see cref="NotFoundException"/> means the requested entity does not exist in the system.
	/// <see cref="MissingRequiredValueException"/> means a value was expected from an operation or provider
	/// but nothing was returned — a contract violation rather than a missing entity.
	/// </para>
	/// <para>
	/// In HTTP terms this maps to <c>500 Internal Server Error</c>.
	/// </para>
	/// </remarks>
	public class MissingRequiredValueException : Exception {
		public const int StatusCode = 500;
		public MissingRequiredValueException() { }
		public MissingRequiredValueException(string? message) : base(message) { }
		public MissingRequiredValueException(string? msg, Exception? inner) : base(msg, inner) { }
	}
}
