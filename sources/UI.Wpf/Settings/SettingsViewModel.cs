using Splat;
using UI.Wpf.Processes;

namespace UI.Wpf.Settings
{
	/// <summary>
	/// General settings view model interface.
	/// </summary>
	public interface ISettingsViewModel
	{
		IProcessesManagerViewModel ConsolesManager { get; }
	}

	/// <summary>
	/// App general settings view model implementation.
	/// </summary>
	public class SettingsViewModel : ISettingsViewModel
	{
		//
		private readonly IProcessesManagerViewModel _consolesManager;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public SettingsViewModel(IProcessesManagerViewModel consolesManager = null)
		{
			_consolesManager = consolesManager ?? Locator.CurrentMutable.GetService<IProcessesManagerViewModel>();
		}

		/// <summary>
		/// Gets the consoles manager instance.
		/// </summary>
		public IProcessesManagerViewModel ConsolesManager => _consolesManager;
	}
}
