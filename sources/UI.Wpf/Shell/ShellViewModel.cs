using ReactiveUI;
using System;
using UI.Wpf.Consoles;
using UI.Wpf.Notebook;

namespace UI.Wpf.Shell
{
	public class ShellViewModel : ReactiveObject
	{
		private readonly ConsolesAreaViewModel _consolesAreaViewModel = null;
		private readonly NotebookAreaViewModel _notebookAreaViewModel = null;

		public ShellViewModel(ConsolesAreaViewModel consolesAreaViewModel, NotebookAreaViewModel notebookAreaViewModel)
		{
			_consolesAreaViewModel = consolesAreaViewModel ?? throw new ArgumentNullException(nameof(consolesAreaViewModel), nameof(ShellViewModel));
			_notebookAreaViewModel = notebookAreaViewModel ?? throw new ArgumentNullException(nameof(notebookAreaViewModel), nameof(ShellViewModel));
		}

		public ConsolesAreaViewModel ConsolesAreaViewModel => _consolesAreaViewModel;

		public NotebookAreaViewModel NotebookAreaViewModel => _notebookAreaViewModel;

		public void Initialize()
		{
		}
	}
}
