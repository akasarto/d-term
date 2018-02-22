using ReactiveUI;
using System;
using UI.Wpf.Consoles;

namespace UI.Wpf.Shell
{
	/// <summary>
	/// App shell view model implementation.
	/// <seealso cref="IShellViewModel"/>
	/// </summary>
	public class ShellViewModel : ReactiveObject, IShellViewModel
	{
		private string _appTitle;

		//
		private IConsoleOptionsPanelViewModel _consoleOptionsPanelViewModel = null;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel(/*IConsoleOptionsPanelViewModel consoleOptionsPanelViewModel*/)
		{
			//_consoleOptionsPanelViewModel = consoleOptionsPanelViewModel ?? throw new ArgumentNullException(nameof(consoleOptionsPanelViewModel), nameof(ShellViewModel));
		}

		/// <summary>
		/// Gets or sets the app title.
		/// </summary>
		public string AppTitle
		{
			get => _appTitle;
			set => this.RaiseAndSetIfChanged(ref _appTitle, value);
		}

		/// <summary>
		/// Gets or sets the console options panel view model.
		/// </summary>
		public IConsoleOptionsPanelViewModel ConsoleOptionsPanelViewModel
		{
			get => _consoleOptionsPanelViewModel;
			set => this.RaiseAndSetIfChanged(ref _consoleOptionsPanelViewModel, value);
		}
	}
}
