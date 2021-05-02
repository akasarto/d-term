using dTerm.Core;
using MaterialDesignThemes.Wpf;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessEditorViewModel : BaseViewModel
    {
        private readonly ShellProcessEditorViewModelValidator _validator = new();

        public ShellProcessEditorViewModel(ProcessEntity shellProcess = null)
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

            LoadData(shellProcess);
        }

        public string Icon { get; set; }
        public string ExePath { get; set; }
        [Reactive] public string Name { get; set; }
        [Reactive] public string ExeArgs { get; set; }

        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public ReactiveCommand<Unit, Unit> Save { get; }

        private void LoadData(ProcessEntity shellProcess = null)
        {
            Icon = shellProcess?.Icon;
            ExePath = shellProcess?.ProcessExecutablePath;
            Name = shellProcess?.Name;
            ExeArgs = shellProcess?.ProcessStartupArgs;
        }
    }
}
