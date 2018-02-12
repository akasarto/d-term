using System;

namespace UI.Wpf.Notebook
{
	public class NotebookWorkspaceViewModel : BaseViewModel
	{
		//
		private readonly NoteAddViewModel _noteAddViewModel = null;
		private readonly NoteDetailsListViewModel _noteCardsListViewModel = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public NotebookWorkspaceViewModel(NoteAddViewModel noteAddViewModel, NoteDetailsListViewModel noteCardsListViewModel)
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
		public NoteDetailsListViewModel NoteCardsListViewModel => _noteCardsListViewModel;
	}
}
