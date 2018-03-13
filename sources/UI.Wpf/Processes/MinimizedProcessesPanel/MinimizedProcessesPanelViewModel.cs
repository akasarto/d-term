using ReactiveUI;
using Splat;

namespace UI.Wpf.Processes
{
	//
	public interface IMinimizedProcessesPanelViewModel
	{
		IReactiveDerivedList<IProcessInstanceModel> Instances { get; }
	}

	//
	public class MinimizedProcessesPanelViewModel : IMinimizedProcessesPanelViewModel
	{
		private readonly IAppState _appState;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public MinimizedProcessesPanelViewModel(IAppState appState = null)
		{
			_appState = appState ?? Locator.CurrentMutable.GetService<IAppState>();
		}

		public IReactiveDerivedList<IProcessInstanceModel> Instances => _appState.GetConsoleInstances();
	}
}
