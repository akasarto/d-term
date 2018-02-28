using FluentValidation;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Validation rules for process view model instances.
	/// </summary>
	public class ProcessValidator : AbstractValidator<IProcessViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessValidator()
		{
			//
			RuleFor(model => model.PicturePath).Must(BeValidGeometryPath);

			//
			RuleFor(model => model.Name).NotEmpty();

			//
			RuleFor(model => model.ProcessBasePath).NotNull();

			//
			RuleFor(model => model.ProcessExecutableName).NotEmpty();
		}

		/// <summary>
		/// Validates the provided icon data.
		/// </summary>
		private bool BeValidGeometryPath(string input)
		{
			var pathData = input?.ToString() ?? string.Empty;

			if (string.IsNullOrWhiteSpace(pathData))
			{
				return true;
			}

			var result = new IsValidGeometryConverter().Convert(
				pathData,
				null,
				null,
				System.Threading.Thread.CurrentThread.CurrentCulture);

			return (bool)result;
		}
	}
}
