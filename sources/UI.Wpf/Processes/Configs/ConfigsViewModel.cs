using Splat;

namespace UI.Wpf.Processes
{
	//
	public interface IConfigsViewModel
	{
		IProcessesManagerViewModel ProcessesManager { get; }
	}

	//
	public class ConfigsViewModel : IConfigsViewModel
	{
		private readonly IProcessesManagerViewModel _processesManagerViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConfigsViewModel(IProcessesManagerViewModel processesManagerViewModel = null)
		{
			_processesManagerViewModel = processesManagerViewModel ?? Locator.CurrentMutable.GetService<IProcessesManagerViewModel>();
		}

		public IProcessesManagerViewModel ProcessesManager => _processesManagerViewModel;
	}
}
