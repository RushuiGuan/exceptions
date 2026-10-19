using Xunit;

namespace Albatross.Exceptions.UnitTest {
	public class ValidationException_Constructor {
		public static TheoryData<ValidationError[], string> Messages => new() {
			{ [], "Validation failed" },
			{ [new("cenCode", "Cen code 'ABCDE' is invalid")], "Validation failed: [cenCode] Cen code 'ABCDE' is invalid" },
			{
				[new("cenCode", "Cen code 'ABCDE' is invalid"), new("marketId", "Market id 0 is invalid")],
				"Validation failed: [cenCode] Cen code 'ABCDE' is invalid; [marketId] Market id 0 is invalid"
			},
			// an error about the object as a whole has no field to bracket
			{ [new("", "Base and quote currency are the same")], "Validation failed: Base and quote currency are the same" },
			{
				[new("", "Base and quote currency are the same"), new("baseCurrency", "Base currency 'EU' is invalid")],
				"Validation failed: Base and quote currency are the same; [baseCurrency] Base currency 'EU' is invalid"
			},
			// the brackets of an indexed path must not be confused with the ones around the field
			{ [new("legs[0].cenCode", "Cen code is missing")], "Validation failed: [legs[0].cenCode] Cen code is missing" },
		};

		[Theory]
		[MemberData(nameof(Messages))]
		public void MessageFromErrors(ValidationError[] errors, string expected) {
			var exception = new ValidationException(errors);
			Assert.Equal(expected, exception.Message);
		}

		[Fact]
		public void Errors_KeptInOrder() {
			ValidationError[] errors = [new("marketId", "Market id 0 is invalid"), new("cenCode", "Cen code is missing")];
			var exception = new ValidationException(errors);
			Assert.Equal(errors, exception.Errors);
		}

		[Fact]
		public void LazyErrors_EnumeratedOnce() {
			var count = 0;
			IEnumerable<ValidationError> Validate() {
				count++;
				yield return new("marketId", "Market id 0 is invalid");
			}
			var exception = new ValidationException(Validate());
			Assert.Single(exception.Errors);
			Assert.Equal("Validation failed: [marketId] Market id 0 is invalid", exception.Message);
			Assert.Equal(1, count);
		}

		[Fact]
		public void MessageOnly_HasNoErrors() {
			var exception = new ValidationException("Cen code is missing");
			Assert.Empty(exception.Errors);
			Assert.Equal("Cen code is missing", exception.Message);
		}
	}
}
