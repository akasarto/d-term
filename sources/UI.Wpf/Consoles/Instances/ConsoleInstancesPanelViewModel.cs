using ReactiveUI;
using Splat;

namespace UI.Wpf.Consoles
{
	//
	public interface IConsoleInstancesPanelViewModel
	{
		IReactiveDerivedList<IConsoleInstanceViewModel> Instances { get; }
	}

	//
	public class ConsoleInstancesPanelViewModel : IConsoleInstancesPanelViewModel
	{
		private readonly IAppState _appState;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ConsoleInstancesPanelViewModel(IAppState appState = null)
		{
			_appState = appState ?? Locator.CurrentMutable.GetService<IAppState>();
		}

		public IReactiveDerivedList<IConsoleInstanceViewModel> Instances => _appState.GetConsoleInstances();
	}
}
