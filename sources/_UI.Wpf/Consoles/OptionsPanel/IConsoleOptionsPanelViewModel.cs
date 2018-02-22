using System.Collections.Generic;
using System.Reactive;
using Consoles.Core;
using ReactiveUI;

namespace UI.Wpf.Consoles
{
	public interface IConsoleOptionsPanelViewModel
	{
		ReactiveCommand<Unit, List<ConsoleOption>> LoadOptions { get; set; }
	}
}