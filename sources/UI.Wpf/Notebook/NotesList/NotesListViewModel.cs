using AutoMapper;
using Notebook.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace UI.Wpf.Notebook
{
	public class NotesListViewModel : ReactiveObject
	{
		//
		private readonly IMapper _mapper = null;
		private readonly INotebookRepository _notebookRepository = null;

		/// <summary>
		/// Constructor.
		/// </summary>
		public NotesListViewModel(IMapper mapper, INotebookRepository notebookRepository)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), nameof(NotesListViewModel));
			_notebookRepository = notebookRepository ?? throw new ArgumentNullException(nameof(notebookRepository), nameof(NotesListViewModel));
		}

		/// <summary>
		/// Current notes list
		/// </summary>
		public ReactiveList<NoteViewModel> Notes { get; set; } = new ReactiveList<NoteViewModel>();

		/// <summary>
		/// Initializer method called by the view.
		/// </summary>
		public void Initialize()
		{
			var notesObsevable = Observable.Start(LoadNotes);

			notesObsevable.Subscribe(notes =>
			{
				Notes = new ReactiveList<NoteViewModel>(notes);
				this.RaisePropertyChanged(nameof(Notes));
			});
		}

		/// <summary>
		/// Load items from repository.
		/// </summary>
		private List<NoteViewModel> LoadNotes()
		{
			var notes = _notebookRepository.GetAll();
			return _mapper.Map<List<NoteViewModel>>(notes);
		}
	}
}
