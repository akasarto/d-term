using dTerm.Core;
using dTerm.UI.Wpf.Views;
using ReactiveUI;
using System;
using System.Reactive;
using System.Windows;

namespace dTerm.UI.Wpf.UserControls
{
    public class ShellProcessStartButtonViewModel : ReactiveObject
    {
        private readonly ProcessEntity _processEntity;

        public ShellProcessStartButtonViewModel(ProcessEntity processEntity)
        {
            Launch = ReactiveCommand.Create<TerminalWindowViewModel>(windowViewModel =>
            {
                var window = windowViewModel.GetWindow();

                window.Owner = Application.Current.MainWindow;
                window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                window.ShowInTaskbar = false;

                window.Show();
            });

            _processEntity = processEntity;
        }

        public Guid Id => _processEntity.Id;
        public string Icon => _processEntity.Icon;
        public string Name => _processEntity.Name;
        public TerminalWindowViewModel TerminalWindowViewModel => new(_processEntity);

        public ReactiveCommand<TerminalWindowViewModel, Unit> Launch { get; }
    }
}
