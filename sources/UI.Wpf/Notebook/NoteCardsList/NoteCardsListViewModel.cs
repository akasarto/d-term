using AutoMapper;
using Notebook.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace UI.Wpf.Notebook
{
	public class NoteCardsListViewModel : ReactiveObject
	{
		//
		private readonly IMapper _mapper = null;
		private readonly INotebookRepository _notebookRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public NoteCardsListViewModel(IMapper mapper, INotebookRepository notebookRepository)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), nameof(NoteCardsListViewModel));
			_notebookRepository = notebookRepository ?? throw new ArgumentNullException(nameof(notebookRepository), nameof(NoteCardsListViewModel));
		}

		/// <summary>
		/// Current notes list
		/// </summary>
		public ReactiveList<NoteCardViewModel> NoteCards { get; set; } = new ReactiveList<NoteCardViewModel>();

		/// <summary>
		/// Initializer method called by the view.
		/// </summary>
		public void Initialize()
		{
			var notesObsevable = Observable.Start(LoadNotes);

			notesObsevable.Subscribe(notes =>
			{
				NoteCards = new ReactiveList<NoteCardViewModel>(notes);
				this.RaisePropertyChanged(nameof(NoteCards));
			});
		}

		/// <summary>
		/// Load items from repository.
		/// </summary>
		private List<NoteCardViewModel> LoadNotes()
		{
			var notes = _notebookRepository.GetAll();
			var result = new List<NoteCardViewModel>();

			foreach (var note in notes)
			{
				var cardModel = _mapper.Map(note, new NoteCardViewModel(_mapper, _notebookRepository));

				result.Add(cardModel);
			}

			return result;
		}
	}
}
