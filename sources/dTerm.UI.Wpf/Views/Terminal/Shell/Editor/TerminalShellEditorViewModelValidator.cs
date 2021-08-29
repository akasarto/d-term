using dTerm.Domain;
using FluentValidation;

namespace dTerm.UI.Wpf.Views
{
    public class TerminalShellEditorViewModelValidator : AbstractValidator<TerminalShellEditorViewModel>
    {
        public TerminalShellEditorViewModelValidator()
        {
            // Name
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Name).MaximumLength(ShellProcess.NameMaxLength);

            // Args
            RuleFor(model => model.ProcessStartupArgs).MaximumLength(ShellProcess.ProcessStartupArgsMaxLength);
        }
    }
}
