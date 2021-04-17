using ReactiveUI;
using System.Reactive.Disposables;

namespace dTerm.UI.Wpf.Views
{
    public abstract class IconBrowserItemBase : BaseUserControl<IconBrowserItemViewModel> { }

    public partial class IconBrowserItem : IconBrowserItemBase
    {
        public IconBrowserItem()
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

                // Name
                this.OneWayBind(
                    ViewModel,
                    vm => vm.Kind,
                    v => v.primaryName.Text,
                    x => x.ToString()
                ).DisposeWith(bindings);

                // Aliases
                this.OneWayBind(
                    ViewModel,
                    vm => vm.Aliases,
                    v => v.aliases.ToolTip,
                    alias => string.Join("\n", alias)
                ).DisposeWith(bindings);
            });
        }
    }
}
