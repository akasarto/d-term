using ReactiveUI;
using System;

namespace UI.Wpf.Notebook
{
	public class AddNoteViewModel : ReactiveObject
	{
		private bool _isAdding;
		private NoteViewModel _note;

		public AddNoteViewModel()
		{
			AddCommand = ReactiveCommand.Create(() =>
			{
				Note = new NoteViewModel()
				{
					Id = Guid.NewGuid()
				};

				IsAdding = true;
			});

			CancelCommand = ReactiveCommand.Create(() =>
			{
				IsAdding = false;
			});

			SaveCommand = ReactiveCommand.Create(() =>
			{
				System.Windows.MessageBox.Show(Note?.Title ?? ":(");
			});
		}

		public bool IsAdding
		{
			get => _isAdding;
			set => this.RaiseAndSetIfChanged(ref _isAdding, value);
		}

		public NoteViewModel Note
		{
			get => _note;
			set => this.RaiseAndSetIfChanged(ref _note, value);
		}

		public ReactiveCommand AddCommand { get; protected set; }
		public ReactiveCommand CancelCommand { get; protected set; }
		public ReactiveCommand SaveCommand { get; protected set; }
	}
}
