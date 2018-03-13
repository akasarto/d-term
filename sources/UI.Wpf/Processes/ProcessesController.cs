using ReactiveUI;
using Splat;
using System;
using System.Reactive;

namespace UI.Wpf.Processes
{
	//
	public interface IProcessesController
	{
		IConsolesPanelViewModel ConsolesPanel { get; }
		IMinimizedInstancesPanelViewModel MinimizedInstancesPanel { get; }
		ITransparencyManagerPanelViewModel TransparencyManagerPanel { get; }
		Interaction<IConfigsViewModel, Unit> OpenConfigsInteraction { get; }
		ReactiveCommand OpenConfigsCommand { get; }
	}

	//
	public class ProcessesController : IProcessesController
	{
		private readonly IConsolesPanelViewModel _consolesPanelViewModel;
		private readonly IMinimizedInstancesPanelViewModel _minimizedInstancesPanel;
		private readonly ITransparencyManagerPanelViewModel _transparencyManagerPanelViewModel;
		private readonly Interaction<IConfigsViewModel, Unit> _openConfigsInteraction;
		private readonly ReactiveCommand _openConfigsCommand;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public ProcessesController(IConsolesPanelViewModel consolesPanelViewModel = null, IMinimizedInstancesPanelViewModel minimizedInstancesPanel = null, ITransparencyManagerPanelViewModel transparencyManagerPanelViewModel = null)
		{
			_consolesPanelViewModel = consolesPanelViewModel ?? Locator.CurrentMutable.GetService<IConsolesPanelViewModel>();
			_minimizedInstancesPanel = minimizedInstancesPanel ?? Locator.CurrentMutable.GetService<IMinimizedInstancesPanelViewModel>();
			_transparencyManagerPanelViewModel = transparencyManagerPanelViewModel ?? Locator.CurrentMutable.GetService<ITransparencyManagerPanelViewModel>();

			_openConfigsInteraction = new Interaction<IConfigsViewModel, Unit>();

			// Settings
			_openConfigsCommand = ReactiveCommand.Create(() => {

				var _configsViewModel = Locator.CurrentMutable.GetService<IConfigsViewModel>();

				_openConfigsInteraction.Handle(_configsViewModel).Subscribe(result =>
				{
					_consolesPanelViewModel.LoadOptionsCommand.Execute().Subscribe();
				});
			});
		}

		public IConsolesPanelViewModel ConsolesPanel => _consolesPanelViewModel;

		public IMinimizedInstancesPanelViewModel MinimizedInstancesPanel => _minimizedInstancesPanel;

		public ITransparencyManagerPanelViewModel TransparencyManagerPanel => _transparencyManagerPanelViewModel;

		public Interaction<IConfigsViewModel, Unit> OpenConfigsInteraction => _openConfigsInteraction;

		public ReactiveCommand OpenConfigsCommand => _openConfigsCommand;
	}
}
