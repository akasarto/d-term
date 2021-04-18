using MaterialDesignThemes.Wpf;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessEditorViewModel : BaseReactiveObject
    {
        private readonly ShellProcessEditorViewModelValidator _validator = new();

        public ShellProcessEditorViewModel()
        {
            Cancel = ReactiveCommand.Create(CancelImpl);
            Save = ReactiveCommand.Create(SaveImpl);
        }

        public ReactiveCommand<Unit, Unit> Cancel { get; }
        public ReactiveCommand<Unit, Unit> Save { get; }

        [Reactive] public string Name { get; set; }
        [Reactive] public string Args { get; set; }

        private void CancelImpl() => DialogHost.Close("shellProcessesPanel");

        private void SaveImpl()
        {
            var result = _validator.Validate(this);

            if (!result.IsValid)
            {
                NotifyErrors(result.Errors);

                return;
            }

            CancelImpl();
        }
    }
}
