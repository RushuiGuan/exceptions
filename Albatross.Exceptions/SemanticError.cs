namespace Albatross.Exceptions {
	/// <summary>
	/// The closed set of semantic error categories shared across Albatross libraries and applications.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This enum names error conditions as values rather than as types, so a single category can describe
	/// both the exceptions defined in this assembly and the framework exceptions that belong to the same
	/// vocabulary (e.g. <see cref="System.ArgumentException"/>). Classifying an arbitrary exception into one
	/// of these categories is the responsibility of the hosting layer, which can see exception types this
	/// assembly does not own.
	/// </para>
	/// </remarks>
	public enum SemanticError {
		/// <summary>Input is structurally or semantically invalid and cannot be processed as-is.</summary>
		Validation,
		/// <summary>The caller's identity has not been established.</summary>
		NotAuthenticated,
		/// <summary>The caller is authenticated but lacks permission.</summary>
		Forbidden,
		/// <summary>A required entity or resource cannot be found.</summary>
		NotFound,
		/// <summary>The operation was attempted but conflicts with the current state of a resource.</summary>
		Conflict,
		/// <summary>The caller's explicitly stated precondition about resource state is not met.</summary>
		PreconditionFailed,
		/// <summary>A value required by an operation or provider contract was null or absent.</summary>
		MissingRequiredValue,
		/// <summary>An argument passed to a method is invalid — a method-contract violation at the API boundary.</summary>
		BadArgument,
		/// <summary>The operation is recognized but deliberately not supported.</summary>
		NotSupported,
		/// <summary>The operation exceeded its allotted time.</summary>
		Timeout,
		/// <summary>The operation was cancelled, typically via a cancellation token.</summary>
		Canceled,
	}
}
