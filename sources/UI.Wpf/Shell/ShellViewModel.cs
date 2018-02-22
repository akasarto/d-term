using ReactiveUI;

namespace UI.Wpf.Shell
{
	public class ShellViewModel : ReactiveObject
	{
		private string _appTitle;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ShellViewModel()
		{
		}

		/// <summary>
		/// Gets or sets the app title.
		/// </summary>
		public string AppTitle
		{
			get => _appTitle;
			set => this.RaiseAndSetIfChanged(ref _appTitle, value);
		}
	}
}
