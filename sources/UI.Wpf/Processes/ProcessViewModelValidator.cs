using FluentValidation;

namespace UI.Wpf.Processes
{
	//
	public class ProcessViewModelValidator : AbstractValidator<IProcessViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessViewModelValidator()
		{
			//
			RuleFor(model => model.PicturePath).Must(BeValidGeometryPath);

			//
			RuleFor(model => model.Type).NotEmpty();

			//
			RuleFor(model => model.Name).NotEmpty();

			//
			RuleFor(model => model.ProcessBasePath).NotEmpty();

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
