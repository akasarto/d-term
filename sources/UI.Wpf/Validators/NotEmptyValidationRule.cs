using System.Globalization;
using System.Windows.Controls;

namespace UI.Wpf.Validators
{
	public class NotEmptyValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (string.IsNullOrWhiteSpace((value ?? "").ToString()))
			{
				return new ValidationResult(false, "This field is required.");
			}

			return ValidationResult.ValidResult;
		}
	}
}
