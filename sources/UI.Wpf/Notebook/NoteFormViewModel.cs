using ReactiveUI;

namespace UI.Wpf.Notebook
{
	public class NoteFormViewModel : ReactiveObject
	{
		private bool _isFormVisible = false;
		//private NoteViewModel _note = null;

		public NoteFormViewModel()
		{

		}

		public bool IsFormVisible
		{
			get => _isFormVisible;
			set => this.RaiseAndSetIfChanged(ref _isFormVisible, value);
		}

		//public NoteViewModel Note
		//{
			
		//}
	}
}
