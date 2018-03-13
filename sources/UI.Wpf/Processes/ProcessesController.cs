using Splat;

namespace UI.Wpf.Processes
{
	//
	public interface IProcessesController
	{
		IConsolesPanelViewModel ConsolesPanel { get; }
		IMinimizedInstancesPanelViewModel MinimizedInstancesPanel { get; }
		ITransparencyManagerPanelViewModel TransparencyManagerPanel { get; }
	}

	//
	public class ProcessesController : IProcessesController
	{
		private readonly IConsolesPanelViewModel _consolesPanelViewModel;
		private readonly IMinimizedInstancesPanelViewModel _minimizedInstancesPanel;
		private readonly ITransparencyManagerPanelViewModel _transparencyManagerPanelViewModel;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesController(IConsolesPanelViewModel consolesPanelViewModel = null, IMinimizedInstancesPanelViewModel minimizedInstancesPanel = null, ITransparencyManagerPanelViewModel transparencyManagerPanelViewModel = null)
		{
			_consolesPanelViewModel = consolesPanelViewModel ?? Locator.CurrentMutable.GetService<IConsolesPanelViewModel>();
			_minimizedInstancesPanel = minimizedInstancesPanel ?? Locator.CurrentMutable.GetService<IMinimizedInstancesPanelViewModel>();
			_transparencyManagerPanelViewModel = transparencyManagerPanelViewModel ?? Locator.CurrentMutable.GetService<ITransparencyManagerPanelViewModel>();
		}

		public IConsolesPanelViewModel ConsolesPanel => _consolesPanelViewModel;

		public IMinimizedInstancesPanelViewModel MinimizedInstancesPanel => _minimizedInstancesPanel;

		public ITransparencyManagerPanelViewModel TransparencyManagerPanel => _transparencyManagerPanelViewModel;
	}
}
