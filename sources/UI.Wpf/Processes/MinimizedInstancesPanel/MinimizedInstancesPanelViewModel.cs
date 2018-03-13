using ReactiveUI;
using Splat;

namespace UI.Wpf.Processes
{
	//
	public interface IMinimizedInstancesPanelViewModel
	{
		IReactiveDerivedList<IInstanceViewModel> Instances { get; }
	}

	//
	public class MinimizedInstancesPanelViewModel : IMinimizedInstancesPanelViewModel
	{
		private readonly IAppState _appState;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public MinimizedInstancesPanelViewModel(IAppState appState = null)
		{
			_appState = appState ?? Locator.CurrentMutable.GetService<IAppState>();
		}

		public IReactiveDerivedList<IInstanceViewModel> Instances => _appState.GetAll();
	}
}
