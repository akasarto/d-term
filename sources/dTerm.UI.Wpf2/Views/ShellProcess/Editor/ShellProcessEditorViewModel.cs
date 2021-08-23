using MaterialDesignThemes.Wpf;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessEditorViewModel : BaseViewModel
    {
        private readonly ShellProcessEditorViewModelValidator _validator = new();

        public ShellProcessEditorViewModel()
        {
            // Cancel Command
            Cancel = ReactiveCommand.Create(() => DialogHost.Close(DialogNames.Main, false));

            // Save Command
            Save = ReactiveCommand.Create(() =>
            {
                if (_validator.Validate(this) is var validation && validation.IsValid)
                {
                    DialogHost.Close(DialogNames.Main, true);

                    return;
                }

                NotifyErrors(validation.Errors);
            });
        }

        public Guid Id { get; set; }
        public string Icon { get; set; }
        public string ProcessExecutablePath { get; set; }
        [Reactive] public string Name { get; set; }
        [Reactive] public string ProcessStartupArgs { get; set; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public ReactiveCommand<Unit, Unit> Save { get; }
    }
}
