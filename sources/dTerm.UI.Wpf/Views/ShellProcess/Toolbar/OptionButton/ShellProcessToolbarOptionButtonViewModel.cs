using dTerm.Core;
using dTerm.UI.Wpf.Services;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessToolbarOptionButtonViewModel : BaseViewModel
    {
        private readonly ShellProcessData _shellProcessData;

        public ShellProcessToolbarOptionButtonViewModel(ShellProcessData shellProcessData = null, ProcessEntity shellProcess = null)
        {
            _shellProcessData = shellProcessData ?? Locator.Current.GetService<ShellProcessData>();

            //
            Launch = ReactiveCommand.Create<ShellProcessTerminalViewModel>(windowViewModel =>
            {
                var window = windowViewModel.GetWindow();

                window.Owner = Application.Current.MainWindow;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.ShowInTaskbar = false;

                window.Show();
            });
            Launch.ThrownExceptions.Subscribe(ex => throw ex);

            // Edit
            ShowShellProcessEditorDialog = new Interaction<ShellProcessEditorViewModel, bool>();
            Edit = ReactiveCommand.CreateFromTask(async () =>
            {
                var shellProcessEditorViewModel = await _shellProcessData.GetByIdAsync(Id);

                _ = ShowShellProcessEditorDialog.Handle(shellProcessEditorViewModel).Where(trueResult => trueResult).ObserveOn(RxApp.MainThreadScheduler).Subscribe(async _ =>
                {
                    await _shellProcessData.UpdateBasicInfoAsync(shellProcessEditorViewModel);

                    Name = shellProcessEditorViewModel.Name;
                });
            });

            //
            ConfirmDeletionDialog = new Interaction<string, bool>();
            Delete = ReactiveCommand.Create(() =>
            {
                _ = ConfirmDeletionDialog.Handle(Name).Where(trueResult => trueResult).ObserveOn(RxApp.MainThreadScheduler).Subscribe(async _ =>
                {
                    await _shellProcessData.DeleteAsync(Id);
                });
            });

            // Change Icon
            ShowIconBrowserDialog = new Interaction<IconBrowserViewModel, bool>();
            ChangeIcon = ReactiveCommand.Create(() =>
            {
                var iconBrowserViewModel = new IconBrowserViewModel();

                _ = ShowIconBrowserDialog.Handle(iconBrowserViewModel).Where(trueResult => trueResult).ObserveOn(RxApp.MainThreadScheduler).Subscribe(async _ =>
                {
                    await _shellProcessData.UpdateIconAsync(Id, iconBrowserViewModel);

                    Icon = iconBrowserViewModel.SelectedIcon.Kind.ToString();
                });
            });

            // Initial values
            Id = shellProcess?.Id ?? Guid.Empty;
            Icon = shellProcess?.Icon;
            Name = shellProcess?.Name;
        }

        [Reactive] public Guid Id { get; set; }
        [Reactive] public string Icon { get; set; }
        [Reactive] public string Name { get; set; }

        public ShellProcessTerminalViewModel TerminalWindowViewModel => new();

        public ReactiveCommand<Unit, Unit> Edit { get; }
        public ReactiveCommand<Unit, Unit> Delete { get; }
        public ReactiveCommand<Unit, Unit> ChangeIcon { get; }
        public ReactiveCommand<ShellProcessTerminalViewModel, Unit> Launch { get; }
        public Interaction<IconBrowserViewModel, bool> ShowIconBrowserDialog { get; }
        public Interaction<ShellProcessEditorViewModel, bool> ShowShellProcessEditorDialog { get; }
        public Interaction<string, bool> ConfirmDeletionDialog { get; }
    }
}
