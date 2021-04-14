using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace dTerm.UI.Wpf.Views
{
    public abstract class ShellProcessStartButtonBase : BaseUserControl<ShellProcessButtonViewModel> { }

    public partial class ShellProcessButton : ShellProcessStartButtonBase
    {
        public ShellProcessButton()
        {
            InitializeComponent();

            this.WhenActivated(bindings =>
            {
                //
                this.OneWayBind(
                    ViewModel,
                    vm => vm.Icon,
                    v => v.launchButtonIcon.Kind,
                    value => Enum.Parse<PackIconKind>(value, ignoreCase: true)
                ).DisposeWith(bindings);

                //
                this.OneWayBind(
                    ViewModel,
                    vm => vm.Name,
                    v => v.ToolTip
                ).DisposeWith(bindings);

                //
                this.BindCommand(
                    ViewModel,
                    vm => vm.Launch,
                    v => v.launchButton,
                    withParameter: p => p.TerminalWindowViewModel
                ).DisposeWith(bindings);

                //
                this.BindCommand(
                    ViewModel,
                    vm => vm.Delete,
                    v => v.deleteButton
                );
            });
        }
    }
}
