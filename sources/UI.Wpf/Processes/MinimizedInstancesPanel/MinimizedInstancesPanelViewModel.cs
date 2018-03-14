using ReactiveUI;
using Splat;
using System.Reactive;
using WinApi.User32;

namespace UI.Wpf.Processes
{
	//
	public interface IMinimizedInstancesPanelViewModel
	{
		IReactiveDerivedList<IInstanceViewModel> Instances { get; }
		ReactiveCommand<IInstanceViewModel, Unit> RestoreInstanceWindowCommand { get; }
	}

	//
	public class MinimizedInstancesPanelViewModel : IMinimizedInstancesPanelViewModel
	{
		private readonly IInstancesManager _instancesManager;
		private readonly ReactiveCommand<IInstanceViewModel, Unit> _restoreInstanceWindowCommand;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public MinimizedInstancesPanelViewModel(IInstancesManager instancesManager = null)
		{
			_instancesManager = instancesManager ?? Locator.CurrentMutable.GetService<IInstancesManager>();

			// Restore window
			_restoreInstanceWindowCommand = ReactiveCommand.Create<IInstanceViewModel>(instance => RestoreInstanceWindowCommandAction(instance));
		}

		public IReactiveDerivedList<IInstanceViewModel> Instances => _instancesManager.GetMinimizedInstances();

		public ReactiveCommand<IInstanceViewModel, Unit> RestoreInstanceWindowCommand => _restoreInstanceWindowCommand;

		private void RestoreInstanceWindowCommandAction(IInstanceViewModel instance)
		{
			instance.IsMinimized = false;
			User32Methods.ShowWindow(instance.ProcessMainWindowHandle, ShowWindowCommands.SW_RESTORE);
		}
	}
}
