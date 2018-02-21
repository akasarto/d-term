using Consoles.Core;
using ReactiveUI;

namespace UI.Wpf.Consoles
{
	public class ProcessBasePathViewModel : BaseViewModel
	{
		private ProcessBasePath _basePath;
		private string _description;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessBasePathViewModel()
		{
		}

		/// <summary>
		/// Gets or sets the base path.
		/// </summary>
		public ProcessBasePath BasePath
		{
			get => _basePath;
			set => this.RaiseAndSetIfChanged(ref _basePath, value);
		}

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		public string Description
		{
			get => _description;
			set => this.RaiseAndSetIfChanged(ref _description, value);
		}
	}
}
