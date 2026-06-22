using System;

namespace Albatross.Exceptions {
	/// <summary>
	/// Thrown when a required entity or resource cannot be found.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Use this exception when the caller requests a specific resource by identity (primary key, unique
	/// identifier, or path) and no matching resource exists. The caller expected the resource to be there;
	/// its absence is an error, not a valid outcome.
	/// </para>
	/// <para><strong>Examples:</strong></para>
	/// <list type="bullet">
	///   <item>A repository lookup by primary key returns no result.</item>
	///   <item>A REST endpoint receives <c>GET /accounts/42</c> but account 42 does not exist.</item>
	///   <item>A CLI command targets a named configuration that has not been created.</item>
	///   <item>A service resolves a foreign-key reference and the referenced entity is missing.</item>
	/// </list>
	/// <para><strong>When NOT to use:</strong></para>
	/// <list type="bullet">
	///   <item>
	///     Search or filter queries that return zero results — an empty result set is a valid outcome,
	///     not an error. Return an empty collection instead.
	///   </item>
	///   <item>
	///     Non-key lookups (e.g. find-by-name) where the caller is prepared to handle absence.
	///     Return <c>null</c> or an empty optional and let the caller decide.
	///   </item>
	/// </list>
	/// <para>
	/// In HTTP terms this maps to <c>404 Not Found</c>.
	/// </para>
	/// </remarks>
	public class NotFoundException : Exception {
		public const int StatusCode = 404;
		public NotFoundException() { }
		public NotFoundException(string? message) : base(message) { }
		public NotFoundException(string? msg, Exception? inner) : base(msg, inner) { }
	}
}