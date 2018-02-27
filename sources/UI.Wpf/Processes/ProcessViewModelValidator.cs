using FluentValidation;

namespace UI.Wpf.Processes
{
	/// <summary>
	/// Validation rules for process view model instances.
	/// </summary>
	public class ProcessViewModelValidator : AbstractValidator<IProcessViewModel>
	{
		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessViewModelValidator()
		{
		}
	}
}
