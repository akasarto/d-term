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
                // Icons
                //this.OneWayBind(
                //    ViewModel,
                //    vm => vm.Icons,
                //    v => v.iconsComboBox.ItemsSource
                //).DisposeWith(bindings);
            });
        }
    }
}
