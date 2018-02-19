﻿using AutoMapper;
using Notebook.Core;
using ReactiveUI;
using System;

namespace UI.Wpf.Notebook
{
	public class NoteDetailsViewModel : ReactiveObject
	{
		//
		private Guid _id;
		private int _intex;
		private string _title;
		private string _description;
		private NoteFormViewModel _formData;
		private bool _isPopupOpen;
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
		/// Cloned note data to use in the form.
		/// </summary>
		public NoteFormViewModel FormData
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
		/// Tracks whether the delete popup is open or not.
		/// </summary>
		public bool IsPopupOpen
		{
			get => _isPopupOpen;
			set => this.RaiseAndSetIfChanged(ref _isPopupOpen, value);
		}

		/// <summary>
		/// Wire up commands with their respective actions.
		/// </summary>
		private void SetupCommands()
		{
			EditCommand = ReactiveCommand.Create(() =>
			{
				FormData = Mapper.Map<NoteFormViewModel>(this);

				IsFlipped = true;
			});

			DeleteCommand = ReactiveCommand.Create(() =>
			{
				_notebookRepository.Delete(Id);

				var deletedNoteEntity = Mapper.Map<NoteEntity>(this);

				MessageBus.Current.SendMessage(new NoteDeletedMessage(deletedNoteEntity));

				IsPopupOpen = false;
			});

			CancelCommand = ReactiveCommand.Create(() =>
			{
				IsFlipped = false;
			});

			SaveCommand = ReactiveCommand.Create(() =>
			{
				FormData.Validate();

				if (FormData.IsValid)
				{
					var newNoteEntity = Mapper.Map<NoteEntity>(FormData);
					var oldNoteEntity = Mapper.Map<NoteEntity>(this);

					_notebookRepository.Update(newNoteEntity);

					MessageBus.Current.SendMessage(new NoteEditedMessage(newNoteEntity, oldNoteEntity));

					IsFlipped = false;
				}
			});
		}
	}
}
