using MaterialDesignThemes.Wpf;
using ReactiveUI;
using Splat;
using UI.Wpf.Processes;

namespace UI.Wpf.Shell
{
	//
	public interface IShellViewModel
	{
		IProcessesController Processes { get; }
		ISnackbarMessageQueue MessageQueue { get; }
	}

	//
	public class ShellViewModel : ReactiveObject, IShellViewModel
	{
		private readonly IProcessesController _processesController;
		
		private readonly ISnackbarMessageQueue _snackbarMessageQueue;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(IProcessesController processesController = null, ISnackbarMessageQueue snackbarMessageQueue = null)
		{
			_processesController = processesController ?? Locator.CurrentMutable.GetService<IProcessesController>();
			_snackbarMessageQueue = snackbarMessageQueue ?? Locator.CurrentMutable.GetService<ISnackbarMessageQueue>();
		}

		public IProcessesController Processes => _processesController;

		public ISnackbarMessageQueue MessageQueue => _snackbarMessageQueue;
	}
}
