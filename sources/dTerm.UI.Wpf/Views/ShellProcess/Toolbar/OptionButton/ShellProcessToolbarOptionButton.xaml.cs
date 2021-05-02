using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System;
using System.Reactive.Disposables;

namespace dTerm.UI.Wpf.Views
{
    public abstract class ShellProcessToolbarOptionButtonBase : BaseUserControl<ShellProcessToolbarOptionButtonViewModel> { }

    public partial class ShellProcessToolbarOptionButton : ShellProcessToolbarOptionButtonBase
    {
        public ShellProcessToolbarOptionButton()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                disposables.Add(ViewModel.ShowEditDialog.RegisterHandler(async interaction =>
                {
                    var view = interaction.Input.GetView();
                    var result = await DialogHost.Show(view, DialogNames.Main);
                    interaction.SetOutput(Convert.ToBoolean(result));
                }));

                // Icon
                this.Bind(
                    ViewModel,
                    vm => vm.Icon,
                    v => v.launchButtonIcon.Kind,
                    value => Enum.Parse<PackIconKind>(value, ignoreCase: true),
                    value => value.ToString()
                ).DisposeWith(disposables);

                // Tooltip
                this.Bind(
                    ViewModel,
                    vm => vm.Name,
                    v => v.launchButton.ToolTip
                ).DisposeWith(disposables);

                // Launch Terminal
                this.BindCommand(
                    ViewModel,
                    vm => vm.Launch,
                    v => v.launchButton,
                    withParameter: p => p.TerminalWindowViewModel
                ).DisposeWith(disposables);

                // Edit
                this.BindCommand(
                    ViewModel,
                    vm => vm.Edit,
                    v => v.edit
                ).DisposeWith(disposables);

                // Change Icon
                this.BindCommand(
                    ViewModel,
                    vm => vm.ChangeIcon,
                    v => v.changeIcon
                ).DisposeWith(disposables);

                // Delete
                this.BindCommand(
                    ViewModel,
                    vm => vm.Delete,
                    v => v.deleteButton
                ).DisposeWith(disposables);
            });
        }
    }
}
