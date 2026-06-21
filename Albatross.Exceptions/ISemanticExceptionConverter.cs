using System;
using System.Diagnostics.CodeAnalysis;

namespace Albatross.Exceptions {
	public interface ISemanticExceptionConverter {
		bool TryConvert(Exception source, [NotNullWhen(true)] out Exception? result);
	}
}