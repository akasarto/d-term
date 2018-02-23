using Consoles.Core;
using ReactiveUI;
using System.Reactive;

namespace UI.Wpf.Consoles
{
	public interface IConsoleOptionsPanelViewModel
	{
		bool IsBusy { get; }
		ReactiveCommand<Unit, IReactiveList<ConsoleOption>> LoadOptionsCommand { get; }
		IConsoleOptionsListViewModel ConsoleOptionsListViewModel { get; }
	}
}
