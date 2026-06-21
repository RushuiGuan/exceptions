using System;

namespace Albatross.Exceptions {
	/// <summary>
	/// Thrown when the caller is authenticated but lacks permission to perform the requested operation.
	/// </summary>
	/// <remarks>
	/// <para>
	/// Use this exception when the caller's identity is known but they are not authorized for the
	/// requested action. The system knows who the caller is; the answer is "no, you may not."
	/// Re-authenticating will not help — the caller needs different permissions.
	/// </para>
	/// <para><strong>Examples:</strong></para>
	/// <list type="bullet">
	///   <item>A non-admin user attempts to delete another user's account.</item>
	///   <item>A read-only service account attempts a write operation.</item>
	///   <item>A user attempts to access a resource belonging to a different tenant.</item>
	///   <item>A CLI command targets an environment the caller's role does not cover.</item>
	/// </list>
	/// <para><strong>How it differs from <see cref="NotAuthenticatedException"/>:</strong></para>
	/// <list type="bullet">
	///   <item>
	///     <see cref="ForbiddenException"/>: identity is established, but the caller lacks the
	///     required permissions. The fix is to grant access, not to log in again.
	///   </item>
	///   <item>
	///     <see cref="NotAuthenticatedException"/>: identity is not established. The caller needs
	///     to provide valid credentials.
	///   </item>
	/// </list>
	/// <para>
	/// In HTTP terms this maps to <c>403 Forbidden</c>.
	/// </para>
	/// </remarks>
	public class ForbiddenException : Exception {
		public const int StatusCode = 403;
		public ForbiddenException() { }
		public ForbiddenException(string msg) : base(msg) { }
	}
}