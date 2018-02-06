using ReactiveUI;
using System;

namespace UI.Wpf.Notebook
{
	public class NotebookWorkspaceViewModel : ReactiveObject
	{
		//
		private readonly NotesListViewModel _notesListViewModel = null;

		/// <summary>
		/// Constructor.
		/// </summary>
		public NotebookWorkspaceViewModel(NotesListViewModel notesListViewModel)
		{
			_notesListViewModel = notesListViewModel ?? throw new ArgumentNullException(nameof(notesListViewModel), nameof(NotebookWorkspaceViewModel));
		}

		/// <summary>
		/// NOtes List View Model
		/// </summary>
		public NotesListViewModel NotesListViewModel => _notesListViewModel;
	}
}
