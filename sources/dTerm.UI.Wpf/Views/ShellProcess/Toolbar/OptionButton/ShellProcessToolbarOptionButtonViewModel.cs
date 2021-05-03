using dTerm.Core;
using dTerm.Core.Reposistories;
using MaterialDesignThemes.Wpf;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessToolbarOptionButtonViewModel : BaseViewModel
    {
        private readonly IShellProcessesRepository _shellProcessesRepository;

        public ShellProcessToolbarOptionButtonViewModel(IShellProcessesRepository shellProcessesRepository = null, ProcessEntity shellProcess = null)
        {
            _shellProcessesRepository = shellProcessesRepository ?? Locator.Current.GetService<IShellProcessesRepository>();

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
                var shellProcess = await _shellProcessesRepository.ReadByIdAsync(Id);
                var shellProcessEditorViewModel = new ShellProcessEditorViewModel(shellProcess);

                _ = ShowShellProcessEditorDialog.Handle(shellProcessEditorViewModel).ObserveOn(RxApp.MainThreadScheduler).Subscribe(async edited =>
                {
                    if (edited)
                    {
                        shellProcess.Name = shellProcessEditorViewModel.Name;
                        shellProcess.ProcessStartupArgs = shellProcessEditorViewModel.ExeArgs;

                        shellProcess = await _shellProcessesRepository.UpdateAsync(shellProcess);

                        RefreshData(shellProcess);
                    }
                });
            });

            //
            ConfirmDeletionDialog = new Interaction<string, bool>();
            Delete = ReactiveCommand.Create(() =>
            {
                _ = ConfirmDeletionDialog.Handle(Name).ObserveOn(RxApp.MainThreadScheduler).Subscribe(async deletionConfirmed =>
                {
                    if (deletionConfirmed)
                    {
                        await _shellProcessesRepository.DeleteAsync(Id);
                    }
                });
            });

            // Change Icon
            ShowIconBrowserDialog = new Interaction<IconBrowserViewModel, bool>();
            ChangeIcon = ReactiveCommand.Create(() =>
            {
                var iconBrowserViewModel = new IconBrowserViewModel();

                _ = ShowIconBrowserDialog.Handle(iconBrowserViewModel).ObserveOn(RxApp.MainThreadScheduler).Subscribe(async iconSelected =>
                {
                    if (iconSelected)
                    {
                        var shellProcess = await _shellProcessesRepository.ReadByIdAsync(Id);

                        shellProcess.Icon = iconBrowserViewModel.SelectedIcon.Kind.ToString();

                        shellProcess = await _shellProcessesRepository.UpdateAsync(shellProcess);

                        RefreshData(shellProcess);
                    }
                });
            });

            RefreshData(shellProcess);
        }

        private void RefreshData(ProcessEntity shellProcess)
        {
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
