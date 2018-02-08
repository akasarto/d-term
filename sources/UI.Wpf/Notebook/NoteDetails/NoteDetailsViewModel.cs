using AutoMapper;
using Notebook.Core;
using ReactiveUI;
using System;
using System.Windows;

namespace UI.Wpf.Notebook
{
	public class NoteDetailsViewModel : ReactiveObject
	{
		//
		private Guid _id;
		private int _intex;
		private string _title;
		private string _description;
		private Visibility _filterVisibility;
		private NoteViewModel _formData;
		private bool _isFlipped;

		//
		private readonly INotebookRepository _notebookRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public NoteDetailsViewModel(INotebookRepository notebookRepository)
		{
			_notebookRepository = notebookRepository ?? throw new ArgumentNullException(nameof(notebookRepository), nameof(NoteDetailsViewModel));

			SetupCommands();
		}

		/// <summary>
		/// Gets or sets the id.
		/// </summary>
		public Guid Id
		{
			get => _id;
			set => this.RaiseAndSetIfChanged(ref _id, value);
		}

		/// <summary>
		/// Gets or sets the order index.
		/// </summary>
		public int Index
		{
			get => _intex;
			set => this.RaiseAndSetIfChanged(ref _intex, value);
		}

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		public string Title
		{
			get => _title;
			set => this.RaiseAndSetIfChanged(ref _title, value);
		}

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		public string Description
		{
			get => _description;
			set => this.RaiseAndSetIfChanged(ref _description, value);
		}

		/// <summary>
		/// Flags when this item was filtered out from the list.
		/// </summary>
		public Visibility FilterVisibility
		{
			get { return _filterVisibility; }
			set { this.RaiseAndSetIfChanged(ref _filterVisibility, value); }
		}

		/// <summary>
		/// Cloned note data to use in the form.
		/// </summary>
		public NoteViewModel FormData
		{
			get => _formData;
			set => this.RaiseAndSetIfChanged(ref _formData, value);
		}

		/// <summary>
		/// Edit a note.
		/// </summary>
		public ReactiveCommand EditCommand { get; protected set; }

		/// <summary>
		/// Delete a note.
		/// </summary>
		public ReactiveCommand DeleteCommand { get; protected set; }

		/// <summary>
		/// Cancel note add/edit.
		/// </summary>
		public ReactiveCommand CancelCommand { get; protected set; }

		/// <summary>
		/// Save note when adding/editing.
		/// </summary>
		public ReactiveCommand SaveCommand { get; protected set; }

		/// <summary>
		/// Flags whick side of the card is being shown. If flipped, the add/edit form is visible.
		/// </summary>
		public bool IsFlipped
		{
			get => _isFlipped;
			set => this.RaiseAndSetIfChanged(ref _isFlipped, value);
		}

		/// <summary>
		/// Wire up commands with their respective actions.
		/// </summary>
		private void SetupCommands()
		{
			EditCommand = ReactiveCommand.Create(() =>
			{
				FormData = Mapper.Map<NoteViewModel>(this);

				IsFlipped = true;
			});

			DeleteCommand = ReactiveCommand.Create(() =>
			{
				_notebookRepository.Delete(Id);

				var deletedNoteEntity = Mapper.Map<NoteEntity>(this);

				MessageBus.Current.SendMessage(new NoteDeletedMessage(deletedNoteEntity));
			});

			CancelCommand = ReactiveCommand.Create(() =>
			{
				IsFlipped = false;
			});

			SaveCommand = ReactiveCommand.Create(() =>
			{
				var newNoteEntity = Mapper.Map<NoteEntity>(FormData);
				var oldNoteEntity = Mapper.Map<NoteEntity>(this);

				_notebookRepository.Update(newNoteEntity);

				MessageBus.Current.SendMessage(new NoteEditedMessage(newNoteEntity, oldNoteEntity));

				IsFlipped = false;
			});
		}
	}
}
