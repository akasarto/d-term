using dTerm.Core;
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

        public ShellProcessEditorViewModel(ProcessEntity shellProcess = null)
        {
            Cancel = ReactiveCommand.Create(CancelImpl);
            Save = ReactiveCommand.Create(SaveImpl);

            LoadData(shellProcess);
        }

        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public ReactiveCommand<Unit, Unit> Save { get; }

        public string Icon { get; set; }
        public string ProcessExecutablePath { get; set; }
        [Reactive] public string Name { get; set; }
        [Reactive] public string Args { get; set; }

        private void LoadData(ProcessEntity shellProcess)
        {
            Icon = shellProcess?.Icon;
            ProcessExecutablePath = shellProcess?.ProcessExecutablePath;
            Name = shellProcess?.Name;
            Args = shellProcess?.ProcessStartupArgs;
        }

        private void CancelImpl() => DialogHost.Close(DialogNames.Main, false);

        private void SaveImpl()
        {
            var validation = _validator.Validate(this);

            if (!validation.IsValid)
            {
                NotifyErrors(validation.Errors);

                return;
            }

            DialogHost.Close(DialogNames.Main, true);
        }
    }
}
