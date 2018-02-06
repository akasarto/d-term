using ReactiveUI;

namespace UI.Wpf.Notebook
{
	public class AddNoteViewModel : ReactiveObject
	{
		private bool _isAdding = false;

		public AddNoteViewModel()
		{
			AddCommand = ReactiveCommand.Create(() =>
			{
				IsAdding = true;
			});

			CancelCommand = ReactiveCommand.Create(() =>
			{
				IsAdding = false;
			});

			SaveCommand = ReactiveCommand.Create(() =>
			{
				System.Windows.MessageBox.Show("Save");
			});
		}

		public bool IsAdding
		{
			get => _isAdding;
			set => this.RaiseAndSetIfChanged(ref _isAdding, value);
		}

		public ReactiveCommand AddCommand { get; protected set; }
		public ReactiveCommand CancelCommand { get; protected set; }
		public ReactiveCommand SaveCommand { get; protected set; }
	}
}
