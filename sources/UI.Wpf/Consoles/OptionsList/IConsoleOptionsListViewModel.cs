using Consoles.Core;
using ReactiveUI;

namespace UI.Wpf.Consoles
{
	public interface IConsoleOptionsListViewModel
	{
		IReactiveDerivedList<ConsoleOptionViewModel> Items { get; }
		ReactiveCommand<IReactiveList<ConsoleOption>, IReactiveDerivedList<ConsoleOptionViewModel>> InitializeItemsCommand { get; }
	}
}
