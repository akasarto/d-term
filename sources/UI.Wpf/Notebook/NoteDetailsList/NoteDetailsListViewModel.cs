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
				selector: noteEntity => Mapper.Map<NoteDetailsViewModel>(noteEntity),
				signalReset: filterChangedObservable,
				scheduler: RxApp.MainThreadScheduler
			);

			_noteDetailViewModels.CountChanged.Subscribe(count =>
			{
				ShowingCount = count;
			});

			SetupMessageBus();
		}

		/// <summary>
		/// Gets or sets when data is being loaded from the repository.
		/// </summary>
		public bool IsLoading
		{
			get => _isLoading;
			set => this.RaiseAndSetIfChanged(ref _isLoading, value);
		}

		/// <summary>
		/// Gets or sets the text used to filter notes.
		/// </summary>
		public string FilterText
		{
			get => _filterText;
			set => this.RaiseAndSetIfChanged(ref _filterText, value);
		}

		/// <summary>
		/// Gets or sets the current notes list.
		/// </summary>
		public IReactiveDerivedList<NoteDetailsViewModel> Notes
		{
			get => _noteDetailViewModels;
			set => this.RaiseAndSetIfChanged(ref _noteDetailViewModels, value);
		}

		/// <summary>
		/// Gets or sets how many items are being shown.
		/// </summary>
		public int ShowingCount
		{
			get => _showingCount;
			set => this.RaiseAndSetIfChanged(ref _showingCount, value);
		}

		/// <summary>
		/// Initializer the model.
		/// </summary>
		public void Initialize()
		{
			IsLoading = true;

			Observable.Start(() =>
			{
				var entities = _notebookRepository.GetAll();
				return entities;
			}).Subscribe(items =>
			{
				IsLoading = false;
				_noteEntities.AddRange(items);
			});
		}

		/// <summary>
		/// Wireup the messages this view model will listen to.
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
				var titleMatch = (noteEntity?.Title?.ToLower() ?? string.Empty).Contains(filterText.ToLower());
				var descriptionMatch = (noteEntity?.Description?.ToLower() ?? string.Empty).Contains(filterText.ToLower());

				return titleMatch || descriptionMatch;
			}

			return true;
		}
	}
}
