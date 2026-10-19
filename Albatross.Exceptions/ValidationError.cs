namespace Albatross.Exceptions {
	public record class ValidationError {
		public ValidationError(string field, string error) {
			this.Field = field; this.Error = error;
		}
		public string Field { get; }
		public string Error { get; }
	}
}
