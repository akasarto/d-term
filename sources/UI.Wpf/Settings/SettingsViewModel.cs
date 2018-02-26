using Splat;
using UI.Wpf.Consoles;

namespace UI.Wpf.Settings
{
	/// <summary>
	/// General settings view model interface.
	/// </summary>
	public interface ISettingsViewModel
	{
		IConsolesManagerViewModel ConsolesManager { get; }
	}

	/// <summary>
	/// App general settings view model implementation.
	/// </summary>
	public class SettingsViewModel : ISettingsViewModel
	{
		//
		private readonly IConsolesManagerViewModel _consolesManager;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public SettingsViewModel(IConsolesManagerViewModel consolesManager = null)
		{
			_consolesManager = consolesManager ?? Locator.CurrentMutable.GetService<IConsolesManagerViewModel>();
		}

		/// <summary>
		/// Gets the consoles manager instance.
		/// </summary>
		public IConsolesManagerViewModel ConsolesManager => _consolesManager;
	}
}
