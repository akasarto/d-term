using ReactiveUI;
using System;

namespace UI.Wpf.Notebook
{
	public class NotebookViewModel : ReactiveObject
	{
		//
		private readonly NotesListViewModel _notesListViewModel = null;

		/// <summary>
		/// Constructor.
		/// </summary>
		public NotebookViewModel(NotesListViewModel notesListViewModel)
		{
			_notesListViewModel = notesListViewModel ?? throw new ArgumentNullException(nameof(notesListViewModel), nameof(NotebookViewModel));
		}

		/// <summary>
		/// NOtes List View Model
		/// </summary>
		public NotesListViewModel NotesListViewModel => _notesListViewModel;
	}
}
