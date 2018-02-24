using System;
using UI.Wpf.Consoles;

namespace UI.Wpf.Settings
{
	/// <summary>
	/// General settings view model interface.
	/// </summary>
	public interface ISettingsViewModel
	{
		IConsoleSettingsViewModel ConsoleSettingsViewModel { get; }
	}

	/// <summary>
	/// App general settings view model implementation.
	/// </summary>
	public class SettingsViewModel : ISettingsViewModel
	{
		//
		private readonly IConsoleSettingsViewModel _consoleSettingsViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public SettingsViewModel(IConsoleSettingsViewModel consoleSettingsViewModel)
		{
			_consoleSettingsViewModel = consoleSettingsViewModel ?? throw new ArgumentNullException(nameof(consoleSettingsViewModel), nameof(SettingsViewModel));
		}

		/// <summary>
		/// Gets the console settings view model instance.
		/// </summary>
		public IConsoleSettingsViewModel ConsoleSettingsViewModel => _consoleSettingsViewModel;
	}
}
