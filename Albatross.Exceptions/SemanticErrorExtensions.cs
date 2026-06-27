using System;
using System.Collections.Generic;

namespace Albatross.Exceptions {
	public static class SemanticErrorExtensions {
		/// <summary>
		/// Classifies an exception into its <see cref="SemanticError"/> category, covering both the exceptions
		/// defined in this assembly and the framework exceptions that share the same vocabulary.
		/// </summary>
		/// <remarks>
		/// This is the default classification. The hosting layer can extend it — for example by running an
		/// <see cref="ISemanticExceptionConverter"/> first to recognize additional exception types — before
		/// falling back to this method.
		/// </remarks>
		/// <returns><c>true</c> if the exception maps to a known category; otherwise <c>false</c>.</returns>
		public static bool TryGetSemanticError(this Exception exception, out SemanticError error) {
			switch (exception) {
				case ValidationException _: error = SemanticError.Validation; return true;
				case NotAuthenticatedException _: error = SemanticError.NotAuthenticated; return true;
				case ForbiddenException _: error = SemanticError.Forbidden; return true;
				case NotFoundException _: error = SemanticError.NotFound; return true;
				case ConflictException _: error = SemanticError.Conflict; return true;
				case PreconditionFailedException _: error = SemanticError.PreconditionFailed; return true;
				case MissingRequiredValueException _: error = SemanticError.MissingRequiredValue; return true;
				case ArgumentException _: error = SemanticError.BadArgument; return true;
				case NotSupportedException _: error = SemanticError.NotSupported; return true;
				case TimeoutException _: error = SemanticError.Timeout; return true;
				case OperationCanceledException _: error = SemanticError.Canceled; return true;
				default: error = default; return false;
			}
		}

		public static SemanticError? Resolve(this Exception ex, params IEnumerable<ISemanticExceptionConverter> converters) {
			if (ex.TryGetSemanticError(out var error))          // already in the vocabulary?
				return error;
			foreach (var converter in converters) {             // otherwise try to normalize it
				if (converter.TryConvert(ex, out var converted) && converted.TryGetSemanticError(out error))
					return error;
			}
			return null;                                         // unknown — let it bubble as a 500
		}
	}
}
