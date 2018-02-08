using AutoMapper;
using Notebook.Core;
using ReactiveUI;
using System;

namespace UI.Wpf.Notebook
{
	public class NoteAddViewModel : ReactiveObject
	{
		//
		private string _title;
		private string _description;
		private NoteViewModel _formData;
		private bool _isFlipped;

		//
		private readonly INotebookRepository _notebookRepository = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public NoteAddViewModel(INotebookRepository notebookRepository)
		{
			_notebookRepository = notebookRepository ?? throw new ArgumentNullException(nameof(notebookRepository), nameof(NoteAddViewModel));

			SetupCommands();
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
		/// Add a new note.
		/// </summary>
		public ReactiveCommand AddCommand { get; protected set; }

		/// <summary>
		/// Cancel note add/edit.
		/// </summary>
		public ReactiveCommand CancelCommand { get; protected set; }

		/// <summary>
		/// Save note when adding/editing.
		/// </summary>
		public ReactiveCommand SaveCommand { get; protected set; }

		/// <summary>
		/// Cloned note data to use in the form.
		/// </summary>
		public NoteViewModel FormData
		{
			get => _formData;
			set => this.RaiseAndSetIfChanged(ref _formData, value);
		}

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
			AddCommand = ReactiveCommand.Create(() =>
			{
				FormData = new NoteViewModel();

				IsFlipped = true;
			});

			CancelCommand = ReactiveCommand.Create(() =>
			{
				IsFlipped = false;
			});

			SaveCommand = ReactiveCommand.Create(() =>
			{
				var note = Mapper.Map<NoteEntity>(FormData);

				note = _notebookRepository.Add(note);

				MessageBus.Current.SendMessage(new NoteAddedMessage(note));

				IsFlipped = false;
			});
		}
	}
}
