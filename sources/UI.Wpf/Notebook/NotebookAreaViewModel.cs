using ReactiveUI;
using System;

namespace UI.Wpf.Notebook
{
	public class NotebookAreaViewModel : ReactiveObject
	{
		//
		private readonly NotesListViewModel _notesListViewModel = null;

		/// <summary>
		/// Constructor.
		/// </summary>
		public NotebookAreaViewModel(NotesListViewModel notesListViewModel)
		{
			_notesListViewModel = notesListViewModel ?? throw new ArgumentNullException(nameof(notesListViewModel), nameof(NotebookAreaViewModel));
		}

		/// <summary>
		/// NOtes List View Model
		/// </summary>
		public NotesListViewModel NotesListViewModel => _notesListViewModel;
	}
}
