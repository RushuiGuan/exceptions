namespace Albatross.Exceptions {
	/// <summary>One validation failure, optionally tied to a field. Carried by <see cref="FieldValidationException"/>.</summary>
	public record class ValidationError {
		/// <param name="field">Path of the offending field, such as <c>legs[0].cenCode</c>.</param>
		/// <param name="error">Explains what is wrong with the field's value.</param>
		public ValidationError(string field, string error) {
			this.Field = field; 
			this.Error = error;
		}

		/// <summary>Creates an error about the object as a whole rather than a single field.</summary>
		public ValidationError(string error) {
			this.Field = string.Empty;
			this.Error = error;
		}
		public string Field { get; }
		public string Error { get; }

		/// <summary>Formats as <c>[field] error</c>, or just the error when there is no field.</summary>
		public override string ToString() {
			if (string.IsNullOrEmpty(Field)) {
				return Error;
			} else {
				return $"[{Field}] {Error}";
			}
		}
	}
}
