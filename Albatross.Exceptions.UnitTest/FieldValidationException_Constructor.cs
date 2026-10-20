using Xunit;

namespace Albatross.Exceptions.UnitTest {
	public class FieldValidationException_Constructor {
		public static TheoryData<ValidationError[], string> Messages => new() {
			{ [], "Validation failed" },
			{ [new("cenCode", "Cen code 'ABCDE' is invalid")], "[cenCode] Cen code 'ABCDE' is invalid" },
			{
				[new("cenCode", "Cen code 'ABCDE' is invalid"), new("marketId", "Market id 0 is invalid")],
				"[cenCode] Cen code 'ABCDE' is invalid; [marketId] Market id 0 is invalid"
			},
			// an error about the object as a whole has no field to bracket
			{ [new("", "Base and quote currency are the same")], "Base and quote currency are the same" },
			{
				[new("", "Base and quote currency are the same"), new("baseCurrency", "Base currency 'EU' is invalid")],
				"Base and quote currency are the same; [baseCurrency] Base currency 'EU' is invalid"
			},
			// the brackets of an indexed path must not be confused with the ones around the field
			{ [new("legs[0].cenCode", "Cen code is missing")], "[legs[0].cenCode] Cen code is missing" },
		};

		[Theory]
		[MemberData(nameof(Messages))]
		public void MessageFromErrors(ValidationError[] errors, string expected) {
			var exception = new FieldValidationException(errors);
			Assert.Equal(expected, exception.Message);
		}

		[Fact]
		public void Errors_KeptInOrder() {
			ValidationError[] errors = [new("marketId", "Market id 0 is invalid"), new("cenCode", "Cen code is missing")];
			var exception = new FieldValidationException(errors);
			Assert.Equal(errors, exception.Errors);
		}

		[Fact]
		public void LazyErrors_EnumeratedOnce() {
			var count = 0;
			IEnumerable<ValidationError> Validate() {
				count++;
				yield return new("marketId", "Market id 0 is invalid");
			}
			var exception = new FieldValidationException(Validate());
			Assert.Single(exception.Errors);
			Assert.Equal("[marketId] Market id 0 is invalid", exception.Message);
			Assert.Equal(1, count);
		}
	}
}
