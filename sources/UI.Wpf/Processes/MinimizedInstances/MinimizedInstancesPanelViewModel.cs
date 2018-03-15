using ReactiveUI;
using Splat;
using System.Reactive;
using WinApi.User32;

namespace UI.Wpf.Processes
{
	//
	public interface IMinimizedInstancesPanelViewModel
	{
		IReactiveDerivedList<IProcessInstanceViewModel> MinimizedInstances { get; }
		ReactiveCommand<IProcessInstanceViewModel, Unit> RestoreInstanceWindowCommand { get; }
	}

	//
	public class MinimizedInstancesPanelViewModel : IMinimizedInstancesPanelViewModel
	{
		private readonly IAppState _appState;
		private readonly IReactiveDerivedList<IProcessInstanceViewModel> _minimizedIntegratedInstances;
		private readonly ReactiveCommand<IProcessInstanceViewModel, Unit> _restoreInstanceWindowCommand;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public MinimizedInstancesPanelViewModel(IAppState appState = null)
		{
			_appState = appState ?? Locator.CurrentMutable.GetService<IAppState>();

			_minimizedIntegratedInstances = _appState.GetProcessInstances().CreateDerivedCollection(
				filter: instance => !instance.IsElevated && instance.IsMinimized,
				selector: instance => instance
			);

			// Restore window
			_restoreInstanceWindowCommand = ReactiveCommand.Create<IProcessInstanceViewModel>(instance => RestoreInstanceWindowCommandAction(instance));
		}

		public IReactiveDerivedList<IProcessInstanceViewModel> MinimizedInstances => _minimizedIntegratedInstances;

		public ReactiveCommand<IProcessInstanceViewModel, Unit> RestoreInstanceWindowCommand => _restoreInstanceWindowCommand;

		private void RestoreInstanceWindowCommandAction(IProcessInstanceViewModel instance)
		{
			instance.IsMinimized = false;
			User32Methods.ShowWindow(instance.ProcessMainWindowHandle, ShowWindowCommands.SW_SHOW);
			User32Methods.ShowWindow(instance.ProcessMainWindowHandle, ShowWindowCommands.SW_RESTORE);
		}
	}
}
