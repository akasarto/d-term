using dTerm.Core;
using ReactiveUI;

namespace dTerm.UI.Wpf.Views
{
    public class TerminalWindowViewModel : ReactiveObject
    {
        private readonly ProcessEntity _processEntity;

        public TerminalWindowViewModel(ProcessEntity processEntity)
        {
            _processEntity = processEntity;
        }

        public string ProcessExecutablePath => _processEntity.ProcessExecutablePath;
    }
}
