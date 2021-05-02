using dTerm.Core;
using FluentValidation;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessEditorViewModelValidator : AbstractValidator<ShellProcessEditorViewModel>
    {
        public ShellProcessEditorViewModelValidator()
        {
            // Name
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Name).MaximumLength(ProcessEntity.NameMaxLength);

            // Args
            RuleFor(model => model.ExeArgs).MaximumLength(ProcessEntity.ProcessStartupArgsMaxLength);
        }
    }
}
