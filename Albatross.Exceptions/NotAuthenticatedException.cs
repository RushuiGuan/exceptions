using System;

namespace Albatross.Exceptions {
	/// <summary>
	/// Thrown when the caller's identity has not been established — no credentials were provided
	/// or they could not be verified.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Use this exception when the system requires an authenticated identity but the caller has
	/// not provided one, or the provided credentials are invalid, expired, or unverifiable.
	/// The system does not know who the caller is.
	/// </para>
	/// <para><strong>Examples:</strong></para>
	/// <list type="bullet">
	///   <item>No authentication token or header is present on a request that requires one.</item>
	///   <item>A bearer token is expired or has been revoked.</item>
	///   <item>A certificate or API key cannot be validated against the identity provider.</item>
	///   <item>A CLI command requires login but no cached credential session exists.</item>
	/// </list>
	/// <para><strong>How it differs from <see cref="ForbiddenException"/>:</strong></para>
	/// <list type="bullet">
	///   <item>
	///     <see cref="NotAuthenticatedException"/>: identity is not established. The caller should
	///     provide valid credentials and retry.
	///   </item>
	///   <item>
	///     <see cref="ForbiddenException"/>: identity is established, but the caller does not have
	///     permission. Re-authenticating will not help.
	///   </item>
	/// </list>
	/// <para>
	/// In HTTP terms this maps to <c>401 Unauthorized</c> (the HTTP name is a historical misnomer —
	/// the actual meaning is "not authenticated").
	/// </para>
	/// </remarks>
	public class NotAuthenticatedException : Exception {
		public const int StatusCode = 401;
		public NotAuthenticatedException() { }
		public NotAuthenticatedException(string msg) : base(msg) { }
	}
}