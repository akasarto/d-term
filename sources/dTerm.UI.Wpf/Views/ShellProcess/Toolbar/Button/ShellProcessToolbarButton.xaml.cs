using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace dTerm.UI.Wpf.Views
{
    public abstract class ShellProcessToolbarButtonBase : BaseUserControl<ShellProcessToolbarButtonViewModel> { }

    public partial class ShellProcessToolbarButton : ShellProcessToolbarButtonBase
    {
        public ShellProcessToolbarButton()
        {
            InitializeComponent();

            this.WhenActivated(bindings =>
            {
                // Icon
                this.OneWayBind(
                    ViewModel,
                    vm => vm.Icon,
                    v => v.launchButtonIcon.Kind,
                    value => Enum.Parse<PackIconKind>(value, ignoreCase: true)
                ).DisposeWith(bindings);

                // Tooltip
                this.OneWayBind(
                    ViewModel,
                    vm => vm.Name,
                    v => v.ToolTip
                ).DisposeWith(bindings);

                // Launch Terminal
                this.BindCommand(
                    ViewModel,
                    vm => vm.Launch,
                    v => v.launchButton,
                    withParameter: p => p.TerminalWindowViewModel
                ).DisposeWith(bindings);

                // Edit
                this.BindCommand(
                    ViewModel,
                    vm => vm.Edit,
                    v => v.edit
                ).DisposeWith(bindings);

                // Change Icon
                this.BindCommand(
                    ViewModel,
                    vm => vm.ChangeIcon,
                    v => v.changeIcon
                ).DisposeWith(bindings);

                // Delete
                this.BindCommand(
                    ViewModel,
                    vm => vm.Delete,
                    v => v.deleteButton
                ).DisposeWith(bindings);
            });
        }
    }
}
