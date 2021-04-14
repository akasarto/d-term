using ReactiveUI;
using System.Reactive.Disposables;

namespace dTerm.UI.Wpf.Views
{
    public abstract class IconEditorBase : BaseUserControl<IconEditorViewModel>
    {
    }

    public partial class IconEditor : IconEditorBase
    {
        public IconEditor()
        {
            InitializeComponent();

            this.WhenActivated(bindings =>
            {
                // Icon
                this.OneWayBind(
                    ViewModel,
                    vm => vm.Kind,
                    v => v.icon.Kind
                ).DisposeWith(bindings);

                // Tooltip
                this.OneWayBind(
                    ViewModel,
                    vm => vm.Aliases,
                    v => v.icon.ToolTip,
                    x => string.Join("\n", x)
                ).DisposeWith(bindings);
            });
        }
    }
}
