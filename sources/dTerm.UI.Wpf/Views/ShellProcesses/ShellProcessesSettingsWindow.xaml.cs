namespace dTerm.UI.Wpf.Views
{
    public abstract class ShellProcessesSettingsWindowBase : BaseWindow<ShellProcessesSettingsWindowViewModel> { }

    public partial class ShellProcessesSettingsWindow : ShellProcessesSettingsWindowBase
    {
        public ShellProcessesSettingsWindow()
        {
            InitializeComponent();
        }
    }
}
