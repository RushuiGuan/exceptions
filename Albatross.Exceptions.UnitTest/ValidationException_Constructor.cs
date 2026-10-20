using Xunit;

namespace Albatross.Exceptions.UnitTest {
	public class ValidationException_Constructor {
		[Theory]
		[InlineData("Cen code is missing", "Cen code is missing")]
		[InlineData("", "Validation failed")]
		[InlineData(null, "Validation failed")]
		public void MessageFromString(string? message, string expected) {
			var exception = new ValidationException(message);
			Assert.Equal(expected, exception.Message);
		}
	}
}
