using dTerm.Core;
using ReactiveUI;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessTerminalWindowViewModel : ReactiveObject
    {
        private readonly ProcessEntity _processEntity;

        public ShellProcessTerminalWindowViewModel(ProcessEntity processEntity)
        {
            _processEntity = processEntity;
        }

        public string ProcessExecutablePath => _processEntity.ProcessExecutablePath;
    }
}
