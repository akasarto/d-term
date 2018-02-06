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
		private readonly NotebookAreaViewModel _notebookAreaViewModel = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(ConsoleAreaViewModel consolesAreaViewModel, NotebookAreaViewModel notebookAreaViewModel)
		{
			_consolesAreaViewModel = consolesAreaViewModel ?? throw new ArgumentNullException(nameof(consolesAreaViewModel), nameof(ShellViewModel));
			_notebookAreaViewModel = notebookAreaViewModel ?? throw new ArgumentNullException(nameof(notebookAreaViewModel), nameof(ShellViewModel));
		}

		/// <summary>
		/// Consoles Area View Model
		/// </summary>
		public ConsoleAreaViewModel ConsolesAreaViewModel => _consolesAreaViewModel;

		/// <summary>
		/// Notebooks Area View Model
		/// </summary>
		public NotebookAreaViewModel NotebookAreaViewModel => _notebookAreaViewModel;

		/// <summary>
		/// Initializer called by the view.
		/// </summary>
		public void Initialize()
		{
		}
	}
}
