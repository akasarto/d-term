using ReactiveUI;
using System;

namespace UI.Wpf.Notebook
{
	public class NoteListItemViewModel : ReactiveObject
	{
		private bool _isEditing = false;
		private NoteViewModel _note;

		public NoteListItemViewModel()
		{
			EditCommand = ReactiveCommand.Create(() =>
			{
				Note = new NoteViewModel();

				Note.Id = this.Id;
				Note.Index = this.Index;
				Note.Title = this.Title;
				Note.Description = this.Description;

				IsEditing = true;
			});

			CancelCommand = ReactiveCommand.Create(() =>
			{
				IsEditing = false;
			});

			SaveCommand = ReactiveCommand.Create(() =>
			{
				System.Windows.MessageBox.Show(Note?.Description ?? ":(");
			});
		}

		public Guid Id { get; set; }
		public int Index { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }

		public bool IsEditing
		{
			get => _isEditing;
			set => this.RaiseAndSetIfChanged(ref _isEditing, value);
		}

		public NoteViewModel Note
		{
			get => _note;
			set => this.RaiseAndSetIfChanged(ref _note, value);
		}

		public ReactiveCommand EditCommand { get; protected set; }
		public ReactiveCommand CancelCommand { get; protected set; }
		public ReactiveCommand SaveCommand { get; protected set; }
	}
}
