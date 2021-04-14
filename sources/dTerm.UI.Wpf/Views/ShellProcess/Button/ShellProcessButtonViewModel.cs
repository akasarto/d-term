using dTerm.Core;
using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessButtonViewModel : ReactiveObject
    {
        private readonly ProcessEntity _processEntity;

        public ShellProcessButtonViewModel(ProcessEntity processEntity)
        {
            Launch = ReactiveCommand.Create<ShellProcessTerminalWindowViewModel>(windowViewModel =>
            {
                var window = windowViewModel.GetWindow();

                window.Owner = Application.Current.MainWindow;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.ShowInTaskbar = false;

                window.Show();
            });

            Edit = ReactiveCommand.CreateFromTask(EditImpl);
            Delete = ReactiveCommand.CreateFromTask(DeleteImpl);
            ChangeIcon = ReactiveCommand.CreateFromTask(ChangeIconImpl);

            _processEntity = processEntity;
        }

        private async Task<Unit> EditImpl()
        {
            var x = new ShellProcessEditor();

            _ = await DialogHost.Show(x, "shellProcessesPanel");

            return Unit.Default;
        }

        private Task<Unit> DeleteImpl()
        {
            return Task.FromResult(Unit.Default);
        }

        private Task<Unit> ChangeIconImpl()
        {
            return Task.FromResult(Unit.Default);
        }

        public Guid Id => _processEntity.Id;
        public string Icon => _processEntity.Icon;
        public string Name => _processEntity.Name;
        public ShellProcessTerminalWindowViewModel TerminalWindowViewModel => new(_processEntity);

        public ReactiveCommand<Unit, Unit> Edit { get; }
        public ReactiveCommand<Unit, Unit> Delete { get; }
        public ReactiveCommand<Unit, Unit> ChangeIcon { get; }
        public ReactiveCommand<ShellProcessTerminalWindowViewModel, Unit> Launch { get; }
    }
}
