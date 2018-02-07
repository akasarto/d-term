using ReactiveUI;
using System;

namespace UI.Wpf.Notebook
{
	public class NotebookWorkspaceViewModel : ReactiveObject
	{
		//
		private readonly NoteCardsListViewModel _noteCardsListViewModel = null;
		private readonly NoteCardViewModel _noteCardViewModel = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public NotebookWorkspaceViewModel(NoteCardsListViewModel noteCardsListViewModel, NoteCardViewModel noteCardViewModel)
		{
			_noteCardsListViewModel = noteCardsListViewModel ?? throw new ArgumentNullException(nameof(noteCardsListViewModel), nameof(NotebookWorkspaceViewModel));
			_noteCardViewModel = noteCardViewModel ?? throw new ArgumentNullException(nameof(noteCardViewModel), nameof(NotebookWorkspaceViewModel));
		}

		/// <summary>
		/// Note Cards List View Model
		/// </summary>
		public NoteCardsListViewModel NoteCardsListViewModel => _noteCardsListViewModel;

		/// <summary>
		/// Add Note View Model
		/// </summary>
		public NoteCardViewModel AddNoteViewModel => _noteCardViewModel;
	}
}
