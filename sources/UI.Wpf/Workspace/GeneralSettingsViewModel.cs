using System;
using UI.Wpf.Consoles;

namespace UI.Wpf.Workspace
{
	/// <summary>
	/// General settings view model interface.
	/// </summary>
	public interface IGeneralSettingsViewModel
	{
	}

	/// <summary>
	/// App general settings view model implementation.
	/// </summary>
	public class GeneralSettingsViewModel : IGeneralSettingsViewModel
	{
		//
		private readonly IConsoleSettingsViewModel _consoleSettingsViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public GeneralSettingsViewModel(IConsoleSettingsViewModel consoleSettingsViewModel)
		{
			_consoleSettingsViewModel = consoleSettingsViewModel ?? throw new ArgumentNullException(nameof(consoleSettingsViewModel), nameof(GeneralSettingsViewModel));
		}
	}
}
