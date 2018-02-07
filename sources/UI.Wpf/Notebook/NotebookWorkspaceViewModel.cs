using ReactiveUI;
using System;

namespace UI.Wpf.Notebook
{
	public class NotebookWorkspaceViewModel : ReactiveObject
	{
		//
		private readonly NoteAddViewModel _noteAddViewModel = null;
		private readonly NoteDetailsListListViewModel _noteCardsListViewModel = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public NotebookWorkspaceViewModel(NoteAddViewModel noteAddViewModel, NoteDetailsListListViewModel noteCardsListViewModel)
		{
			_noteAddViewModel = noteAddViewModel ?? throw new ArgumentNullException(nameof(noteAddViewModel), nameof(NotebookWorkspaceViewModel));
			_noteCardsListViewModel = noteCardsListViewModel ?? throw new ArgumentNullException(nameof(noteCardsListViewModel), nameof(NotebookWorkspaceViewModel));
		}

		/// <summary>
		/// Add Note View Model
		/// </summary>
		public NoteAddViewModel NoteAddViewModel => _noteAddViewModel;

		/// <summary>
		/// Note Cards List View Model
		/// </summary>
		public NoteDetailsListListViewModel NoteCardsListViewModel => _noteCardsListViewModel;
	}
}
