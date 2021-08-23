using dTerm.Core;

namespace dTerm.UI.Wpf.Views
{
    public class ShellProcessTerminalViewModel : BaseViewModel
    {
        private readonly ShellProcess _processEntity;

        public ShellProcessTerminalViewModel(ShellProcess processEntity = null)
        {
            _processEntity = processEntity;
        }

        public string ProcessExecutablePath => _processEntity?.ProcessExecutablePath;
    }
}
