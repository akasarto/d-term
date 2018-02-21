using FluentValidation;

namespace UI.Wpf.Notebook
{
	public class NoteFormViewModelValidator : AbstractValidator<NoteFormViewModel>
	{
		public NoteFormViewModelValidator()
		{
			RuleFor(model => model.Title).NotEmpty();
		}
	}
}
