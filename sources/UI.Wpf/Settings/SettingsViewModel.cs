using Splat;
using UI.Wpf.Processes;

namespace UI.Wpf.Settings
{
	//
	public interface ISettingsViewModel
	{
		IConsolesManagerViewModel ConsolesManagerViewModel { get; }
	}

	//
	public class SettingsViewModel : ISettingsViewModel
	{
		private readonly IConsolesManagerViewModel _consolesManagerViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public SettingsViewModel(IConsolesManagerViewModel consolesManagerViewModel = null)
		{
			_consolesManagerViewModel = consolesManagerViewModel ?? Locator.CurrentMutable.GetService<IConsolesManagerViewModel>();
		}

		public IConsolesManagerViewModel ConsolesManagerViewModel => _consolesManagerViewModel;
	}
}
