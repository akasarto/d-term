using ReactiveUI;
using System.Reactive.Disposables;

namespace dTerm.UI.Wpf.Views
{
    public abstract class ShellProcessEditorBase : BaseUserControl<ShellProcessEditorViewModel>
    {
    }

    public partial class ShellProcessEditor : ShellProcessEditorBase
    {
        public ShellProcessEditor()
        {
            InitializeComponent();

            ViewModel = new ShellProcessEditorViewModel();

            this.WhenActivated(bindings =>
            {
                // Cancel button
                this.BindCommand(
                    ViewModel,
                    vm => vm.Cancel,
                    v => v.cancelButton
                ).DisposeWith(bindings);
            });
        }
    }
}
