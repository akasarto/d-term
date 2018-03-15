using MaterialDesignThemes.Wpf;
using ReactiveUI;
using Splat;
using UI.Wpf.Processes;

namespace UI.Wpf.Shell
{
	//
	public interface IShellViewModel
	{
		IAppContext AppContext { get; }
		IProcessesController Processes { get; }
		ISnackbarMessageQueue SnackbarMessageQueue { get; }
	}

	//
	public class ShellViewModel : ReactiveObject, IShellViewModel
	{
		private readonly IAppContext _appContext;
		private readonly IProcessesController _processesController;
		private readonly ISnackbarMessageQueue _snackbarMessageQueue;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(IAppContext appContext = null, IProcessesController processesController = null, ISnackbarMessageQueue snackbarMessageQueue = null)
		{
			_appContext = appContext ?? Locator.CurrentMutable.GetService<IAppContext>();
			_processesController = processesController ?? Locator.CurrentMutable.GetService<IProcessesController>();
			_snackbarMessageQueue = snackbarMessageQueue ?? Locator.CurrentMutable.GetService<ISnackbarMessageQueue>();
		}

		public IAppContext AppContext => _appContext;
		public IProcessesController Processes => _processesController;
		public ISnackbarMessageQueue SnackbarMessageQueue => _snackbarMessageQueue;
	}
}
