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
                // Edit Interaction
                // ToDo: Find the proper RxUI way of closing the dialog outside of the vm
                disposables.Add(ViewModel.ShowShellProcessEditorDialog.RegisterHandler(async interaction =>
                {
                    var view = interaction.Input.GetView();
                    var result = await DialogHost.Show(view, DialogNames.Main);
                    interaction.SetOutput(Convert.ToBoolean(result));
                }));

                // Icon Browser Interaction
                // ToDo: Find the proper RxUI way of closing the dialog outside of the vm
                disposables.Add(ViewModel.ShowIconBrowserDialog.RegisterHandler(async interaction =>
                {
                    var view = interaction.Input.GetView();
                    var result = await DialogHost.Show(view, DialogNames.Main);
                    interaction.SetOutput(Convert.ToBoolean(result));
                }));

                // Icon
                this.Bind(
                    ViewModel,
                    vm => vm.Icon,
                    v => v.launchButtonIcon.Kind
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
