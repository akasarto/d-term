using AutoMapper;
using MaterialDesignThemes.Wpf;
using Processes.Core;
using ReactiveUI;
using Splat;
using System;
using System.Reactive;
using System.Threading.Tasks;
using UI.Wpf.Properties;

namespace UI.Wpf.Processes
{
	public interface IUtilitiesPanelViewModel
	{
		ReactiveCommand<string, IProcessInstanceViewModel> StartUtilityProcessCommand { get; }
	}

	public class UtilitiesPanelViewModel : ReactiveObject, IUtilitiesPanelViewModel
	{
		private readonly IAppState _appState;
		private readonly IProcessFactory _processFactory;
		private readonly IProcessesTracker _processesTracker;
		private readonly ISnackbarMessageQueue _snackbarMessageQueue;
		private readonly ReactiveCommand<string, IProcessInstanceViewModel> _startUtilityProcessCommand;

		/// <summary>
		/// Constructor method.
		/// </summary>
		public UtilitiesPanelViewModel(
			IAppState appState = null,
			IProcessFactory processFactory = null,
			IProcessesTracker processesTracker = null,
			ISnackbarMessageQueue snackbarMessageQueue = null)
		{
			_appState = appState ?? Locator.CurrentMutable.GetService<IAppState>();
			_processFactory = processFactory ?? Locator.CurrentMutable.GetService<IProcessFactory>();
			_processesTracker = processesTracker ?? Locator.CurrentMutable.GetService<IProcessesTracker>();
			_snackbarMessageQueue = snackbarMessageQueue ?? Locator.CurrentMutable.GetService<ISnackbarMessageQueue>();

			_startUtilityProcessCommand = ReactiveCommand.CreateFromTask<string, IProcessInstanceViewModel>(async (utilityPath) => await StartUtilityProcessCommandAction(utilityPath));
			_startUtilityProcessCommand.ThrownExceptions.Subscribe(@exception => StartUtilityProcessCommandError(@exception));
			_startUtilityProcessCommand.Subscribe(instance => StartUtilityProcessCommandHandler(instance));
		}

		public ReactiveCommand<string, IProcessInstanceViewModel> StartUtilityProcessCommand => _startUtilityProcessCommand;

		private Task<IProcessInstanceViewModel> StartUtilityProcessCommandAction(string utilityPath) => Task.Run(() =>
		{
			var parts = utilityPath.Split('@');
			var instance = default(IProcessInstanceViewModel);
			var process = _processFactory.Create(ProcessBasePath.App, parts[1]);

			if (process.Start())
			{
				instance = Mapper.Map<IProcess, IProcessInstanceViewModel>(process, context => context.AfterMap((source, target) =>
				{
					target.Name = parts[0];
				}));

				_processesTracker.Track(process.Id);

				return Task.FromResult(instance);
			}

			process.Kill();

			return Task.FromResult<IProcessInstanceViewModel>(null);
		});

		private void StartUtilityProcessCommandError(Exception exception)
		{
			// ToDo: Log exception
			_snackbarMessageQueue.Enqueue(Resources.ErrorCreatingInstance);
		}

		private void StartUtilityProcessCommandHandler(IProcessInstanceViewModel instance)
		{
			if (instance == null)
			{
				_snackbarMessageQueue.Enqueue(Resources.ProcessStartFailure);
				return;
			}

			_appState.AddProcessInstance(instance);
		}
	}
}
