using Splat;
using UI.Wpf.Processes;

namespace UI.Wpf.Settings
{
	/// <summary>
	/// General settings view model interface.
	/// </summary>
	public interface ISettingsViewModel
	{
		IProcessesManagerViewModel ProcessesManagerViewModel { get; }
	}

	/// <summary>
	/// App general settings view model implementation.
	/// </summary>
	public class SettingsViewModel : ISettingsViewModel
	{
		//
		private readonly IProcessesManagerViewModel _processesManagerViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public SettingsViewModel(IProcessesManagerViewModel processesManagerViewModel = null)
		{
			_processesManagerViewModel = processesManagerViewModel ?? Locator.CurrentMutable.GetService<IProcessesManagerViewModel>();
		}

		/// <summary>
		/// Gets the processes manager view model instance.
		/// </summary>
		public IProcessesManagerViewModel ProcessesManagerViewModel => _processesManagerViewModel;
	}
}
