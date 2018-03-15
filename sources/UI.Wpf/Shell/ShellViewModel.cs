using MaterialDesignThemes.Wpf;
using ReactiveUI;
using Splat;
using UI.Wpf.Processes;

namespace UI.Wpf.Shell
{
	//
	public interface IShellViewModel
	{
		IAppState AppState { get; }
		IProcessesController Processes { get; }
		ISnackbarMessageQueue SnackbarMessageQueue { get; }
	}

	//
	public class ShellViewModel : ReactiveObject, IShellViewModel
	{
		private readonly IAppState _appContext;
		private readonly IProcessesController _processesController;
		private readonly ISnackbarMessageQueue _snackbarMessageQueue;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(IAppState appContext = null, IProcessesController processesController = null, ISnackbarMessageQueue snackbarMessageQueue = null)
		{
			_appContext = appContext ?? Locator.CurrentMutable.GetService<IAppState>();
			_processesController = processesController ?? Locator.CurrentMutable.GetService<IProcessesController>();
			_snackbarMessageQueue = snackbarMessageQueue ?? Locator.CurrentMutable.GetService<ISnackbarMessageQueue>();
		}

		public IAppState AppState => _appContext;
		public IProcessesController Processes => _processesController;
		public ISnackbarMessageQueue SnackbarMessageQueue => _snackbarMessageQueue;
	}
}
