using ReactiveUI;
using System;
using UI.Wpf.Consoles;
using UI.Wpf.Notebook;

namespace UI.Wpf.Shell
{
	public class ShellViewModel : ReactiveObject
	{
		//
		private readonly ConsolesWorkspaceViewModel _consolesWorkspaceViewModel = null;
		private readonly NotebookWorkspaceViewModel _notebookWorkspaceViewModel = null;

		/// <summary>
		/// Constructor.
		/// </summary>
		public ShellViewModel(ConsolesWorkspaceViewModel consolesWorkspaceViewModel, NotebookWorkspaceViewModel notebookWorkspaceViewModel)
		{
			_consolesWorkspaceViewModel = consolesWorkspaceViewModel ?? throw new ArgumentNullException(nameof(consolesWorkspaceViewModel), nameof(ShellViewModel));
			_notebookWorkspaceViewModel = notebookWorkspaceViewModel ?? throw new ArgumentNullException(nameof(notebookWorkspaceViewModel), nameof(ShellViewModel));
		}

		/// <summary>
		/// Consoles Area View Model
		/// </summary>
		public ConsolesWorkspaceViewModel ConsolesWorkspaceViewModel => _consolesWorkspaceViewModel;

		/// <summary>
		/// Notebook Area View Model
		/// </summary>
		public NotebookWorkspaceViewModel NotebookWorkspaceViewModel => _notebookWorkspaceViewModel;
	}
}
