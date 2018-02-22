using Consoles.Core;
using ReactiveUI;
using System.Collections.Generic;
using System.Reactive;

namespace UI.Wpf.Consoles
{
	public interface IConsoleOptionsPanelViewModel
	{
		ReactiveCommand<Unit, List<ConsoleOption>> LoadOptions { get; }
	}
}
