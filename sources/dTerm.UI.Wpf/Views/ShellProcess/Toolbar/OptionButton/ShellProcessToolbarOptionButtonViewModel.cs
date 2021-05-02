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
        private readonly Interaction<ShellProcessEditorViewModel, bool> _showEditDialog;

        public ShellProcessToolbarOptionButtonViewModel(IShellProcessesRepository shellProcessesRepository = null, ProcessEntity shellProcess = null)
        {
            _shellProcessesRepository = shellProcessesRepository ?? Locator.Current.GetService<IShellProcessesRepository>();

            //
            _showEditDialog = new Interaction<ShellProcessEditorViewModel, bool>();

            //
            Launch = ReactiveCommand.Create<ShellProcessTerminalViewModel>(windowViewModel =>
            {
                //var window = windowViewModel.GetWindow();

                //window.Owner = Application.Current.MainWindow;
                //window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                //window.ShowInTaskbar = false;

                //window.Show();
            });

            Edit = ReactiveCommand.CreateFromTask(EditImpl);
            Delete = ReactiveCommand.CreateFromTask(DeleteImpl);
            ChangeIcon = ReactiveCommand.CreateFromTask(ChangeIconImpl);

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
        public Interaction<ShellProcessEditorViewModel, bool> ShowEditDialog => _showEditDialog;

        private async Task<Unit> EditImpl()
        {
            var shellProcess = await _shellProcessesRepository.ReadByIdAsync(Id);
            var shellProcessEditorViewModel = new ShellProcessEditorViewModel(shellProcess);

            _ = _showEditDialog.Handle(shellProcessEditorViewModel).ObserveOn(RxApp.MainThreadScheduler).Subscribe(async edited =>
            {
                if (edited)
                {
                    shellProcess.Name = shellProcessEditorViewModel.Name;
                    shellProcess.ProcessStartupArgs = shellProcessEditorViewModel.Args;

                    shellProcess = await _shellProcessesRepository.UpdateAsync(shellProcess);

                    RefreshData(shellProcess);
                }
            });

            return Unit.Default;
        }

        private Task<Unit> DeleteImpl()
        {
            return Task.FromResult(Unit.Default);
        }

        private async Task<Unit> ChangeIconImpl()
        {
            var iconEditor = new IconBrowser();

            _ = await DialogHost.Show(iconEditor, DialogNames.Main);

            return Unit.Default;
        }
    }
}
