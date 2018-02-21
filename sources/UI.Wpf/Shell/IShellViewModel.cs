using MaterialDesignThemes.Wpf;
using UI.Wpf.Consoles;

namespace UI.Wpf.Shell
{
	public interface IShellViewModel
	{
		ISnackbarMessageQueue SnackbarMessageQueue { get; }
		IConsoleOptionsPanelViewModel ConsoleOptionsPanelViewModel { get; }
	}
}
