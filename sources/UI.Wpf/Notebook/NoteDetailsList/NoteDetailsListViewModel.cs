using AutoMapper;
using Notebook.Core;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Linq;

namespace UI.Wpf.Notebook
{
	public class NoteDetailsListViewModel : ReactiveObject
	{
		//
		private bool _isLoading;
		private string _filterText;
		private int _showingCount;
		private ReactiveList<NoteEntity> _noteEntities;
		private IReactiveDerivedList<NoteDetailsViewModel> _noteDetailViewModels;

		//
		private readonly INotebookRepository _notebookRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public NoteDetailsListViewModel(INotebookRepository notebookRepository)
		{
			_notebookRepository = notebookRepository ?? throw new ArgumentNullException(nameof(notebookRepository), nameof(NoteDetailsListViewModel));

			_noteEntities = new ReactiveList<NoteEntity>()
			{
				ChangeTrackingEnabled = true
			};

			var filterChangedObservable = this.ObservableForProperty(
				viewModel => viewModel.FilterText
			).Throttle(
				TimeSpan.FromMilliseconds(275),
				RxApp.MainThreadScheduler
			);

			Notes = _noteEntities.CreateDerivedCollection(
				filter: noteEntity => ApplyFilter(noteEntity),
				selector: noteEntity => BuildDetailsViewModel(noteEntity),
				orderer: (noteDetailsViewModelX, noteDetailsViewModelY) =>
				{
					// If same index, order by title.
					int dresult = noteDetailsViewModelX.Index.CompareTo(noteDetailsViewModelY.Index);
					if (dresult == 0) noteDetailsViewModelX.Title.CompareTo(noteDetailsViewModelY.Title);
					return dresult;
				},
				signalReset: filterChangedObservable
			);

			_noteDetailViewModels.CountChanged.Subscribe(count =>
			{
				ShowingCount = count;
			});

			SetupMessageBus();
		}

		/// <summary>
		/// Flags when data is being loaded from the repository.
		/// </summary>
		public bool IsLoading
		{
			get => _isLoading;
			set => this.RaiseAndSetIfChanged(ref _isLoading, value);
		}

		/// <summary>
		/// Text used to filter notes.
		/// </summary>
		public string FilterText
		{
			get => _filterText;
			set => this.RaiseAndSetIfChanged(ref _filterText, value);
		}

		/// <summary>
		/// Current notes list
		/// </summary>
		public IReactiveDerivedList<NoteDetailsViewModel> Notes
		{
			get => _noteDetailViewModels;
			set => this.RaiseAndSetIfChanged(ref _noteDetailViewModels, value);
		}

		/// <summary>
		/// Tracks how many items are being shown.
		/// </summary>
		public int ShowingCount
		{
			get => _showingCount;
			set => this.RaiseAndSetIfChanged(ref _showingCount, value);
		}

		/// <summary>
		/// Initializer method called by the view.
		/// </summary>
		public void Initialize()
		{
			IsLoading = true;

			Observable.Start(() =>
			{
				var entities = _notebookRepository.GetAll();
				return entities;
			}, RxApp.MainThreadScheduler)
			.Subscribe(items =>
			{
				IsLoading = false;
				_noteEntities.AddRange(items);
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
				var noteEntity = _noteEntities.FirstOrDefault(n => n.Id == message.DeletedNote.Id);

				if (noteEntity != null)
				{
					_noteEntities.Remove(noteEntity);
				}
			});

			MessageBus.Current.Listen<NoteEditedMessage>().Subscribe(message =>
			{
				var noteEntity = _noteEntities.FirstOrDefault(n => n.Id == message.OldNote.Id);

				if (noteEntity != null)
				{
					_noteEntities.Remove(noteEntity);
					_noteEntities.Add(message.NewNote);
				}
			});
		}

		/// <summary>
		/// Check if the current entity shoul be discarded by the filter.
		/// </summary>
		private bool ApplyFilter(NoteEntity noteEntity)
		{
			var filterText = FilterText;

			if (!string.IsNullOrEmpty(filterText))
			{
				var titleMatch = noteEntity.Title.ToLower().Contains(filterText.ToLower());
				var descriptionMatch = noteEntity.Description.ToLower().Contains(filterText.ToLower());

				return titleMatch || descriptionMatch;
			}

			return true;
		}

		/// <summary>
		/// Gets the view model for the current entity.
		/// </summary>
		private NoteDetailsViewModel BuildDetailsViewModel(NoteEntity noteEntity)
		{
			return Mapper.Map<NoteDetailsViewModel>(noteEntity);
		}
	}
}
