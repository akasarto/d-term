using FluentValidation;

namespace UI.Wpf.Consoles
{
	public class ConsoleOptionViewModelValidator : AbstractValidator<IConsoleOptionViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleOptionViewModelValidator()
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
