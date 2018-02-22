using MaterialDesignThemes.Wpf;
using System;
using UI.Wpf.Consoles;
using UI.Wpf.Infrastructure;
using UI.Wpf.Shared;

namespace UI.Wpf.Shell
{
	public class ShellViewModel : BaseViewModel, IShellViewModel
	{
		//
		private readonly ISnackbarMessageQueue _snackbarMessageQueue = null;
		private readonly IConsoleOptionsPanelViewModel _consoleOptionsPanelViewModel = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(ISnackbarMessageQueue snackbarMessageQueue, IViewModelFactory viewModelFactory)
		{
			_snackbarMessageQueue = snackbarMessageQueue ?? throw new ArgumentNullException(nameof(snackbarMessageQueue), nameof(ShellViewModel));
			_consoleOptionsPanelViewModel = viewModelFactory.Create<IConsoleOptionsPanelViewModel>();
		}

		/// <summary>
		/// Global messages queue.
		/// </summary>
		public ISnackbarMessageQueue SnackbarMessageQueue => _snackbarMessageQueue;

		/// <summary>
		/// Get the console options panel view model.
		/// </summary>
		public IConsoleOptionsPanelViewModel ConsoleOptionsPanelViewModel => _consoleOptionsPanelViewModel;
	}
}
