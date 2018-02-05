using AutoMapper;
using Notebook.Core;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace UI.Wpf.Notebook
{
	public class NotebookAreaViewModel : ReactiveObject
	{
		private readonly IMapper _mapper = null;
		private readonly INotebookRepository _notebookRepository = null;

		public NotebookAreaViewModel(IMapper mapper, INotebookRepository notebookRepository)
		{
			_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper), nameof(NotebookAreaViewModel));
			_notebookRepository = notebookRepository ?? throw new ArgumentNullException(nameof(notebookRepository), nameof(NotebookAreaViewModel));
		}

		public ReactiveList<NoteViewModel> Notes { get; set; } = new ReactiveList<NoteViewModel>();

		public void Initialize()
		{
			var notesObsevable = Observable.Start(LoadNotes);

			notesObsevable.Subscribe(notes =>
			{
				Notes = new ReactiveList<NoteViewModel>(notes);
				this.RaisePropertyChanged(nameof(Notes));
			});
		}

		private List<NoteViewModel> LoadNotes()
		{
			var notes = _notebookRepository.GetAll();
			return _mapper.Map<List<NoteViewModel>>(notes);
		}
	}
}
