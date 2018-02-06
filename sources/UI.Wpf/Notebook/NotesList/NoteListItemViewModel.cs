using ReactiveUI;
using System;

namespace UI.Wpf.Notebook
{
	public class NoteListItemViewModel : ReactiveObject
	{
		private bool _isEditing = false;

		public NoteListItemViewModel()
		{
			EditCommand = ReactiveCommand.Create(() =>
			{
				IsEditing = true;
			});

			CancelCommand = ReactiveCommand.Create(() =>
			{
				IsEditing = false;
			});

			SaveCommand = ReactiveCommand.Create(() =>
			{
				System.Windows.MessageBox.Show("Save");
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

		public ReactiveCommand EditCommand { get; protected set; }
		public ReactiveCommand CancelCommand { get; protected set; }
		public ReactiveCommand SaveCommand { get; protected set; }
	}
}
