using AutoMapper;
using Notebook.Core;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;

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

			SetupMessageBus();

			SetupFilter();
		}

		/// <summary>
		/// Current notes list
		/// </summary>
		public IReactiveDerivedList<NoteDetailsViewModel> Notes => _noteDetailViewModels;

		/// <summary>
		/// Filters the notes list.
		/// </summary>
		public ReactiveCommand<string, Unit> SearchNotes { get; protected set; }

		/// <summary>
		/// Text used to filter notes.
		/// </summary>
		public string FilterText
		{
			get => _filterText;
			set => this.RaiseAndSetIfChanged(ref _filterText, value);
		}

		/// <summary>
		/// Intercepts the observable stream to apply filtering.
		/// </summary>
		public Subject<Predicate<NoteDetailsViewModel>> FilterObservable { get; set; } = new Subject<Predicate<NoteDetailsViewModel>>();

		/// <summary>
		/// Initializer method called by the view.
		/// </summary>
		public void Initialize()
		{
			var notesObsevable = Observable.Start(() =>
			{
				var entities = _notebookRepository.GetAll();
				_noteEntities = new ReactiveList<NoteEntity>(entities);
				_noteEntities.ChangeTrackingEnabled = true;
			});

			notesObsevable.Subscribe(_ =>
			{
				_noteDetailViewModels = _noteEntities.CreateDerivedCollection(
					selector: x =>
					{
						var noteDetailsViewModel = Mapper.Map<NoteDetailsViewModel>(x);
						FilterObservable.Subscribe(match =>
						{
							if (match(noteDetailsViewModel))
							{
								noteDetailsViewModel.FilterVisibility =  Visibility.Visible;
								return;
							}
							noteDetailsViewModel.FilterVisibility =  Visibility.Collapsed;
						});
						return noteDetailsViewModel;
					},
					filter: x => true,
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
		}

		/// <summary>
		/// Wireup the message this view model wil listen to.
		/// </summary>
		private void SetupMessageBus()
		{
			MessageBus.Current.Listen<NoteAddedMessage>().Subscribe(message =>
			{
				_noteEntities.Add(message.NewNote);
			});

			MessageBus.Current.Listen<NoteDeletedMessage>().Subscribe(message =>
			{
				var noteEntity = _noteEntities.First(n => n.Id == message.DeletedNote.Id);

				_noteEntities.Remove(noteEntity);
			});

			MessageBus.Current.Listen<NoteEditedMessage>().Subscribe(message =>
			{
				var noteEntity = _noteEntities.First(n => n.Id == message.OldNote.Id);

				_noteEntities.Remove(noteEntity);
				_noteEntities.Add(message.NewNote);
			});
		}

		/// <summary>
		/// Set the obsevable to apply filters whent the search text changes.
		/// </summary>
		public void SetupFilter()
		{
			this.ObservableForProperty(
				viewModel => viewModel.FilterText
			)
			.Throttle(
				TimeSpan.FromMilliseconds(375)
			)
			.Subscribe(property =>
			{
				var filterText = property.Value;

				if (string.IsNullOrEmpty(filterText))
				{
					FilterObservable.OnNext(x => true);

					return;
				}

				FilterObservable.OnNext(
					x => x.Title.ToLower().Contains(filterText.ToLower())
					|| x.Description.ToLower().Contains(filterText.ToLower())
				);
			});
		}
	}
}
