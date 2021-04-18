using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System.Reactive;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessEditorViewModel : ReactiveObject
    {
        public ShellProcessEditorViewModel()
        {
        }

        public ReactiveCommand<Unit, Unit> Cancel { get; }

        private void CancelImpl() => DialogHost.Close("shellProcessesPanel");
    }
}
