using ReactiveUI;
using System;
using UI.Wpf.Consoles;
using UI.Wpf.Notebook;

namespace UI.Wpf.Shell
{
	public class ShellViewModel : ReactiveObject
	{
		//
		private readonly ConsoleAreaViewModel _consolesAreaViewModel = null;
		private readonly NotebookViewModel _notebookViewModel = null;

		/// <summary>
		/// Constructor.
		/// </summary>
		public ShellViewModel(ConsoleAreaViewModel consolesAreaViewModel, NotebookViewModel notebookViewModel)
		{
			_consolesAreaViewModel = consolesAreaViewModel ?? throw new ArgumentNullException(nameof(consolesAreaViewModel), nameof(ShellViewModel));
			_notebookViewModel = notebookViewModel ?? throw new ArgumentNullException(nameof(notebookViewModel), nameof(ShellViewModel));
		}

		/// <summary>
		/// Consoles Area View Model
		/// </summary>
		public ConsoleAreaViewModel ConsolesAreaViewModel => _consolesAreaViewModel;

		/// <summary>
		/// Notebook Area View Model
		/// </summary>
		public NotebookViewModel NotebookAreaViewModel => _notebookViewModel;
	}
}
