using MaterialDesignThemes.Wpf;
using ReactiveUI;
using System;
using UI.Wpf.Consoles;
using UI.Wpf.Notebook;

namespace UI.Wpf.Shell
{
	public class ShellViewModel : ReactiveObject
	{
		//
		private readonly ISnackbarMessageQueue _snackbarMessageQueue = null;
		private readonly ConsolesWorkspaceViewModel _consolesWorkspaceViewModel = null;
		private readonly NotebookWorkspaceViewModel _notebookWorkspaceViewModel = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(ISnackbarMessageQueue snackbarMessageQueue, ConsolesWorkspaceViewModel consolesWorkspaceViewModel, NotebookWorkspaceViewModel notebookWorkspaceViewModel)
		{
			_snackbarMessageQueue = snackbarMessageQueue ?? throw new ArgumentNullException(nameof(snackbarMessageQueue), nameof(ConsolesWorkspaceViewModel));
			_consolesWorkspaceViewModel = consolesWorkspaceViewModel ?? throw new ArgumentNullException(nameof(consolesWorkspaceViewModel), nameof(ShellViewModel));
			_notebookWorkspaceViewModel = notebookWorkspaceViewModel ?? throw new ArgumentNullException(nameof(notebookWorkspaceViewModel), nameof(ShellViewModel));
		}

		/// <summary>
		/// Consoles workspace view model.
		/// </summary>
		public ConsolesWorkspaceViewModel ConsolesWorkspaceViewModel => _consolesWorkspaceViewModel;

		/// <summary>
		/// Notebook workspace view model
		/// </summary>
		public NotebookWorkspaceViewModel NotebookWorkspaceViewModel => _notebookWorkspaceViewModel;

		/// <summary>
		/// Global messages queue.
		/// </summary>
		public ISnackbarMessageQueue MessageQueue => _snackbarMessageQueue;
	}
}
