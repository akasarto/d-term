using dTerm.Core;
using ReactiveUI;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessTerminalViewModel : ReactiveObject
    {
        private readonly ProcessEntity _processEntity;

        public ShellProcessTerminalViewModel(ProcessEntity processEntity)
        {
            _processEntity = processEntity;
        }

        public string ProcessExecutablePath => _processEntity.ProcessExecutablePath;
    }
}
