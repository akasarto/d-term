using UI.Wpf.Consoles;

namespace UI.Wpf.Shell
{
	/// <summary>
	/// Shell view model interface.
	/// </summary>
	public interface IShellViewModel
	{
		/// <summary>
		/// Gets or sets the app title.
		/// </summary>
		string AppTitle { get; set; }

		/// <summary>
		/// Gets the console options panel view model.
		/// </summary>
		IConsoleOptionsPanelViewModel ConsoleOptionsPanelViewModel { get; }
	}
}
