using ReactiveUI;

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
            });
        }
    }
}
