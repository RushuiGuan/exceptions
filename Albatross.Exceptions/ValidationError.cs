namespace Albatross.Exceptions {
	public record class ValidationError {
		public ValidationError(string field, string error) {
			this.Field = field; 
			this.Error = error;
		}

		public ValidationError(string error) {
			this.Field = string.Empty;
			this.Error = error;
		}
		public string Field { get; }
		public string Error { get; }

		public override string ToString() {
			if (string.IsNullOrEmpty(Field)) {
				return Error;
			} else {
				return $"[{Field}] {Error}";
			}
		}
	}
}
