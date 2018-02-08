using AutoMapper;
using Notebook.Core;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace UI.Wpf.Notebook
{
	public class NoteDetailsListListViewModel : ReactiveObject
	{
		//
		private string _filterText = null;
		private ReactiveList<NoteEntity> _noteEntities = null;
		private IReactiveDerivedList<NoteDetailsViewModel> _noteDetailViewModels;

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
		public IReactiveDerivedList<NoteDetailsViewModel> Notes => _noteDetailViewModels;

		public string FilterText
		{
			get => _filterText;
			set => this.RaiseAndSetIfChanged(ref _filterText, value);
		}

		/// <summary>
		/// Initializer method called by the view.
		/// </summary>
		public void Initialize()
		{
			var notesObsevable = Observable.Start(() =>
			{
				var entities = _notebookRepository.GetAll();
				_noteEntities = new ReactiveList<NoteEntity>(entities);
			});

			notesObsevable.Subscribe(_ =>
			{
				_noteDetailViewModels = _noteEntities.CreateDerivedCollection(
					selector: x => Mapper.Map<NoteDetailsViewModel>(x),
					filter: x =>
					{
						if (string.IsNullOrWhiteSpace(FilterText))
						{
							return true;
						}

						return x.Title.ToLower().Contains(FilterText) || x.Description.ToLower().Contains(FilterText);
					},
					orderer: (x, y) =>
					{
						// If same index, order by title.
						int dresult = x.Index.CompareTo(y.Index);
						if (dresult == 0) x.Title.CompareTo(y.Title);
						return dresult;
					}
				);

				this.RaisePropertyChanged(nameof(Notes));
			});

			//
			MessageBus.Current.Listen<NoteAddedMessage>().Subscribe(message =>
			{
				_noteEntities.Add(message.NewNote);
			});

			MessageBus.Current.Listen<NoteDeletedMessage>().Subscribe(message =>
			{
				_noteEntities.Remove(message.DeletedNote);

				//var noteDetailsViewModel = Mapper.Map<NoteDetailsViewModel>(message.NewNote);

				//Notes.Add(noteDetailsViewModel);
			});

			MessageBus.Current.Listen<NoteEditedMessage>().Subscribe(message =>
			{
				//var noteDetailsViewModel = Mapper.Map<NoteDetailsViewModel>(message.NewNote);

				//var findNote = Notes.IndexOf(noteDetailsViewModel);

				//Notes.Add(noteDetailsViewModel);
			});
		}
	}
}
