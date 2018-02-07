using AutoMapper;
using Notebook.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace UI.Wpf.Notebook
{
	public class NoteDetailsListListViewModel : ReactiveObject
	{
		//
		private readonly INotebookRepository _notebookRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public NoteDetailsListListViewModel(INotebookRepository notebookRepository)
		{
			_notebookRepository = notebookRepository ?? throw new ArgumentNullException(nameof(notebookRepository), nameof(NoteDetailsListListViewModel));
		}

		/// <summary>
		/// Current notes list
		/// </summary>
		public ReactiveList<NoteDetailsViewModel> NoteCards { get; set; } = new ReactiveList<NoteDetailsViewModel>();

		/// <summary>
		/// Initializer method called by the view.
		/// </summary>
		public void Initialize()
		{
			var notesObsevable = Observable.Start(LoadNotes);

			notesObsevable.Subscribe(notes =>
			{
				NoteCards = new ReactiveList<NoteDetailsViewModel>(notes);
				this.RaisePropertyChanged(nameof(NoteCards));
			});
		}

		/// <summary>
		/// Load items from repository.
		/// </summary>
		private List<NoteDetailsViewModel> LoadNotes()
		{
			var notes = _notebookRepository.GetAll();
			var result = Mapper.Map<List<NoteDetailsViewModel>>(notes);

			return result;
		}
	}
}
