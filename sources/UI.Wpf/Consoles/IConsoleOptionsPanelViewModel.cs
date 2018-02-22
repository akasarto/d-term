using Consoles.Core;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;

namespace UI.Wpf.Consoles
{
	/// <summary>
	/// Console options panel view model interface.
	/// </summary>
	public interface IConsoleOptionsPanelViewModel
	{
		/// <summary>
		/// Gets or sets the command responsible for loading the exising console options.
		/// </summary>
		ReactiveCommand<Unit, List<ConsoleOption>> LoadOptions { get; }
	}
}
