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
		/// Gets the add note view model.
		/// </summary>
		public NoteAddViewModel NoteAddViewModel => _noteAddViewModel;

		/// <summary>
		/// Gets the note details list view model.
		/// </summary>
		public NoteDetailsListViewModel NoteCardsListViewModel => _noteCardsListViewModel;
	}
}
