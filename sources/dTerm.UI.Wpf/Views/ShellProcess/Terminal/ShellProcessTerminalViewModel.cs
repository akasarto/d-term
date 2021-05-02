using dTerm.Core;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessTerminalViewModel : BaseViewModel
    {
        private readonly ProcessEntity _processEntity;

        public ShellProcessTerminalViewModel(ProcessEntity processEntity = null)
        {
            _processEntity = processEntity;
        }

        public string ProcessExecutablePath => _processEntity?.ProcessExecutablePath;
    }
}
