using Splat;
using UI.Wpf.Consoles;

namespace UI.Wpf.Settings
{
	/// <summary>
	/// General settings view model interface.
	/// </summary>
	public interface ISettingsViewModel
	{
		IConsoleConfigsViewModel ConsoleConfigsViewModel { get; }
	}

	/// <summary>
	/// App general settings view model implementation.
	/// </summary>
	public class SettingsViewModel : ISettingsViewModel
	{
		//
		private readonly IConsoleConfigsViewModel _consoleConfigsViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public SettingsViewModel(IConsoleConfigsViewModel consoleConfigsViewModel = null)
		{
			_consoleConfigsViewModel = consoleConfigsViewModel ?? Locator.CurrentMutable.GetService<IConsoleConfigsViewModel>();
		}

		/// <summary>
		/// Gets the console settings view model instance.
		/// </summary>
		public IConsoleConfigsViewModel ConsoleConfigsViewModel => _consoleConfigsViewModel;
	}
}
