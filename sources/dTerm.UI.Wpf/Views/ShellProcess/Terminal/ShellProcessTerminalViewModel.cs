using dTerm.Core;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessTerminalViewModel : BaseReactiveObject
    {
        private readonly ProcessEntity _processEntity;

        public ShellProcessTerminalViewModel(ProcessEntity processEntity)
        {
            _processEntity = processEntity;
        }

        public string ProcessExecutablePath => _processEntity.ProcessExecutablePath;
    }
}
